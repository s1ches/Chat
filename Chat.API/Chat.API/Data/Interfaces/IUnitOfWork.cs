using Chat.API.Data.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Chat.API.Data.Interfaces;

public interface IUnitOfWork
{
    public ChatRepository ChatRepository { get; init; }

    public UserRepository UserRepository { get; init; }
    
    public MessageRepository MessageRepository { get; init; }
    
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    
    IExecutionStrategy CreateExecutionStrategy();
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}