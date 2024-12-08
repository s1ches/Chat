namespace Chat.API.Interfaces;

public interface IPasswordHasher
{
    string HashPassword(string password);

    public bool VerifyPassword(string password, string hash);
}