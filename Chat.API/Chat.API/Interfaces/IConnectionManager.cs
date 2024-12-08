namespace Chat.API.Interfaces;

public interface IConnectionManager
{ 
    Task AddConnectionAsync(string connectionId, Guid userId, CancellationToken cancellationToken = default);
    
    Task RemoveConnectionAsync(Guid userId, CancellationToken cancellationToken = default);
}