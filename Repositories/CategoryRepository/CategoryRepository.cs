using DarazApp.DbContext;
using DarazApp.DTOs;
using DarazApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DarazApp.Repositories.CategoryRepository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly DbContext.DbContext _context;

        public CategoryRepository(DbContext.DbContext context) :base(context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetTopLevelCategoriesAsync()
        {
            return await _context.Categories
                                 .Where(c => c.ParentCategoryId == null)
                                 .ToListAsync();
        }

        public async Task<List<Category>> GetSubcategoriesAsync(int categoryId)
        {
            return await _context.Categories
                                 .Where(c => c.ParentCategoryId == categoryId)
                                 .ToListAsync();
        }


        public async Task<Category> GetCategoryByNameAsync(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
        }


    }

}
