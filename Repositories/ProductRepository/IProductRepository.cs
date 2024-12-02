using DarazApp.DTOs;
using DarazApp.Models;

namespace DarazApp.Repositories.ProductRepository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsByIdAsync(int productId);
        Task<Product> AddProductAsync(Product product);


        //for category Products
        Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId);


        Task<List<Product>> SearchProductsByNameAsync(string productName);

        Task<Product> UpdateProductAsync(Product updatedProduct);

        Task<PagedResultDto<Product>> GetUsersWithPaginationAsync(PaginationQueryDto paginationQuery);

    }
}
