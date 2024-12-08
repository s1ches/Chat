using Chat.API.Requests.Auth;
using Chat.API.Requests.Auth.Login;
using Chat.API.Requests.Auth.Register;

namespace Chat.API.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    
    Task<AuthResponse> RefreshAccessTokenAsync(Guid userId, CancellationToken cancellationToken = default);
}