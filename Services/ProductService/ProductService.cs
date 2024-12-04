using DarazApp.DTOs;
using DarazApp.Models;
using DarazApp.Repositories;
using DarazApp.Responses;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace DarazApp.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepository;

        public ProductService(IGenericRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> AddProductAsync(Product product)
        {

            Product addedproduct = await _productRepository.AddAsync(product);
            return addedproduct;
        }


        public async Task<Product> GetProductsById(int productId)
        {
            return await _productRepository.GetByIdAsync(productId);
        }


        public async Task<List<Product>> SearchProductsByNameAsync(string productName)
        {
            if (string.IsNullOrEmpty(productName))
            {
                throw new Exception("Name of product can not empty.");
            }

            List<Product> products = await _productRepository.FindByConditionAsync<Product>(p => p.Name.Contains(productName)).ToListAsync();

            if (products == null || !products.Any())
            {
                throw new Exception("No products found matching the search criteria.");
            }

            return products;
        }


        public async Task<PagedResultDto<Product>> GetProductsWithPaginationAsync(PaginationQueryDto paginationQuery)
        {
            return await _productRepository.GetWithPaginationAsync(paginationQuery);
        }
    }
}
