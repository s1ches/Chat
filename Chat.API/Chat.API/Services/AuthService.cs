using System.Net;
using Chat.API.Data.Interfaces;
using Chat.API.Domain.Entities;
using Chat.API.Domain.Enums;
using Chat.API.Exceptions;
using Chat.API.Interfaces;
using Chat.API.Options;
using Chat.API.Requests.Auth;
using Chat.API.Requests.Auth.Login;
using Chat.API.Requests.Auth.Register;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Chat.API.Services;

public class AuthService(
    IUnitOfWork unitOfWork,
    IJwtProvider jwtProvider,
    IPasswordHasher passwordHasher,
    IClaimsManager claimsManager,
    IOptions<AuthOptions> authOptions) : IAuthService
{
    private readonly AuthOptions _authOptions = authOptions.Value;
    
    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.UserName))
            throw new BaseException("Username is required", HttpStatusCode.BadRequest);

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new BaseException("Password is required", HttpStatusCode.BadRequest);

        var user = await unitOfWork.UserRepository.GetUserByNameAsync(request.UserName, cancellationToken);

        if (user == null)
            throw new BaseException("Invalid username or password", HttpStatusCode.BadRequest);

        if (!passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            throw new BaseException("Invalid username or password", HttpStatusCode.BadRequest);

        var claims = claimsManager.GetClaims(user);

        user.AccessToken = jwtProvider.GenerateToken(claims);
        user.RefreshToken = jwtProvider.GenerateRefreshToken();
        
        var executionStrategy = unitOfWork.CreateExecutionStrategy();
        await executionStrategy.ExecuteAsync(async () => 
        { 
            var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                await unitOfWork.UserRepository.UpdateAsync(user, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            } 
        });

        return new AuthResponse
        {
            AccessToken = user.AccessToken,
        };
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.UserName))
            throw new BaseException("Username is required", HttpStatusCode.BadRequest);

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new BaseException("Password is required", HttpStatusCode.BadRequest);

        if (string.IsNullOrWhiteSpace(request.RepeatedPassword))
            throw new BaseException("Repeated password is required", HttpStatusCode.BadRequest);

        if (request.Password.Length < _authOptions.MinPasswordLength)
            throw new BaseException($"Password is too short(min length: {_authOptions.MinPasswordLength})",
                HttpStatusCode.BadRequest);

        if (request.Password != request.RepeatedPassword)
            throw new BaseException("Passwords do not match", HttpStatusCode.BadRequest);

        if (await unitOfWork.UserRepository.HasUserWithNameAsync(request.UserName, cancellationToken))
            throw new BaseException("Username is taken", HttpStatusCode.Conflict);

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName,
            PasswordHash = passwordHasher.HashPassword(request.Password),
            Role = Role.User,
            CreateDate = DateTime.UtcNow,
            UpdateDate = DateTime.UtcNow,
            IsBlocked = false
        };

        var claims = claimsManager.GetClaims(user);

        user.AccessToken = jwtProvider.GenerateToken(claims);
        user.RefreshToken = jwtProvider.GenerateRefreshToken();
        user.RefreshTokenExpiry = DateTime.UtcNow.Add(TimeSpan.FromDays(_authOptions.RefreshTokenLifetimeDays));

        var executionStrategy = unitOfWork.CreateExecutionStrategy();
        await executionStrategy.ExecuteAsync(async () => 
        { 
            var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                await unitOfWork.UserRepository.CreateAsync(user, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
        
        return new AuthResponse { AccessToken = user.AccessToken };
    }

    public async Task<AuthResponse> RefreshAccessTokenAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken: cancellationToken);
        
        if(user == null)
            throw new BaseException("User not found", HttpStatusCode.BadRequest);
        
        if(user.RefreshTokenExpiry < DateTime.UtcNow)
            throw new BaseException("Refresh token has expired", HttpStatusCode.Forbidden);
        
        var claims = claimsManager.GetClaims(user);
        user.AccessToken = jwtProvider.GenerateToken(claims);
        user.RefreshToken = jwtProvider.GenerateRefreshToken();
        user.RefreshTokenExpiry = DateTime.UtcNow.Add(TimeSpan.FromDays(_authOptions.RefreshTokenLifetimeDays));
        
        var executionStrategy = unitOfWork.CreateExecutionStrategy();
        await executionStrategy.ExecuteAsync(async () => 
        { 
            var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                await unitOfWork.UserRepository.UpdateAsync(user, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
        
        return new AuthResponse { AccessToken = user.AccessToken };
    }
}