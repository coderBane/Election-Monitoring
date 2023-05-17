using System.Linq.Expressions;

namespace Election2023.Application.Interfaces.Repositories;

public interface IRepository<T> where T : class
{
    IQueryable<T> Table { get; }

    IQueryable<T> TableNoTracking { get; }

    IEnumerable<T> Find(Expression<Func<T, bool>> filter);

    Task<IEnumerable<T>> GetAllAsync();

    Task<T?> GetAsync(object id);

    Task<bool> AddAsync(T entity);

    Task UpdateAsync(object id, T entity);

    Task<bool> DeleteAsync(object id);
}

