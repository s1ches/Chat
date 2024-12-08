using Chat.API.Interfaces;

namespace Chat.API.Services;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);
    
    public bool VerifyPassword(string password, string hash)
        => BCrypt.Net.BCrypt.Verify(text: password, hash: hash);
}