using FreelanceAPI.Requests;
using FreelanceAPI.Responses;
using FreelanceAPI.Services.Interface;
using FreelanceMarketplace.API.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace FreelanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        
        public async Task<IActionResult> Register( RegisterRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(CollectModelErrors());
            var result = await _authService.RegisterAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(CollectModelErrors());
            var result = await _authService.LoginAsync(dto);
            return result.IsSuccess ? Ok(result) : Unauthorized(result);
        }
        [HttpPost("refresh-token")]
       
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout() { 
       
var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            var result = await _authService.LogoutAsync(userId);
            return result ? Ok(new { message = "Logged out successfully" }) : BadRequest();
        }
        private AuthResponse CollectModelErrors()
        {
            var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();
            return new AuthResponse
            {
                IsSuccess = false,
                Message = "Validation failed",
                Errors = errors
            };
        }
    }
}

