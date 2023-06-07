using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Common.Model
{
    public class LoginViewModel
    {
        [EmailAddress]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$", ErrorMessage = "Please enter valid email.")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

    }
}
