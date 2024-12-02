using DarazApp.DbContext;
using DarazApp.DTOs;
using DarazApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DarazApp.Repositories.OrderRepository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly UserDbContext _context;

        public OrderRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<Order> AddAsync(Order order)
        {

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();  // Save changes to the database
            return order;  // Return the created order
        }

        // Get order by ID
        public async Task<Order> GetByIdAsync(int orderId)
        {
            
            return await _context.Orders
                .Include(o => o.Product)  // Include the product related to the order
                .FirstOrDefaultAsync(o => o.OrderId == orderId);  // Find by OrderId
        }

        public async Task<List<Order>> GetByUserIdAsync(string userId)
        {
            // Retrieve all orders for a specific user
            return await _context.Orders
                .Where(o => o.UserId == userId)  // Filter by UserId
                .Include(o => o.Product)  // Include related product details
                .ToListAsync();  // Return list of orders
        }

     
        public async Task<Order> UpdateAsync(Order order)
        {
            // Update the order in the database
            _context.Orders.Update(order);
            await _context.SaveChangesAsync(); 
            return order;  
        }

        public async Task<PagedResultDto<Order>> GetUsersWithPaginationAsync(PaginationQueryDto paginationQuery)
        {
            // Extract data from the PaginationQueryDto
            string searchKeyword = paginationQuery.SearchKeyword;
            int pageNumber = paginationQuery.PageNumber;
            int pageSize = paginationQuery.PageSize;
            string sortBy = paginationQuery.SortBy;
            bool ascending = paginationQuery.Ascending;

            // Build the query based on search criteria and pagination settings
            IQueryable<Order> query = _context.Orders.AsQueryable();

            // Search by keyword in any text field (e.g., Username, Email)
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                query = query.Where(u => u.OrderStatus.Contains(searchKeyword));
            }

            // Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (ascending)
                {
                    query = query.OrderBy(u => EF.Property<object>(u, sortBy));
                }
                else
                {
                    query = query.OrderByDescending(u => EF.Property<object>(u, sortBy));
                }
            }

            // Pagination logic
            int totalRecords = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            int skip = (pageNumber - 1) * pageSize;

            List<Order> items = await query.Skip(skip).Take(pageSize).ToListAsync();

            return new PagedResultDto<Order>
            {
                Items = items,
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }


    }

}
