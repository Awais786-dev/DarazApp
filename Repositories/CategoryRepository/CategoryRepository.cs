using DarazApp.DbContext;
using DarazApp.DTOs;
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

        public async Task<PagedResultDto<Category>> GetUsersWithPaginationAsync(PaginationQueryDto paginationQuery)
        {
            // Extract data from the PaginationQueryDto
            string searchKeyword = paginationQuery.SearchKeyword;
            int pageNumber = paginationQuery.PageNumber;
            int pageSize = paginationQuery.PageSize;
            string sortBy = paginationQuery.SortBy;
            bool ascending = paginationQuery.Ascending;

            // Build the query based on search criteria and pagination settings
            IQueryable<Category> query = _context.Categories.AsQueryable();

            // Search by keyword in any text field (e.g., Username, Email)
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                query = query.Where(u => u.Name.Contains(searchKeyword));
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

            List<Category> items = await query.Skip(skip).Take(pageSize).ToListAsync();

            return new PagedResultDto<Category>
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
