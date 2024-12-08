namespace Chat.API.Options;

public class DataContextOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    
    public int MaxRetryCountOnFailure { get; set; } = 0;
    
    public int RetryDelayInSeconds { get; set; } = 0;
}