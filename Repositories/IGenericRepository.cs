using DarazApp.DTOs;
using System.Linq.Expressions;

namespace DarazApp.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<PagedResultDto<T>> GetWithPaginationAsync(PaginationQueryDto paginationQuery);

        IQueryable<TEntity> FindByConditionAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

       // Task<List<T>> FindByConditionsAsync(string conditions);

        //Task<List<T>> SearchByNameAsync(Expression<Func<T, string>> nameSelector, string searchKeyword);
    }

}
