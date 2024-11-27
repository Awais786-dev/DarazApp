using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DarazApp.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "FullName is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string FullName { get; set; }
        public string Address { get; set; }


    }
}
