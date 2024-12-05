using DarazApp.DTOs;
using DarazApp.Models;
using DarazApp.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DarazApp.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<OrderItem> _orderItemRepository;


        public OrderService(IGenericRepository<Order> orderRepository, IGenericRepository<Product> productRepository, IGenericRepository<OrderItem> orderItemRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _orderItemRepository = orderItemRepository;
        }

        public async Task<Order> CreateOrderAsync(OrderInputDto orderDto)
        {
            decimal totalAmount = 0;

            // Validate input and retrieve products
            foreach (var item in orderDto.Items)
            {
                Product product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null || product.StockQuantity < item.NumOfItems)
                {
                    throw new Exception($"Product {item.ProductId} has insufficient stock.");
                }

                // Reduce stock
                product.StockQuantity -= item.NumOfItems;
                await _productRepository.UpdateAsync(product);

                // Accumulate the total amount
                totalAmount += product.Price * item.NumOfItems;
            }

            // Create Order
            Order order = new()
            {
                UserId = orderDto.UserId,
                OrderStatus = "Pending",
                PaymentMethod = orderDto.PaymentMethod,
                TotalAmount = totalAmount, // Set total amount calculated in the loop

                // Address Details
                Province = orderDto.Province,
                PhoneNumber = orderDto.PhoneNumber,
                City = orderDto.City,
                StreetDetails = orderDto.StreetDetails,
                Address = orderDto.Address,

                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                IsActive = true,
            };

            Order createdOrder = await _orderRepository.AddAsync(order);

            // Create Order Items
            foreach (var item in orderDto.Items)
            {
                OrderItem orderItem = new()
                {
                    OrderId = createdOrder.Id,
                    ProductId = item.ProductId,
                    Quantity = item.NumOfItems,
                    Price = (await _productRepository.GetByIdAsync(item.ProductId)).Price * item.NumOfItems,
                };

                await _orderItemRepository.AddAsync(orderItem);
            }

            return createdOrder;
        }


        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdWithIncludesAsync(
                orderId,
                query => query
                    .Include(o => o.OrderItems)              // Include OrderItems
                    .ThenInclude(oi => oi.Product)           // Include Product for each OrderItem
            );

            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            return order;
        }


        public async Task<List<Order>> GetOrdersByUserAsync(string userId)
        {
            Expression<Func<Order, bool>> predicate = (o => o.UserId == userId);

            List<Order> orders = await _orderRepository.FindWithIncludesAsync(
               predicate, // Filter by userId
                query => query
                    .Include(o => o.OrderItems)              // Include OrderItems
                    .ThenInclude(oi => oi.Product)           // Include Product for each OrderItem
            );

            return orders;
        }


        public async Task<PagedResultDto<Order>> GetOrdersWithPaginationAsync(PaginationQueryDto paginationQuery)
        {
            // return await _orderRepository.GetWithPaginationAsync(paginationQuery);
            return await _orderRepository.GetWithPaginationAsync(
           paginationQuery,
           includeChain: query =>
               query.Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
       );
        }

    }

}
