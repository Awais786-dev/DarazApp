using DarazApp.DTOs;
using DarazApp.Models;

namespace DarazApp.Repositories.OrderRepository
{
    public interface IOrderRepository
    {
        Task<Order> AddAsync(Order order);  // Create order
        Task<Order> GetByIdAsync(int orderId);  // Get order by ID
        Task<List<Order>> GetByUserIdAsync(string userId);  // Get orders by UserId
        Task<Order> UpdateAsync(Order order);  // Update order

        Task<PagedResultDto<Order>> GetUsersWithPaginationAsync(PaginationQueryDto paginationQuery);

    }
}
