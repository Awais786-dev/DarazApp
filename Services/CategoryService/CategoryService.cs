using DarazApp.DTOs;
using DarazApp.Models;
using DarazApp.Repositories.CategoryRepository;
using DarazApp.Repositories.ProductRepository;

namespace DarazApp.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public async Task<List<Category>> GetTopLevelCategoriesAsync()
        {
            return await _categoryRepository.GetTopLevelCategoriesAsync();
        }

        public async Task<List<Category>> GetSubcategoriesAsync(int categoryId)
        {
            List<Category> category= await _categoryRepository.GetSubcategoriesAsync(categoryId);
            return category;

        }

        //here i have to use service of product not repos of product.
        public async Task<List<Product>> GetProductsForCategoryAsync(int categoryId)
        {
            return await _productRepository.GetProductsByCategoryIdAsync(categoryId); // need to change
        }



        public async Task<Category> AddCategoryAsync(Category category)
        {
           await _categoryRepository.AddCategoryAsync(category);
            return category;
        }

        public async Task<Category> GetCategoryByNameAsync(string name)
        {
            return await _categoryRepository.GetCategoryByNameAsync(name);
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetCategoryByIdAsync(id);
        }


        public async Task<PagedResultDto<Category>> GetUsersWithPaginationAsync(PaginationQueryDto paginationQuery)
        {
            return await _categoryRepository.GetUsersWithPaginationAsync(paginationQuery);

        }







    }

}
