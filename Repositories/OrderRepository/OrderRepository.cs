using DarazApp.DbContext;
using DarazApp.DTOs;
using DarazApp.Models;
using DarazApp.Repositories.CategoryRepository;
using Microsoft.EntityFrameworkCore;

namespace DarazApp.Repositories.OrderRepository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DbContext.DbContext _context;

        public OrderRepository(DbContext.DbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<List<Order>> GetByUserIdAsync(string userId)
        {
            // Retrieve all orders for a specific user
            return await _context.Orders
                .Where(o => o.UserId == userId)  // Filter by UserId
                .Include(o => o.Product)  // Include related product details
                .ToListAsync();  // Return list of orders
        }

    }

}
