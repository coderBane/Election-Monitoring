using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Election2023.DataStore.Database;
using Election2023.Application.Interfaces.Repositories;


namespace Election2023.DataStore.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ElectionDbContext _dbContext;
    protected internal ILogger _logger;

	public Repository(ElectionDbContext dbContext, ILogger logger)
	{
        _logger = logger;
        _dbContext = dbContext;
        _logger.LogDebug($"Intialized {nameof(T)} Repository");
	}

    protected virtual DbSet<T> DbSet => _dbContext.Set<T>();

    public IQueryable<T> Table => DbSet;

    public IQueryable<T> TableNoTracking => DbSet.AsNoTracking();

    public virtual async Task<bool> AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        return true;
    }

    public virtual async Task<bool> DeleteAsync(object id)
    {
        T? entity = await DbSet.FindAsync(id);
        if (entity is null) return false;

        DbSet.Remove(entity);
        return true;
    }

    public virtual IEnumerable<T> Find(Expression<Func<T, bool>> filter)
    {
        return DbSet.Where(filter) ?? Enumerable.Empty<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await DbSet.AsNoTracking().ToListAsync() ?? Enumerable.Empty<T>();
    }

    public virtual async Task<T?> GetAsync(object id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual Task UpdateAsync(object id, T entity)
    {
        T? exists = DbSet.Find(id);
        if (exists is null)
            return Task.FromException(new NullReferenceException($"{nameof(exists)} is null"));

        _dbContext.Entry(exists).CurrentValues.SetValues(entity);
        return Task.CompletedTask;
    }
}

