using Chat.API.Data.Interfaces;
using Chat.API.Data.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Chat.API.Data;

public class UnitOfWork: IUnitOfWork
{
    private readonly ChatDbContext _dbContext;
    
    public ChatRepository ChatRepository { get; init; }
    
    public UserRepository UserRepository { get; init; }
    
    public MessageRepository MessageRepository { get; init; }

    public UnitOfWork(ChatDbContext dbContext,
        ChatRepository chatRepository,
        UserRepository userRepository,
        MessageRepository messageRepository)
    {
        ChatRepository = chatRepository;
        UserRepository = userRepository;
        MessageRepository = messageRepository;
        _dbContext = dbContext;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public IExecutionStrategy CreateExecutionStrategy()
    {
        return _dbContext.Database.CreateExecutionStrategy();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.RollbackTransactionAsync(cancellationToken);
    }
}