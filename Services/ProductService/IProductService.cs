using DarazApp.Models;

namespace DarazApp.Services.ProductService
{
    public interface IProductService
    {
        Task<Product> AddProductAsync(Product product);
        Task<List<Product>> GetProductsForCategoryAsync(int categoryId);

    
    
    }

}
