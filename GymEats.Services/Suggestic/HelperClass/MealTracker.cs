using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Suggestic.HelperClass
{
    public class MealTracker
    {
        public string mealId { get; set; }
        public Meal meal { get; set; }
        public string value { get; set; }
        public string date { get; set; }
    }

    public class MealTrackerData
    {
        public List<MealTracker> mealTracker { get; set; }
    }
}
