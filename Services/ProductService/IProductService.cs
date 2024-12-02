using DarazApp.DTOs;
using DarazApp.Models;
using DarazApp.Responses;

namespace DarazApp.Services.ProductService
{
    public interface IProductService
    {
        Task<Product> AddProductAsync(Product product);
        Task<List<Product>> GetProductsById(int productId);

        Task<List<Product>> SearchProductsByNameAsync(string productName);

        Task<PagedResultDto<Product>> GetUsersWithPaginationAsync(PaginationQueryDto paginationQuery);


    }

}
