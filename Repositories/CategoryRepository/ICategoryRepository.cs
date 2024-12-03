using DarazApp.DTOs;
using DarazApp.Models;

namespace DarazApp.Repositories.CategoryRepository
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<List<Category>> GetTopLevelCategoriesAsync(); // not generic

        Task<List<Category>> GetSubcategoriesAsync(int categoryId);   //not generic

        Task<Category> GetCategoryByNameAsync(string name);   //not generic

    }
}
