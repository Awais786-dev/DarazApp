using DarazApp.DbContext;
using DarazApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DarazApp.Repositories.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly UserDbContext _context;

        public ProductRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return await _context.Products
                                 .Where(p => p.CategoryId == categoryId)
                                 .ToListAsync();
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }
    }

}
