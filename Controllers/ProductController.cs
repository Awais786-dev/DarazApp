using AutoMapper;
using DarazApp.DTOs;
using DarazApp.Models;
using DarazApp.Services.CategoryService;
using DarazApp.Services.ProductService;
using Microsoft.AspNetCore.Mvc;

namespace DarazApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;

      //  private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;


        public ProductsController(IMapper mapper, IProductService productService)  //ICategoryService categoryService,
        {
         //   _categoryService = categoryService;
            _mapper = mapper;
            _productService = productService;
        }

        // POST /api/products
        //[HttpPost]
        //public async Task<IActionResult> AddProduct([FromBody] Product product)
        //{
        //    if (product == null)
        //        return BadRequest("Invalid product data.");

        //    await _categoryService.AddProductAsync(product);
        //    return CreatedAtAction(nameof(GetProductsForCategory), new { id = product.CategoryId }, product);
        //}


        //// Ensure this method exists to match the CreatedAtAction reference
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetProductsForCategory(int id)
        //{
        //    var products = await _categoryService.GetProductsForCategoryAsync(id);
        //    if (products == null)
        //        return NotFound();

        //    return Ok(products);
        //}

        // POST /api/products
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto productDto)
        {

            if (productDto == null || string.IsNullOrEmpty(productDto.Name) || productDto.Price <= 0)
            {
                return BadRequest<string>("Invalid product data.");
            }

            // Optionally, you can validate whether a product with the same name already exists, if required
            // var existingProduct = await _categoryService.GetProductByNameAsync(productDto.Name);
            // if (existingProduct != null) return BadRequest("Product already exists.");

            // Map the ProductDto to Product model
            var product = _mapper.Map<Product>(productDto);

            // Add the product to the database using the service
            var addedProduct = await _productService.AddProductAsync(product);

            // Map the added product back to ProductDto for the response
            var addedProductDto = _mapper.Map<ProductDto>(addedProduct);

            // Return a success response with the created product DTO
            return Ok<ProductDto>(addedProductDto, "Product created successfully.");
        }

        // Ensure this method exists to match the CreatedAtAction reference
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductsForCategory(int id)
        {
            var products = await _productService.GetProductsForCategoryAsync(id);

            if (products == null || products.Count == 0)
                return NotFound("No products found for the specified category.");

            // Map the list of products to ProductDto
            var productDtos = _mapper.Map<List<ProductDto>>(products);

            // Return a success response with the list of products
            return Ok(productDtos, "Fetched products successfully.");
        }




    }

}
