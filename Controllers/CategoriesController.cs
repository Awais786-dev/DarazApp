using AutoMapper;
using DarazApp.DTOs;
using DarazApp.Helpers;
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
            try
            {
                List<Category> categories = await _categoryService.GetTopLevelCategoriesAsync();
                List<CategoryDto> categoryDtos = _mapper.Map<List<CategoryDto>>(categories);

                return Ok<List<CategoryDto>>(categoryDtos, CategoryResponseMessages.TopLevelCategoriesFetchedSuccess);  
            }
            catch (Exception ex)
            {
                return BadRequest<string>($"{CategoryResponseMessages.ErrorOccurred} {ex.Message}");  
            }
        }

        // GET /api/categories/{id}/subcategories
        [HttpGet("{id}/subcategories")]
        public async Task<IActionResult> GetSubcategories(int id)
        {
            try
            {
                List<Category> subcategories = await _categoryService.GetSubcategoriesAsync(id);

                // Map the list of Category entities to DTOs
                List<CategoryDto> subcategoryDtos = _mapper.Map<List<CategoryDto>>(subcategories);

                return Ok<List<CategoryDto>>(subcategoryDtos, CategoryResponseMessages.SubcategoriesFetchedSuccess); 
            }
            catch (Exception ex)
            {
                return BadRequest<string>($"{CategoryResponseMessages.ErrorOccurred} {ex.Message}"); 
            }
        }

        // GET /api/categories/{id}/products
        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetProductsForCategory(int id)
        {
            try
            {
                List<Product> products = await _categoryService.GetProductsForCategoryAsync(id);

                List<ProductDto> productDtos = _mapper.Map<List<ProductDto>>(products);

                return Ok<List<ProductDto>>(productDtos, CategoryResponseMessages.ProductsFetchedSuccess); 
            }
            catch (Exception ex)
            {
                return BadRequest<string>($"{CategoryResponseMessages.ErrorOccurred} {ex.Message}"); 
            }
        }

        // POST /api/categories
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto categoryDto)
        {
            try
            {
                if (categoryDto == null || string.IsNullOrEmpty(categoryDto.Name))
                {
                    return BadRequest<string>(CategoryResponseMessages.CategoryNameRequired);  
                }

                // Check if category already exists
                Category existingCategory = await _categoryService.GetCategoryByNameAsync(categoryDto.Name);
                if (existingCategory != null)
                {
                    return BadRequest<string>(CategoryResponseMessages.CategoryAlreadyExists);  
                }

                // Map the DTO to the Category model
                Category category = _mapper.Map<Category>(categoryDto);

                // Add the new category to the database
                Category addedCategory = await _categoryService.AddCategoryAsync(category);

                // Map the added category to CategoryDto for the response
                CategoryDto addedCategoryDto = _mapper.Map<CategoryDto>(addedCategory);

                // Return a success response with the created category DTO
                return Ok<CategoryDto>(addedCategoryDto, CategoryResponseMessages.CategoryCreatedSuccess); 
            }
            catch (Exception ex)
            {
                return BadRequest<string>($"{CategoryResponseMessages.ErrorOccurred} {ex.Message}");  
            }
        }

        [HttpGet("GetOnPagination")]
        public async Task<ActionResult> GetCategories([FromQuery] PaginationQueryDto paginationQuery)
        {
            try
            {
                PagedResultDto<Category> pagedResult = await _categoryService.GetCategoryWithPaginationAsync(paginationQuery);

                if (pagedResult == null || !pagedResult.Items.Any())
                {
                    return NotFound<CategoryDto>(CategoryResponseMessages.CategoryNotExists);  // Use constant for "No products found" message
                }

                // Map the paged result items to ProductDto
                List<CategoryDto> categoryDtos = _mapper.Map<List<CategoryDto>>(pagedResult.Items);

                // Return the paginated result with the pagination metadata
                PagedResultDto<CategoryDto> response = new PagedResultDto<CategoryDto>
                {
                    Items = categoryDtos,
                    TotalRecords = pagedResult.TotalRecords,
                    TotalPages = pagedResult.TotalPages,
                    PageNumber = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize
                };

                return Ok(response, OrderResponseMessages.OrdersRetrievedSuccess);
            }
            catch (Exception ex)
            {
                return BadRequest<CategoryDto>(CategoryResponseMessages.ErrorOccurred, new List<string> { ex.Message });
            }
        }
    }
}
