using DarazApp.Models;

namespace DarazApp.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<List<Category>> GetTopLevelCategoriesAsync();
        Task<List<Category>> GetSubcategoriesAsync(int categoryId);
        Task<List<Product>> GetProductsForCategoryAsync(int categoryId);
      //  Task AddProductAsync(Product product);



        Task<Category> AddCategoryAsync(Category category);
        Task<Category> GetCategoryByNameAsync(string name);
        Task<Category> GetCategoryByIdAsync(int id);
    }
}
