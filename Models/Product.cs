using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DarazApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }

        // Foreign key to Category (the final subcategory)
        public int CategoryId { get; set; }

        // Navigation property to Category
        public Category Category { get; set; }

        public int StockQuantity { get; set; }  // New field to track stock

        // New columns
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Default to current UTC time
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow; // Default to current UTC time
        public bool IsActive { get; set; } = true; // Default to true


    }
}
