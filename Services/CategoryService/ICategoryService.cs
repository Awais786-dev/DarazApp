using DarazApp.DTOs;
using DarazApp.Models;

namespace DarazApp.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<List<Category>> GetTopLevelCategoriesAsync();
      
        Task<List<Category>> GetSubcategoriesAsync(int categoryId);
        
        Task<List<Product>> GetProductsForCategoryAsync(int categoryId);

        Task<Category> AddCategoryAsync(Category category);

        Task<Category> GetCategoryByNameAsync(string name);

        Task<PagedResultDto<Category>> GetUsersWithPaginationAsync(PaginationQueryDto paginationQuery);

        Task<Category> GetCategoryByIdAsync(int id);
    }
}
