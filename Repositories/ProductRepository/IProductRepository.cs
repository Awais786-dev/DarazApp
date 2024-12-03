using DarazApp.DTOs;
using DarazApp.Models;

namespace DarazApp.Repositories.ProductRepository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        //for category Products
        Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId);   

        Task<List<Product>> SearchProductsByNameAsync(string productName);    

    }
}
