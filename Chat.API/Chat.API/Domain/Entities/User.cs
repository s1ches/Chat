using Chat.API.Domain.BaseEntities;
using Chat.API.Domain.Enums;

namespace Chat.API.Domain.Entities;

public class User : BaseAuditableEntity
{
    public string UserName { get; set; } = string.Empty;
    
    public string? ConnectionId { get; set; } = string.Empty;
    
    public Role Role { get; set; }
    
    public string PasswordHash {get; set;} = string.Empty;
    
    public List<Chat> Chats { get; set; } = [];
    
    public bool IsBlocked { get; set; }
    
    public string RefreshToken { get; set; } = string.Empty;
    
    public DateTime RefreshTokenExpiry { get; set; }
    
    public string AccessToken { get; set; } = string.Empty;
}