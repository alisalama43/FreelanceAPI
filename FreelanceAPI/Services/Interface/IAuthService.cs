using FreelanceAPI.Requests;
using FreelanceAPI.Responses;
using FreelanceMarketplace.API.DTOs.Auth;


namespace FreelanceAPI.Services.Interface
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequestDto registerRequest);
        Task<AuthResponse> LoginAsync(LoginDto dto);
        
        Task<bool> LogoutAsync(string userId);
    }
}
