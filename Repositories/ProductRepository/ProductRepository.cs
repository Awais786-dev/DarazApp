using DarazApp.DbContext;
using DarazApp.DTOs;
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

        public async Task<List<Product>> GetProductsByIdAsync(int productId)
        {
            return await _context.Products
                                 .Where(p => p.Id == productId)
                                 .ToListAsync();
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


        public async Task<List<Product>> SearchProductsByNameAsync(string productName)
        {
            // Use the LIKE operator to search for products whose name contains the provided term
            return await _context.Products
                .Where(p => p.Name.Contains(productName))  // SQL LIKE equivalent
                .ToListAsync();
        }


        public async Task<Product> UpdateProductAsync(Product updatedProduct)
        {
            // Ensure the product exists in the database by its Id
            Product existingProduct = await _context.Products
                                                 .FirstOrDefaultAsync(p => p.Id == updatedProduct.Id);

            if (existingProduct == null)
            {
                // If the product doesn't exist, throw an exception 
                throw new InvalidOperationException("Product not found.");
            }

            // Update only the properties you want to change, like StockQuantity, Name, etc.
            existingProduct.Name = updatedProduct.Name ?? existingProduct.Name; // Only update if not null
            existingProduct.StockQuantity = updatedProduct.StockQuantity;  // Update stock quantity
            existingProduct.CategoryId = updatedProduct.CategoryId; // Update category ID
            existingProduct.Price = updatedProduct.Price; // Update price
            existingProduct.ModifiedAt = updatedProduct.ModifiedAt; // Update price
            existingProduct.CreatedAt = updatedProduct.CreatedAt; // Update price
             
            if(updatedProduct.StockQuantity<=0)
            {
                existingProduct.IsActive = false;
            }
            else
            {
                existingProduct.IsActive = true;
            }

            // Mark the product as modified and save the changes
            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();

            // Return the updated product
            return existingProduct;
        }



        public async Task<PagedResultDto<Product>> GetUsersWithPaginationAsync(PaginationQueryDto paginationQuery)
        {
            // Extract data from the PaginationQueryDto
            string searchKeyword = paginationQuery.SearchKeyword;
            int pageNumber = paginationQuery.PageNumber;
            int pageSize = paginationQuery.PageSize;
            string sortBy = paginationQuery.SortBy;
            bool ascending = paginationQuery.Ascending;

            // Build the query based on search criteria and pagination settings
            IQueryable<Product> query = _context.Products.AsQueryable();

            // Search by keyword in any text field (e.g., Username, Email)
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                query = query.Where(u => u.Name.Contains(searchKeyword) || u.Description.Contains(searchKeyword));
            }

            // Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (ascending)
                {
                    query = query.OrderBy(u => EF.Property<object>(u, sortBy));
                }
                else
                {
                    query = query.OrderByDescending(u => EF.Property<object>(u, sortBy));
                }
            }

            // Pagination logic
            int totalRecords = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            int skip = (pageNumber - 1) * pageSize;

            List<Product> items = await query.Skip(skip).Take(pageSize).ToListAsync();

            return new PagedResultDto<Product>
            {
                Items = items,
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }

}
