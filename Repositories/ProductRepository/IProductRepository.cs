using DarazApp.Models;

namespace DarazApp.Repositories.ProductRepository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId);
        Task<Product> AddProductAsync(Product product);

    }
}
