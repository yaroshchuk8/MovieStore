using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MovieStore.Application.Common.Interfaces.Repositories;

namespace MovieStore.Infrastructure.Persistence.Repositories;

internal class BaseRepository<T>(MovieStoreDbContext context) : IBaseRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();
    
    public async Task<List<T>> GetAllAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IQueryable<T>>? includes = null,
        Func<IQueryable<T>, IQueryable<T>>? orderBy = null,
        bool asNoTracking = true)
    {
        IQueryable<T> query = _dbSet;

        if (includes != null)
        {
            query = includes(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }
        
        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.ToListAsync();
    }

    public async Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>>? predicate, 
        Func<IQueryable<T>, IQueryable<T>>? includes = null,
        Func<IQueryable<T>, IQueryable<T>>? orderBy = null,
        bool asNoTracking = true)
    {
        IQueryable<T> query = _dbSet;

        if (includes != null)
        {
            query = includes(query);
        }
        
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        
        if (orderBy != null)
        {
            query = orderBy(query);
        }
        
        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync();
    }
    
    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, 
        Func<IQueryable<T>, IQueryable<T>>? includes = null)
    {
        IQueryable<T> query = _dbSet;

        if (includes != null)
        {
            query = includes(query);
        }
        
        return await query.AnyAsync(predicate);
    }
    
    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IQueryable<T>>? includes = null)
    {
        IQueryable<T> query = _dbSet;

        if (includes != null)
        {
            query = includes(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.CountAsync();
    }
    
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }
    
    public async Task AddRangeAsync(List<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void UpdateRange(List<T> entities)
    {
        _dbSet.UpdateRange(entities);
    }
    
    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
}