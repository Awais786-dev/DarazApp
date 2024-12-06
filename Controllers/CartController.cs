using AutoMapper;
using DarazApp.Controllers;
using DarazApp.DTOs;
using DarazApp.Helpers;
using DarazApp.Models;
using DarazApp.Services;
using DarazApp.Services.CartService;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CartController : BaseController
{
    private readonly ICartService _cartService;
    private readonly IMapper _mapper;

    public CartController(ICartService cartService, IMapper mapper)
    {
        _cartService = cartService;
        _mapper = mapper;
    }

    // POST: api/Cart
    [HttpPost]
    public async Task<IActionResult> AddToCart([FromBody] CartInputDto cartInputDto)
    {
        ActionResult validationResponse = ValidateModel(cartInputDto);
        if (validationResponse != null) return validationResponse;

        try
        {
            // Call the service to handle the business logic
            Cart cart = await _cartService.AddToCartAsync(cartInputDto);

            // Map the result to DTO for returning the response
            CartDto cartDto = _mapper.Map<CartDto>(cart);
            return Ok<CartDto>(cartDto, "Cart created ");

        }
        catch (Exception ex)
        {
            return BadRequest<string>("Error in Creating Cart", new List<string> { ex.Message });
        }
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCart(string userId)
    {
        try
        {
            List<Cart> cart = await _cartService.GetCartAsync(userId);

            List<CartDto> cartDto = _mapper.Map<List<CartDto>>(cart);

            // Return the mapped DTO in the response
            return Ok<List<CartDto>>(cartDto, "Cart updated successfully.");
        }
        catch (Exception ex)
        {
            return NotFound<string>("Cart Not Found");
        }
    }
    [HttpPut("UpdateCart")]
    public async Task<IActionResult> UpdateCart(CartInputDto cartInputDto)
    {
        try
        {
            var updatedCart = await _cartService.UpdateCartAsync(cartInputDto);

            // Use mapping to return a DTO response
            var cartDto = _mapper.Map<CartDto>(updatedCart);
            return Ok<CartDto>(cartDto, "Cart updated successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest<string>("Error in Creating Cart", new List<string> { ex.Message });
        }
    }

    [HttpDelete("Cart/{itemId}")]
    public async Task<IActionResult> RemoveCartItem(int itemId )
    {

        try
        {
            Cart updatedCart = await _cartService.RemoveCartItemAsync(itemId);

            // Return the updated cart using mapper
            CartDto cartDto = _mapper.Map<CartDto>(updatedCart);
            return Ok<CartDto>(cartDto, "Cart item removed successfully.");
        }
        catch(Exception ex)
        {
            return BadRequest<string>("Error in Removing Cart Items", new List<string> { ex.Message });

        }
    }

    [HttpDelete("Cart")]
    public async Task<IActionResult> DeleteCart(int cartId)
    {
        try
        {
            await _cartService.DeleteCartAsync(cartId);
            return Ok<string>(null, "Cart deleted successfully.");
        }
        catch(Exception ex)
        {
            return BadRequest<string>("Error in Removing Cart", new List<string> { ex.Message });
        }
    }

    [HttpPost("Checkout")]
    public async Task<IActionResult> CheckoutCartAsync([FromBody] CheckoutCartDto checkoutCartDto)
    {
        // Validate the input model
        var validationResult = ValidateModel(checkoutCartDto);
        if (validationResult != null)
        {
            return validationResult;
        }

        try
        {
            // Call the Checkout service
            var cart = await _cartService.CheckoutCartAsync(checkoutCartDto);
            CartDto cartDto = _mapper.Map<CartDto>(cart);

            // Return response
            return Ok<CartDto>(cartDto, "Cart has been successfully checked out and transferred to an order.");
        }
        catch (Exception ex)
        {
            return BadRequest<string>("Error in CheckOut Cart", new List<string> { ex.Message });
        }
    }




}
