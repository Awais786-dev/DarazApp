using System.ComponentModel.DataAnnotations;

namespace DarazApp.DTOs
{
    public class UserDto
    {
       // public string? Id { get; set; }

        [Required(ErrorMessage = "FullName is required")]
        [StringLength(100, ErrorMessage = "FullName cannot exceed 100 characters")]
        public string FullName { get; set; }


        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        public string? Password { get; set; }
        public string? Address { get; set; }
    }

}
