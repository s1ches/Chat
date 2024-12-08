using Chat.API.Interfaces;
using Chat.API.Requests.Auth;
using Chat.API.Requests.Auth.Login;
using Chat.API.Requests.Auth.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService, IClaimsManager claimsManager) : ControllerBase
{
    [HttpPost("login")]
    public async Task<AuthResponse> Login(LoginRequest request, CancellationToken cancellationToken)
        => await authService.LoginAsync(request, cancellationToken);
    
    [HttpPost("register")]
    public async Task<AuthResponse> Register(RegisterRequest request, CancellationToken cancellationToken)
        => await authService.RegisterAsync(request, cancellationToken);

    [HttpPost("refresh")]
    [Authorize]
    public async Task<AuthResponse> RefreshAccessToken(CancellationToken cancellationToken)
        => await authService.RefreshAccessTokenAsync(claimsManager.GetUserId(HttpContext.User), cancellationToken);
}