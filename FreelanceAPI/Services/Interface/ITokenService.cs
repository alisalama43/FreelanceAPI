using FreelanceAPI.Models;
using System.Security.Claims;

namespace FreelanceAPI.Services.Interface
{
    public interface ITokenService
    {
        Task<string> GenerateAccessTokenAsync(User user, IList<string> roles);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }

}
