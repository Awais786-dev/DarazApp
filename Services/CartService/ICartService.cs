using DarazApp.DTOs;
using DarazApp.Models;

namespace DarazApp.Services.CartService
{
    public interface ICartService
    {
        Task<Cart> AddToCartAsync(CartInputDto cartDto);

        Task<List<Cart>> GetCartAsync(string userId);

        Task<Cart> UpdateCartAsync(CartInputDto cartDto);

        Task<Cart> RemoveCartItemAsync(int itemId);

        Task DeleteCartAsync(int cartId);

        Task<Cart> CheckoutCartAsync(CheckoutCartDto checkoutCartDto);
    }
}
