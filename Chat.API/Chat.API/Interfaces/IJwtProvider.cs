using System.Security.Claims;

namespace Chat.API.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(List<Claim> userClaims);
    
    string GenerateRefreshToken();
}