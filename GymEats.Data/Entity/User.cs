using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymEats.Data.Entity
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? UniqueToken { get; set; }
        public Guid? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExirationTime { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? SuggesticId { get; set; }
        public string? MealPlanEndDate { get; set; }

        public int? UserDetailsId { get; set; }
        [ForeignKey("UserDetailsId")]
        public UserDetails? UserDetails { get; set; }
        public int? UserAddressId { get; set; }
        [ForeignKey("UserAddressId")]
        public UserAddress? UserAddress { get; set;}
    }
}