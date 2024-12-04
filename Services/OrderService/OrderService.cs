using DarazApp.DTOs;
using DarazApp.Models;
using DarazApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DarazApp.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<Product> _productRepository;

        public OrderService(IGenericRepository<Order> orderRepository, IGenericRepository<Product> productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<Order> CreateOrderAsync(OrderInputDto orderDto)
        {
            // Validate input
            if (orderDto.NumOfItems <= 0)
            {
                throw new Exception("Number of items must be greater than 0.");
            }

            // Retrieve product and validate stock
            Product product = await _productRepository.GetByIdAsync(orderDto.ProductId);

            if (product == null)
            {
                throw new Exception("Product Id does not exist.");
            }
            if (product.StockQuantity < orderDto.NumOfItems || product.StockQuantity == 0)
            {
                throw new Exception("Not enough stock for the product.");

            }

            // Decrease stock quantity
            product.StockQuantity -= orderDto.NumOfItems;
            await _productRepository.UpdateAsync(product);

            // Map DTO to Order
            Order order = new Order
            {
                ProductId = orderDto.ProductId,
                UserId = orderDto.UserId,
                NumOfItems = orderDto.NumOfItems,
                OrderStatus = "Pending", // Default status for new orders
                PaymentMethod = orderDto.PaymentMethod,
                TotalAmount = product.Price * orderDto.NumOfItems, // Calculate total amount based on NumOfItems and Product price
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                ModifiedAt = DateTime.UtcNow,
                Address = orderDto.Address
            };

            // Save order
            Order createdOrder = await _orderRepository.AddAsync(order);

            return createdOrder;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            return order;
        }


        public async Task<List<Order>> GetOrdersByUserAsync(string userId)
        {
            var orders = await _orderRepository.FindByConditionAsync<Order>(o => o.UserId == userId).ToListAsync();

            return orders;
        }


        public async Task<PagedResultDto<Order>> GetOrdersWithPaginationAsync(PaginationQueryDto paginationQuery)
        {
            return await _orderRepository.GetWithPaginationAsync(paginationQuery);
        }

    }

}
