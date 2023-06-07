using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Common.Model
{
    public class UserViewModel
    {
        public DateTime CreatedOn { get; set; }
        public string? UniqueToken { get; set; }
        public Guid? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExirationTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? IsActive { get; set; }
        public bool IsBan { get; set; }
        public string? EmailConfirmatiomToken { get; set; }
    }
}
