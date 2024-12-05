using DarazApp.DTOs;
using System.Linq.Expressions;

namespace DarazApp.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdWithIncludesAsync(int id, Func<IQueryable<T>, IQueryable<T>> includeChain);
        Task<List<T>> FindWithIncludesAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> includeChain);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<PagedResultDto<T>> GetWithPaginationAsync(PaginationQueryDto paginationQuery, Func<IQueryable<T>, IQueryable<T>> includeChain = null);
        IQueryable<TEntity> FindByConditionAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

    }

}
