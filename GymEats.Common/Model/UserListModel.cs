using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Common.Model
{
    public class UserListModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; }
        public string SuggesticId { get; set; }
        public string MealPlanEndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
