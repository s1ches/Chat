using Chat.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Data.Repositories;

public class UserRepository(ChatDbContext dbContext)
    : GenericRepository<User>(dbContext)
{
    private readonly ChatDbContext _dbContext = dbContext;

    public async Task<User?> GetUserByNameAsync(string name, CancellationToken cancellationToken = default)
        => await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserName == name, cancellationToken: cancellationToken);

    public async Task<bool> HasUserWithNameAsync(string name, CancellationToken cancellationToken = default)
        => await _dbContext.Users.AnyAsync(x => x.UserName == name, cancellationToken);
    
    public async Task<List<User>> GetUsersByNamesAsync(IEnumerable<string> names, CancellationToken cancellationToken = default)
    {
        var userNames = names.ToList();
        
        return await _dbContext.Users
            .AsNoTracking()
            .Where(x => userNames.Contains(x.UserName))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<string?>> GetUsersConnectionsByNamesAsync(IEnumerable<string> names,
        CancellationToken cancellationToken = default)
    {
        var userNames = names.ToList();
        
        return await _dbContext.Users
            .AsNoTracking()
            .Where(x => userNames.Contains(x.UserName))
            .Select(x => x.ConnectionId)
            .ToListAsync(cancellationToken);
    }
}