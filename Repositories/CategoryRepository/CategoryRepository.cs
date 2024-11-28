using DarazApp.DbContext;
using DarazApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DarazApp.Repositories.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly UserDbContext _context;

        public CategoryRepository(UserDbContext context)
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



        public async Task<Category> AddCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> GetCategoryByNameAsync(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

    }

}
