using AutoMapper;
using DarazApp.DTOs;
using DarazApp.Helpers;
using DarazApp.Models;
using DarazApp.Services.CategoryService;
using DarazApp.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DarazApp.Controllers
{
    [Route("api/[controller]")]
  //  [ApiController]
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto productDto)
        {
            try
            {
                // Validate input data
                if (productDto == null || string.IsNullOrEmpty(productDto.Name) || productDto.Price <= 0 || productDto.StockQuantity <= 0)
                {
                    return BadRequest<string>(ProductResponseMessages.ErrorOccurred);  // Use constant for error message
                }

                // Map ProductDto to Product model
                Product product = _mapper.Map<Product>(productDto);

                // Add the product via the service
                Product addedProduct = await _productService.AddProductAsync(product);

                ProductDto addedProductDto = _mapper.Map<ProductDto>(addedProduct);

                // Return success response
                return Ok(addedProductDto, ProductResponseMessages.ProductCreatedSuccess);  // Use constant for success message
            }
            catch (Exception ex)
            {
                return BadRequest<string>(ProductResponseMessages.ErrorOccurred, new List<string> { ex.Message });  // Use constant for error message
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductsForCategory(int id)
        {
            try
            {
                // Fetch products for the category
                Product product = await _productService.GetProductsById(id);

                // Check if products were found
                if (product == null)
                {
                    return NotFound<string>(ProductResponseMessages.NoProductsFound);  // Use constant for "No products found" message
                }

                // Map the list of products to ProductDto
                ProductDto productDto = _mapper.Map<ProductDto>(product);

                // Return success response with the product list
                return Ok(productDto, ProductResponseMessages.ProductsRetrievedSuccess);  // Use constant for success message
            }
            catch (Exception ex)
            {

                return BadRequest<string>(ProductResponseMessages.ErrorOccurred, new List<string> { ex.Message });  // Use constant for error message
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchProductsByName(string productName)
        {
            try
            {
                List<Product> products = await _productService.SearchProductsByNameAsync(productName);
                if (products == null || !products.Any())
                {
                    return NotFound<string>(ProductResponseMessages.ProductNotFound);  // Use constant for "Product not found" message
                }

                List<ProductDto> productDtos = _mapper.Map<List<ProductDto>>(products);

                return Ok<List<ProductDto>>(productDtos, ProductResponseMessages.ProductsRetrievedSuccess);  // Use constant for success message
            }
            catch (Exception ex)
            {
                return BadRequest<string>(ProductResponseMessages.ErrorOccurred, new List<string> { ex.Message });  // Use constant for error message
            }
        }

        [HttpGet("GetOnPagination")]
        public async Task<ActionResult> GetProducts([FromQuery] PaginationQueryDto paginationQuery)
        {
            try
            {
                PagedResultDto<Product> pagedResult = await _productService.GetProductsWithPaginationAsync(paginationQuery);

                if (pagedResult == null || !pagedResult.Items.Any())
                {
                    return NotFound<ProductDto>(ProductResponseMessages.NoProductsFound);  // Use constant for "No products found" message
                }

                // Map the paged result items to ProductDto
                List<ProductDto> productDtos = _mapper.Map<List<ProductDto>>(pagedResult.Items);

                // Return the paginated result with the pagination metadata
                PagedResultDto<ProductDto> response = new PagedResultDto<ProductDto>
                {
                    Items = productDtos,
                    TotalRecords = pagedResult.TotalRecords,
                    TotalPages = pagedResult.TotalPages,
                    PageNumber = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize
                };

                return Ok(response, ProductResponseMessages.ProductsRetrievedSuccess);  // Use constant for success message
            }
            catch (Exception ex)
            {
                return BadRequest<ProductDto>(ProductResponseMessages.ErrorOccurred, new List<string> { ex.Message });
            }
        }
    }
}
