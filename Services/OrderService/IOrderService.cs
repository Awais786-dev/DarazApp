using DarazApp.DTOs;
using DarazApp.Models;

namespace DarazApp.Services.OrderService
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(OrderDto orderDto);

        Task<Order> GetOrderByIdAsync(int orderId);

        Task<List<Order>> GetOrdersByUserAsync(string userId);

        Task<PagedResultDto<Order>> GetUsersWithPaginationAsync(PaginationQueryDto paginationQuery);


    }
}
