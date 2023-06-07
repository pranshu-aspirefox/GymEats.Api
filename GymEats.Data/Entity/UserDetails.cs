using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Data.Entity
{
    public class UserDetails
    {
        [Key]
        public int Id { get; set; }
        public int Age { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string Gender { get; set; }
        public double? TargetWeight { get; set; }
        public int? Calories { get; set; }
        public int? PlanAmount { get; set; }
        public int? WaterIntake { get; set; }
        public decimal? UserBMI { get; set; }
        public string? DailyActivityLevel { get; set; }
        public string? MealPlanSubscriptionId { get; set; }
        

        public Guid? DietId { get; set; }
        [ForeignKey("DietId")]
        public virtual Diet? DietDetails { get; set; }
        public Guid? SurveyId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Boolean IsVerified { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean IsDeleted { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
