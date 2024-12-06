using DarazApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DarazApp.DbContext
{

    public class DbContext : IdentityDbContext<User>
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Order> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }


    }
}
