using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DarazApp.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }

        // Foreign key to Category (the final subcategory)
        public int CategoryId { get; set; }

        // Navigation property to Category
        public Category Category { get; set; }

        public int StockQuantity { get; set; }  // New field to track stock



    }
}
