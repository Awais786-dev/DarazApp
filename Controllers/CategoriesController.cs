using AutoMapper;
using DarazApp.DTOs;
using DarazApp.Models;
using DarazApp.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;

namespace DarazApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;


        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        // GET /api/categories
        [HttpGet]
        public async Task<IActionResult> GetTopLevelCategories()
        {
            List<Category> categories = await _categoryService.GetTopLevelCategoriesAsync();
            List<CategoryDto> categoryDtos = _mapper.Map<List<CategoryDto>>(categories);

            return Ok<List<CategoryDto>>(categoryDtos, "Fetched top-level categories successfully.");

        }

        // GET /api/categories/{id}/subcategories
        [HttpGet("{id}/subcategories")]
        public async Task<IActionResult> GetSubcategories(int id)
        {
            List<Category> subcategories = await _categoryService.GetSubcategoriesAsync(id);

            // Map the list of Category entities to DTOs
            List<CategoryDto> subcategoryDtos = _mapper.Map<List<CategoryDto>>(subcategories);

            return Ok<List<CategoryDto>>(subcategoryDtos, "Fetched subcategories successfully.");
        }

        // GET /api/categories/{id}/products
        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetProductsForCategory(int id)
        {
            List<Product> products = await _categoryService.GetProductsForCategoryAsync(id);
           
            List<ProductDto> productDtos = _mapper.Map<List<ProductDto>>(products);

            return Ok<List<ProductDto>>(productDtos, "Fetched products successfully.");
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null || string.IsNullOrEmpty(categoryDto.Name))
            {
                return BadRequest<string>("Category name is required.");
            }

            // Check if category already exists
            Category existingCategory = await _categoryService.GetCategoryByNameAsync(categoryDto.Name);
            if (existingCategory != null)
            {
                return BadRequest<string>("Category already exists.");
            }

            // Map the DTO to the Category model
            Category category = _mapper.Map<Category>(categoryDto);

            // Add the new category to the database
            Category addedCategory = await _categoryService.AddCategoryAsync(category);

            // Map the added category to CategoryDto for the response
            CategoryDto addedCategoryDto = _mapper.Map<CategoryDto>(addedCategory);

            // Return a success response with the created category DTO
            return Ok<CategoryDto>(addedCategoryDto, "Category created successfully.");
        }
    }
}

