using System.Net;
using Chat.API.Data.Interfaces;
using Chat.API.Exceptions;
using Chat.API.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Chat.API.Services;

public class ConnectionManager(IUnitOfWork unitOfWork) : IConnectionManager
{
    public async Task AddConnectionAsync(string connectionId, Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);
        
        if(user == null)
            throw new BaseException("User not found", HttpStatusCode.BadRequest);

        var executionStrategy = unitOfWork.CreateExecutionStrategy();
        await executionStrategy.ExecuteAsync(async () =>
        {
            var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                user.ConnectionId = connectionId;
                await unitOfWork.UserRepository.UpdateAsync(user, cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }

    public async Task RemoveConnectionAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);
        
        if(user == null)
            throw new BaseException("User not found", HttpStatusCode.BadRequest);

        var executionStrategy = unitOfWork.CreateExecutionStrategy();
        await executionStrategy.ExecuteAsync(async () =>
        {
            var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                user.ConnectionId = null;
                await unitOfWork.UserRepository.UpdateAsync(user, cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }
}