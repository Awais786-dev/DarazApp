using DarazApp.DTOs;
using DarazApp.Models;
using DarazApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DarazApp.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly IGenericRepository<Cart> _cartRepository;
        private readonly IGenericRepository<CartItem> _cartItemRepository;
        private readonly IGenericRepository<Product> _productRepository; 
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<OrderItem> _orderItemRepository;
        public CartService(IGenericRepository<Cart> cartRepository, IGenericRepository<CartItem> cartItemRepository, IGenericRepository<Product> productRepository, IGenericRepository<Order> orderRepository, IGenericRepository<OrderItem> orderItemRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }


        public async Task<Cart> AddToCartAsync(CartInputDto cartDto)
        {
            // Validate input
            if (cartDto == null || cartDto.Items == null || cartDto.Items.Count == 0)
            {
                throw new ArgumentException("Cart input is invalid.");
            }

            string userId = cartDto.UserId;

            var cart = (await _cartRepository.FindWithIncludesAsync(
                   c => c.UserId == userId,
                   query => query.Include(c => c.CartItems).ThenInclude(ci => ci.Product)))
                    .FirstOrDefault();

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    CartItems = new List<CartItem>()
                };

                cart = await _cartRepository.AddAsync(cart);
            }

            // Process each item in the DTO
            foreach (var itemDto in cartDto.Items)
            {

                // Validate the quantity
                if (itemDto.Quantity <= 0 || itemDto.Quantity > 3)
                {
                    throw new Exception($"Quantity for product {itemDto.ProductId} must be between 1 and 3.");
                }

                // Validate the product
                var product = await _productRepository.GetByIdAsync(itemDto.ProductId);


                if (product == null)
                {
                    throw new Exception($"Product with ID {itemDto.ProductId} does not exist.");
                }

                // Check if the product is already in the cart
                var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == itemDto.ProductId);

                if (existingCartItem != null)
                {
                    // Update the quantity and price
                    existingCartItem.Quantity += itemDto.Quantity;
                    existingCartItem.Price = product.Price * existingCartItem.Quantity;
                    await _cartItemRepository.UpdateAsync(existingCartItem);
                }
                else
                {
                    // Add a new item to the cart
                    var newCartItem = new CartItem
                    {
                        CartId = cart.Id,
                        ProductId = itemDto.ProductId,
                        Quantity = itemDto.Quantity,
                        Price = product.Price * itemDto.Quantity,
                        ProductName = product.Name,
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow
                    };

                    await _cartItemRepository.AddAsync(newCartItem);
                }
            }

            // Update the cart's ModifiedAt timestamp
            cart.ModifiedAt = DateTime.UtcNow;
            await _cartRepository.UpdateAsync(cart);

            return cart;
        }

        public async Task<List<Cart>> GetCartAsync(string userId)
        {
            // Validate input
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty.");

            // Retrieve the user's cart along with cart items and their associated product names
            List<Cart> cart = await _cartRepository.FindWithIncludesAsync(
                c => c.UserId == userId,
                query => query.Include(c => c.CartItems)
                              .ThenInclude(ci => ci.Product)
            );

            if (cart == null || !cart.Any())
                throw new Exception("Cart not found for the specified user.");

            return cart;
        }

        public async Task<Cart> UpdateCartAsync(CartInputDto cartDto)
        {
            // Validate input
            if (cartDto == null || cartDto.Items == null || cartDto.Items.Count == 0)
                throw new ArgumentException("Cart input is invalid.");

            string userId = cartDto.UserId;

            // Retrieve the existing cart for the user
            var cart = (await _cartRepository.FindWithIncludesAsync(
                c => c.UserId == userId,
                query => query.Include(c => c.CartItems)))
                .FirstOrDefault();

            if (cart == null || !cart.CartItems.Any())
                throw new Exception("Cart not found or empty for the specified user.");

            foreach (var itemDto in cartDto.Items)
            {
                // Validate the quantity
                if (itemDto.Quantity <= 0 || itemDto.Quantity > 3)
                {
                    throw new Exception($"Quantity for product {itemDto.ProductId} must be between 1 and 3.");
                }

                // Validate the product
                var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                if (product == null)
                    throw new Exception($"Product with ID {itemDto.ProductId} does not exist.");

                // Check if the product is in stock
                if (product.StockQuantity < itemDto.Quantity)
                    throw new Exception($"Product {product.Name} is out of stock or does not have sufficient quantity.");

                // Find the item in the cart
                var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == itemDto.ProductId);

                if (existingCartItem != null)
                {
                    // Update the quantity and price based on the latest product price
                    existingCartItem.Quantity = itemDto.Quantity;
                    existingCartItem.Price = product.Price * itemDto.Quantity;
                    existingCartItem.ModifiedAt = DateTime.UtcNow;

                    await _cartItemRepository.UpdateAsync(existingCartItem);
                }
                else
                {
                    // If the product does not exist in the cart, add it
                    var newCartItem = new CartItem
                    {
                        CartId = cart.Id,
                        ProductId = itemDto.ProductId,
                        ProductName = product.Name,
                        Quantity = itemDto.Quantity,
                        Price = product.Price * itemDto.Quantity,
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow
                    };

                    await _cartItemRepository.AddAsync(newCartItem);
                }
            }

            // Update cart modified timestamp
            cart.ModifiedAt = DateTime.UtcNow;
            await _cartRepository.UpdateAsync(cart);

            return cart;
        }


        public async Task<string> DeleteCartItemAsync(int cartId, int cartItemId)
        {
            // Retrieve the cart item with the specified cartItemId and cartId for the user
            var cartItem = await _cartItemRepository.FindWithIncludesAsync(
                ci => ci.Id == cartItemId && ci.CartId == cartId,
                query => query.Include(ci => ci.Cart)

            );

            var firstCartItem = cartItem.FirstOrDefault();


            if (firstCartItem == null)
            {
                throw new Exception("CartItem not found or you don't have permission to delete it.");
            }

            // Remove the cart item
            await _cartItemRepository.DeleteAsync(firstCartItem.Id);

            // Optionally, check if the cart is empty after deletion
            var cart = await _cartRepository.GetByIdAsync(cartId);
            if (cart.CartItems.Count == 0)
            {
                await _cartRepository.DeleteAsync(cart.Id);
            }

            return "Cart item deleted successfully.";
        }

        public async Task<Cart> RemoveCartItemAsync(int itemId)
        {
            // Retrieve the CartItem by ID
            var cartItem = await _cartItemRepository.GetByIdAsync(itemId);
            if (cartItem == null)
            {
                throw new Exception($"Cart item with ID {itemId} does not exist.");
            }



            // Retrieve the updated Cart
            var cart = await _cartRepository.GetByIdWithIncludesAsync(
                cartItem.CartId,
                query => query.Include(c => c.CartItems).ThenInclude(ci => ci.Product)
            );

            if (cart == null || !cart.CartItems.Any())
            {
                throw new Exception("Cart is empty or does not exist.");
            }

            // Delete the CartItem
            await _cartItemRepository.DeleteAsync(itemId);


            return cart;
        }

        public async Task DeleteCartAsync(int cartId)
        {
            // Retrieve the Cart with includes
            var cart = await _cartRepository.GetByIdWithIncludesAsync(
                cartId,
                query => query.Include(c => c.CartItems)
            );

            if (cart == null)
            {
                throw new Exception($"Cart with ID {cartId} does not exist.");
            }

            // Create a list of CartItem IDs to delete
            var cartItemIds = cart.CartItems.Select(ci => ci.Id).ToList();

            // Delete all CartItems associated with the cart
            foreach (var itemId in cartItemIds)
            {
                await _cartItemRepository.DeleteAsync(itemId);
            }


            // Delete the Cart
            await _cartRepository.DeleteAsync(cartId);
        }


        public async Task<Cart> CheckoutCartAsync(CheckoutCartDto checkoutCartDto)
        {
            // Retrieve the user's cart with cart items
            var cart = (await _cartRepository.FindWithIncludesAsync(
                c => c.UserId == checkoutCartDto.UserId, // Predicate to filter by UserId
                query => query.Include(c => c.CartItems).ThenInclude(ci => ci.Product) // Include CartItems and related Product
            )).FirstOrDefault();

            if (cart == null || cart.CartItems.Count == 0)
            {
                throw new Exception("Cart is empty or does not exist.");
            }

            // Initialize total amount for the order
            decimal totalAmount = 0;

            // Create Order with address details
            Order order = new Order
            {
                UserId = checkoutCartDto.UserId,
                OrderStatus = "Pending",
                PaymentMethod = "COD", // Placeholder until payment is done
                TotalAmount = totalAmount,
                Province = checkoutCartDto.Province,
                PhoneNumber = checkoutCartDto.PhoneNumber,
                City = checkoutCartDto.City,
                StreetDetails = checkoutCartDto.StreetDetails,
                Address = checkoutCartDto.Address,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                IsActive = true
            };

            order = await _orderRepository.AddAsync(order);

            // Transfer CartItems to OrderItems and update stock
            foreach (var cartItem in cart.CartItems)
            {
                // Check if the product has sufficient stock
                if (cartItem.Quantity > cartItem.Product.StockQuantity)
                {
                    throw new Exception($"Product {cartItem.Product.Name} does not have enough stock.");
                }

                // Deduct stock
                cartItem.Product.StockQuantity -= cartItem.Quantity;
                await _productRepository.UpdateAsync(cartItem.Product);

                // Add OrderItem
                OrderItem orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price, // This will be the price at the time of checkout
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow
                };

                await _orderItemRepository.AddAsync(orderItem);

                // Accumulate total order amount
                totalAmount += cartItem.Price;
            }

            // Update order total amount
            order.TotalAmount = totalAmount;
            await _orderRepository.UpdateAsync(order);

            // Create a list of CartItem IDs to delete
            var cartItemIds = cart.CartItems.Select(ci => ci.Id).ToList();

            // Delete all CartItems associated with the cart
            foreach (var itemId in cartItemIds)
            {
                await _cartItemRepository.DeleteAsync(itemId);
            }

            // Optionally, delete the cart itself (if the cart is no longer needed)
            await _cartRepository.DeleteAsync(cart.Id);

            return cart;  // Optionally return the cart or order details
        }






    }
}
