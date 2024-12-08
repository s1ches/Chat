using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Chat.API.Interfaces;
using Chat.API.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Chat.API.Services;

public class JwtProvider(IOptions<JwtOptions> jwtOptions) : IJwtProvider
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    
    public string GenerateToken(List<Claim> userClaims)
    {
        var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
        
        var jwt = new JwtSecurityToken(issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.Now.AddMinutes(_jwtOptions.AccessTokenLifetimeMinutes),
            claims: userClaims,
            signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}