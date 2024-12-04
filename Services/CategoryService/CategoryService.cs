using DarazApp.DTOs;
using DarazApp.Models;
using DarazApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DarazApp.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IGenericRepository<Product> _productRepository;

        public CategoryService(IGenericRepository<Category> categoryRepository, IGenericRepository<Product> productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public async Task<List<Category>> GetTopLevelCategoriesAsync()
        {
           // return await _categoryRepository.GetTopLevelCategoriesAsync();
            return await _categoryRepository.FindByConditionAsync<Category>(c => c.ParentCategoryId == null).ToListAsync();

        }

        public async Task<List<Category>> GetSubcategoriesAsync(int categoryId)
        {
            //List<Category> category= await _categoryRepository.GetSubcategoriesAsync(categoryId);
            //return category;
            return await _categoryRepository.FindByConditionAsync<Category>(c => c.ParentCategoryId == categoryId).ToListAsync();


        }

        //here i have to use service of product not repos of product.
        public async Task<List<Product>> GetProductsForCategoryAsync(int categoryId)
        {
            return await _categoryRepository.FindByConditionAsync<Product>(p => p.CategoryId == categoryId).ToListAsync();

        }



        public async Task<Category> AddCategoryAsync(Category category)
        {
            // Check if the ParentCategoryId exists (if provided)
            if (category.ParentCategoryId.HasValue) // Assuming ParentCategoryId is nullable
            {
                var parentCategoryExists = await _categoryRepository
                    .FindByConditionAsync<Category>(c => c.Id == category.ParentCategoryId.Value)
                    .AnyAsync();

                if (!parentCategoryExists)
                {
                    throw new InvalidOperationException($"Parent category with ID {category.ParentCategoryId} does not exist.");
                }
            }

            await _categoryRepository.AddAsync(category);
            return category;
        }

        public async Task<Category> GetCategoryByNameAsync(string name)
        {
           // return await _categoryRepository.GetCategoryByNameAsync(name);
            return await _categoryRepository.FindByConditionAsync<Category>(c => c.Name == name).FirstOrDefaultAsync();

        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }


        public async Task<PagedResultDto<Category>> GetCategoryWithPaginationAsync(PaginationQueryDto paginationQuery)
        {
            return await _categoryRepository.GetWithPaginationAsync(paginationQuery);

        }







    }

}
