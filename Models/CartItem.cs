using System.ComponentModel.DataAnnotations;

namespace DarazApp.Models
{
    public class CartItem :BaseEntity
    {
        [Required]
        public int CartId { get; set; } // Foreign Key (Cart table)

        [Required]
        public int ProductId { get; set; } // Foreign Key (Product table)

        [Required]
        [MaxLength(255)]
        public string ProductName { get; set; } // Store the product name at the time of addition

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; } // Price of the product at the time of addition to the cart

        // Navigation property to Cart
        public Cart Cart { get; set; }

        // Navigation property to Product
        public Product Product { get; set; }
    }

}
