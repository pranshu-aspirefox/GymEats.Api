using System.ComponentModel.DataAnnotations;

namespace GymEats.Common.Model
{
    public class RequestPasswordViewModel
    {
        [EmailAddress]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$", ErrorMessage = "Please enter valid email.")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }
}
