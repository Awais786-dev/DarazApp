using DarazApp.DTOs;

namespace DarazApp.DTOs
{
    public class OrderItemDto
    {
        public int OrderItemId { get; set; } // Unique identifier for the OrderItem
        public int ProductId { get; set; }
        public string ProductName { get; set; } // Name of the product for display
        public int Quantity { get; set; } // Quantity of the product in the order
        public decimal UnitPrice { get; set; } // Price of a single unit of the product
        public decimal TotalPrice { get; set; } // Calculated: UnitPrice * Quantity
    }
}


//public int ProductId { get; set; }
//public int Quantity { get; set; }
//public decimal UnitPrice { get; set; }