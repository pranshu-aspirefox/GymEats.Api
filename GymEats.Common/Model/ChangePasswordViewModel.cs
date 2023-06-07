using GymEats.Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Common.Model
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password and Confirm Password doesn't match !")]
        public string ConfirmPassword { get; set; }
        [EmailAddress]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$", ErrorMessage = "Please enter valid email.")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }
}
