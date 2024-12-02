using DarazApp.DTOs;
using DarazApp.Models;
using DarazApp.Repositories.OrderRepository;
using DarazApp.Repositories.ProductRepository;

namespace DarazApp.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<Order> CreateOrderAsync(OrderDto orderDto)
        {
            // Validate input
            if (orderDto.NumOfItems <= 0)
            {
                throw new Exception("Number of items must be greater than 0.");
            }

            // Retrieve product and validate stock
            List<Product> products = await _productRepository.GetProductsByIdAsync(orderDto.ProductId);

            Product product = products.FirstOrDefault(p => p.Id == orderDto.ProductId); 

            if (product == null || product.StockQuantity < orderDto.NumOfItems || product.StockQuantity==0)
            {
                throw new Exception("Not enough stock for the product.");
            }

            // Decrease stock quantity
            product.StockQuantity -= orderDto.NumOfItems;
            await _productRepository.UpdateProductAsync(product);

            // Map DTO to Order
            Order order = new Order
            {
                ProductId = orderDto.ProductId,
                UserId = orderDto.UserId,
                NumOfItems = orderDto.NumOfItems,
                OrderStatus = "Pending", // Default status for new orders
                PaymentMethod = orderDto.PaymentMethod,
                TotalAmount = product.Price * orderDto.NumOfItems, // Calculate total amount based on NumOfItems and Product price
                OrderDate = DateTime.UtcNow,
                IsActive = true,
                ModifiedAt = DateTime.UtcNow,
                Address=orderDto.Address
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
            var orders = await _orderRepository.GetByUserIdAsync(userId);
            return orders;
        }


        public async Task<PagedResultDto<Order>> GetUsersWithPaginationAsync(PaginationQueryDto paginationQuery)
        {
            return await _orderRepository.GetUsersWithPaginationAsync(paginationQuery);
        }

    }

}
