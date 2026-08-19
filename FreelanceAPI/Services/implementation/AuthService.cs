using FreelanceAPI.Models;
using FreelanceAPI.Requests;
using FreelanceAPI.Responses;
using FreelanceAPI.Services.Interface;
using FreelanceMarketplace.API.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace FreelanceAPI.Services.implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<AuthService> _logger;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signInManager;
        public AuthService(
            UserManager<User> userManager,
            RoleManager<ApplicationRole> roleManager,
            ILogger<AuthService> logger,
           ITokenService tokenService,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequestDto dto)
        {
            // 1) التأكد إن الإيميل مش مستخدم قبل كده
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Email is already registered",
                    Errors = new List<string> { "A user with this email already exists." }
                };
            }

            // 2) التأكد إن الـ Role المطلوبة موجودة في النظام
            var roleExists = await _roleManager.RoleExistsAsync(dto.Role);
            if (!roleExists)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Invalid role specified",
                    Errors = new List<string> { $"Role '{dto.Role}' does not exist." }
                };
            }

            // 3) بناء الـ ApplicationUser Entity
            var user = new User
            {
                UserName = dto.Email, // بنستخدم الإيميل كـ UserName لتبسيط الـ Login
                Email = dto.Email,
                Name = dto.FullName,
                CreatedAt = DateTime.UtcNow,

            };

            // 4) إنشاء الـ User عن طريق UserManager
            // UserManager.CreateAsync بيعمل الآتي داخليًا:
            //   - Validation للباسورد حسب الـ Password Policy في Program.cs
            //   - Hashing للباسورد عن طريق PasswordHasher<TUser>
            //   - INSERT في جدول AspNetUsers
            //   - توليد SecurityStamp و ConcurrencyStamp تلقائيًا
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                _logger.LogWarning("Registration failed for {Email}: {Errors}",
                    dto.Email, string.Join(", ", errors));

                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Registration failed",
                    Errors = errors
                };
            }

            // 5) إضافة الـ User للـ Role المطلوبة (Seller أو Client)
            var roleResult = await _userManager.AddToRoleAsync(user, dto.Role);
            if (!roleResult.Succeeded)
            {
                // Rollback يدوي: لو فشل تعيين الـ Role، نحذف الـ User اللي اتعمل عشان منسيبش بيانات ناقصة
                await _userManager.DeleteAsync(user);

                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Failed to assign role",
                    Errors = roleResult.Errors.Select(e => e.Description).ToList()
                };
            }

            _logger.LogInformation("New user registered: {Email} as {Role}", dto.Email, dto.Role);

            // 6) إرجاع النتيجة (لسه من غير JWT - هنضيفه في Phase 5 الخاص بالـ Login)
            return new AuthResponse
            {
                IsSuccess = true,
                Message = "Registration successful",
                UserId = user.Id,
                Email = user.Email,
                FullName = user.Name,
                Role = dto.Role
            };
        }
        public async Task<AuthResponse> LoginAsync(LoginDto dto)
        {
           
var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
            
return new AuthResponse
{
    IsSuccess = false,
    Message = "Invalid email or password",
    Errors = new List<string> { "Invalid credentials" }
};
            }
           
if (!user.IsActive)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Account is deactivated",
                    Errors = new List<string> { "This account has been deactivated. Contact support." }
                };
            }
            
if (await _userManager.IsLockedOutAsync(user))
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Account is locked",
                    Errors = new List<string> { "Too many failed attempts. Try again later." }
                };
            }
 
var signInResult = await _signInManager.CheckPasswordSignInAsync(
user, dto.Password, lockoutOnFailure: true);
            if (!signInResult.Succeeded)
            {
                if (signInResult.IsLockedOut)
                {
                    return new AuthResponse
                    {
                        IsSuccess = false,
                        Message = "Account locked due to multiple failed attempts",
                        Errors = new List<string> { "Try again after 15 minutes." }
                    };
                }
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Invalid email or password",
                    Errors = new List<string> { "Invalid credentials" }
                };
            }
        
var roles = await _userManager.GetRolesAsync(user);
            // 6) الـ توليد Access Token والـ Refresh Token
            var accessToken = await _tokenService.GenerateAccessTokenAsync(user, roles);
            var refreshToken = _tokenService.GenerateRefreshToken();
            
user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);
            _logger.LogInformation("User logged in: {Email}", dto.Email);
            return new AuthResponse
            {
                IsSuccess = true,
                Message = "Login successful",
                UserId = user.Id,
                Email = user.Email,
                FullName = user.Name,
                Role = roles.FirstOrDefault(),
                Token = accessToken,
                RefreshToken = refreshToken,
                TokenExpiration = DateTime.UtcNow.AddMinutes(15)
            };

        }
        public async Task<bool> LogoutAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;
    
user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await _userManager.UpdateAsync(user);
            return true;
        }

      
    }
}
