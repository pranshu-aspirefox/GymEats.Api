using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Common.Model
{
    public class MealmeItemRequest
    {
        public string MealName { get; set; }
        public double Calorie { get; set; }
        public double Protein { get; set; }
        public double Fat { get; set; }
        public double Carbs { get; set; }
        public int Day { get; set; }
        public string MealType { get; set; }
    }
}
