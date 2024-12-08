using System.ComponentModel.DataAnnotations.Schema;
using Chat.API.Domain.BaseEntities;

namespace Chat.API.Domain.Entities;

public class Message : BaseAuditableEntity
{
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    
    public User User { get; set; } = null!;
    
    [ForeignKey(nameof(Chat))]
    public Guid ChatId { get; set; }
    
    public Chat Chat { get; set; } = null!;
    
    public string MessageContent { get; set; } = string.Empty;
}