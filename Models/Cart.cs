using System.ComponentModel.DataAnnotations;

namespace DarazApp.Models
{
    public class Cart : BaseEntity
    {
        [Required]
        public string UserId { get; set; } // Foreign Key (User table)

        // Navigation property to the User
        public User User { get; set; }

        // Navigation property to CartItems
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
