using DarazApp.Models;

namespace DarazApp.Repositories.CategoryRepository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetTopLevelCategoriesAsync();
        Task<List<Category>> GetSubcategoriesAsync(int categoryId);


        Task<Category> AddCategoryAsync(Category category);
        Task<Category> GetCategoryByNameAsync(string name);
        Task<Category> GetCategoryByIdAsync(int id);
    }
}
