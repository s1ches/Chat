using Chat.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Data;

public class ChatDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Message> Messages { get; set; } = null!;

    public DbSet<Domain.Entities.Chat> Chats { get; set; } = null!;
    
    public ChatDbContext() 
    { }
    
    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
    { }
}