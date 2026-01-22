using System.Linq.Expressions;

namespace MovieStore.Application.Common.Interfaces.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<List<T>> GetAllAsync(
        Expression<Func<T, bool>>? predicate = null,
        // Func<IQueryable<T>, IQueryable<T>>? includes = null,
        List<string>? includes = null,
        Func<IQueryable<T>, IQueryable<T>>? orderBy = null,
        int? skip = null,
        int? take = null,
        bool asNoTracking = true
    );

    Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>>? predicate,
        // Func<IQueryable<T>, IQueryable<T>>? includes = null,
        List<string>? includes = null,
        Func<IQueryable<T>, IQueryable<T>>? orderBy = null,
        bool asNoTracking = true
    );

    Task<bool> ExistsAsync(
        Expression<Func<T, bool>> predicate,
        // Func<IQueryable<T>, IQueryable<T>>? includes = null
        List<string>? includes = null
    );
    
    Task<int> CountAsync(
        Expression<Func<T, bool>>? predicate = null,
        // Func<IQueryable<T>, IQueryable<T>>? includes = null
        List<string>? includes = null
    );
    
    Task AddAsync(T entity);
    Task AddRangeAsync(List<T> entity);
    void Add(T entity);
    void Update(T entity);
    void UpdateRange(List<T> entities);
    void Delete(T entity);
    // Task<bool> SaveChangesAsync();
}