using System.ComponentModel.DataAnnotations;

namespace Dialogistic.Models
{
    public class CreateUserModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}