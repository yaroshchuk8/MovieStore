using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MovieStore.Application.Common.Interfaces.Repositories;

namespace MovieStore.Infrastructure.Common.Persistence.Repositories;

internal class BaseRepository<T>(MovieStoreDbContext context) : IBaseRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();
    
    public async Task<List<T>> GetAllAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IQueryable<T>>? includes = null,
        Func<IQueryable<T>, IQueryable<T>>? orderBy = null,
        int? skip = null,
        int? take = null,
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

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }
        
        // Apply Sorting (Crucial for Skip/Take)
        if (skip.HasValue || take.HasValue)
        {
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            else
            {
                // Automated Metadata-driven sorting
                var pk = context.Model.FindEntityType(typeof(T))?.FindPrimaryKey();
                if (pk != null)
                {
                    IOrderedQueryable<T>? orderedQuery = null;
                    for (int i = 0; i < pk.Properties.Count; i++)
                    {
                        var propertyName = pk.Properties[i].Name;
                        orderedQuery = i == 0 
                            ? query.OrderBy(x => EF.Property<object>(x, propertyName)) 
                            : orderedQuery!.ThenBy(x => EF.Property<object>(x, propertyName));
                    }
                    query = orderedQuery ?? query;
                }
            }
        }
        else if (orderBy != null)
        {
            query = orderBy(query);
        }

        if (skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }
        
        if (take.HasValue)
        {
            query = query.Take(take.Value);
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
    
    public async Task<bool> ExistsAsync(
        Expression<Func<T, bool>> predicate, 
        Func<IQueryable<T>, IQueryable<T>>? includes = null)
    {
        IQueryable<T> query = _dbSet;

        if (includes != null)
        {
            query = includes(query);
        }
        
        return await query.AnyAsync(predicate);
    }
    
    public async Task<int> CountAsync(
        Expression<Func<T, bool>>? predicate = null,
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