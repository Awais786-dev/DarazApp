using DarazApp.DTOs;
using DarazApp.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;


public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DarazApp.DbContext.DbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(DarazApp.DbContext.DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    //public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    //{
    //    return await _dbSet.Where(predicate).ToListAsync();
    //}

    // new added
    //public async Task<List<TEntity>> FindByConditionAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
    //{
    //    return await _context.Set<TEntity>().Where(predicate).ToListAsync();
    //}
    public IQueryable<TEntity> FindByConditionAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
    {
        return _context.Set<TEntity>().Where(predicate);
    }


    public async Task<PagedResultDto<T>> GetWithPaginationAsync(PaginationQueryDto paginationQuery)
    {
        string searchConditions = paginationQuery.SearchKeyword;
        int pageNumber = paginationQuery.PageNumber;
        int pageSize = paginationQuery.PageSize;
        string sortBy = paginationQuery.SortBy;
        bool ascending = paginationQuery.Ascending;

        IQueryable<T> query = _dbSet.AsQueryable();

        // Search (if applicable to the entity)
        if (!string.IsNullOrEmpty(searchConditions))
        {
            query = query.Where(searchConditions);
        }

        // Sorting
        if (!string.IsNullOrEmpty(sortBy))
        {
            if (ascending)
            {
                query = query.OrderBy(e => EF.Property<object>(e, sortBy));
            }
            else
            {
                query = query.OrderByDescending(e => EF.Property<object>(e, sortBy));
            }
        }

        // Pagination
        int totalRecords = await query.CountAsync();
        int skip = (pageNumber - 1) * pageSize;
        List<T> items = await query.Skip(skip).Take(pageSize).ToListAsync();

        return new PagedResultDto<T>
        {
            Items = items,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}
