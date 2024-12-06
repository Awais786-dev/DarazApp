using System.ComponentModel.DataAnnotations;

namespace DarazApp.DTOs
{
    public class AddToCartDto
    {
        [Required]
        public string UserId { get; set; } // The ID of the user adding the product to the cart.

        [Required]
        public int ProductId { get; set; } // The ID of the product being added.

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; } // The quantity of the product to add.
    }

}
