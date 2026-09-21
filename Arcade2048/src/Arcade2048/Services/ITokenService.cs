using Arcade2048.Domain.Users;
using System.Security.Claims;


namespace Arcade2048.Services

{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromAccessToken(string token);

    }
}
