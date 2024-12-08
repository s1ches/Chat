using Chat.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Data.Repositories;

public class MessageRepository(ChatDbContext dbContext)
    : GenericRepository<Message>(dbContext)
{
    private readonly ChatDbContext _dbContext = dbContext;

    public async Task<IEnumerable<Message>> GetByChatIdAsync(Guid chatId, CancellationToken cancellationToken = default)
        => await _dbContext.Messages.AsNoTracking()
            .Include(x => x.Chat)
            .Include(x => x.User)
            .Where(x => x.Chat.Id == chatId)
            .OrderByDescending(x => x.CreateDate)
            .ToListAsync(cancellationToken: cancellationToken);
}