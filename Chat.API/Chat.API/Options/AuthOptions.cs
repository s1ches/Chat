namespace Chat.API.Options;

public class AuthOptions
{
    public int MinPasswordLength { get; set; } = 6;
    
    public int RefreshTokenLifetimeDays { get; set; } = 1;
}