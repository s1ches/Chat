using Chat.API.Data.Interfaces;
using Chat.API.Domain.BaseEntities;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Data.Repositories;

public class GenericRepository<T>(ChatDbContext dbContext)
    : IGenericRepository<T> where T : BaseEntity, new()
{
    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) 
        => await dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Set<T>().AsNoTracking().AnyAsync(x => x.Id == id, cancellationToken);

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        => await dbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);

    public virtual async Task<Guid> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);

        if (entity != null)
            dbContext.Set<T>().Remove(entity);

        return id;
    }

    public virtual Task DeleteAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        dbContext.Set<T>().RemoveRange(entities);
        return Task.CompletedTask;
    }

    public virtual Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var entry = dbContext.Set<T>().Add(entity);
        return Task.FromResult(entry.Entity);
    }

    public virtual async Task<IEnumerable<T>> CreateAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        var newEntities = entities.ToList();
        await dbContext.Set<T>().AddRangeAsync(newEntities, cancellationToken);
        return newEntities;
    }

    public virtual Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var entry = dbContext.Set<T>().Update(entity);
        return Task.FromResult(entry.Entity);
    }

    public virtual Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        var updatedEntities = entities.ToList();
        dbContext.Set<T>().UpdateRange(updatedEntities);
        return Task.FromResult<IEnumerable<T>>(updatedEntities);
    }
}