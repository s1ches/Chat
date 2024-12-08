using System.Security.Claims;
using Chat.API.Domain.Entities;

namespace Chat.API.Interfaces;

public interface IClaimsManager
{
    List<Claim> GetClaims(User user);

    Guid GetUserId(ClaimsPrincipal user);
    
    string GetUserName(ClaimsPrincipal user);
}