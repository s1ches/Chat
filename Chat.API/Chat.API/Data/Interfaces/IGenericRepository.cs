namespace Chat.API.Data.Interfaces;

public interface IGenericRepository<T>
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    
    Task<Guid> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task DeleteAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<T>> CreateAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    
    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
}