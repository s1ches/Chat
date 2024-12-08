using System.Net;
using System.Security.Claims;
using Chat.API.Domain.Entities;
using Chat.API.Exceptions;
using Chat.API.Interfaces;

namespace Chat.API.Services;

public class ClaimsManager : IClaimsManager
{
    public List<Claim> GetClaims(User user)
        =>
        [
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Role, user.Role.ToString())
        ];

    public Guid GetUserId(ClaimsPrincipal user)
        => Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                      throw new BaseException("Invalid access token", HttpStatusCode.Forbidden));

    public string GetUserName(ClaimsPrincipal user)
        => user.FindFirst(ClaimTypes.Name)?.Value ??
           throw new BaseException("Invalid access token", HttpStatusCode.Forbidden);
}