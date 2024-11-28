using System.ComponentModel.DataAnnotations;

namespace DarazApp.DTOs
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
