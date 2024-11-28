using DarazApp.Models;
using DarazApp.Repositories.ProductRepository;
using Microsoft.EntityFrameworkCore;

namespace DarazApp.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

       public async Task<Product> AddProductAsync(Product product)
        {

            Product addedproduct= await _productRepository.AddProductAsync(product);
            return addedproduct;
        }


        public async Task<List<Product>> GetProductsForCategoryAsync(int categoryId)
        {
            return await _productRepository.GetProductsByCategoryIdAsync(categoryId);
        }

    }
}
