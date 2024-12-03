using DarazApp.DTOs;
using DarazApp.Models;

namespace DarazApp.Repositories.OrderRepository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<List<Order>> GetByUserIdAsync(string userId);  // Get orders by UserId (not generic)
    
    }
}
