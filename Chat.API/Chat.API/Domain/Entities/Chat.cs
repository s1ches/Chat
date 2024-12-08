using Chat.API.Domain.BaseEntities;

namespace Chat.API.Domain.Entities;

public class Chat : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    
    public List<Message> Messages { get; set; } = [];
    
    public List<User> Users { get; set; } = [];
}