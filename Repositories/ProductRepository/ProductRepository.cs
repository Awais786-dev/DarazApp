using DarazApp.DbContext;
using DarazApp.DTOs;
using DarazApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DarazApp.Repositories.ProductRepository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly DbContext.DbContext _context;


        public ProductRepository(DbContext.DbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return await _context.Products
                                 .Where(p => p.CategoryId == categoryId)
                                 .ToListAsync();
        }

        public async Task<List<Product>> SearchProductsByNameAsync(string productName)
        {
            // Use the LIKE operator to search for products whose name contains the provided term
            return await _context.Products
                .Where(p => p.Name.Contains(productName))  // SQL LIKE equivalent
                .ToListAsync();
        }



    }

}
