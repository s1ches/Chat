using Microsoft.EntityFrameworkCore;

namespace Chat.API.Data.Repositories;

public class ChatRepository(ChatDbContext dbContext)
    : GenericRepository<Domain.Entities.Chat>(dbContext)
{
    private readonly ChatDbContext _dbContext = dbContext;

    public async Task<List<Domain.Entities.Chat>> GetChatsByUserIdAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Chats
            .Include(x => x.Users)
            .AsNoTracking()
            .Where(x => x.Users.Select(u => u.Id).Contains(userId))
            .OrderByDescending(x => x.UpdateDate)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<Domain.Entities.Chat>> GetChatsByUserNameAsync(string userName,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Chats
            .Include(x => x.Users)
            .AsNoTracking()
            .Where(x => x.Users.Select(u => u.UserName).Contains(userName))
            .OrderByDescending(x => x.UpdateDate)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<bool> ExistsUserInChatAsync(Guid chatId, Guid userId, CancellationToken cancellationToken = default)
        => await _dbContext.Chats
            .Where(x => x.Id == chatId)
            .Where(x => x.Users.Select(u => u.Id).Contains(userId))
            .AnyAsync(cancellationToken);
}