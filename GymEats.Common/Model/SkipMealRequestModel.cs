using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Common.Model
{
    public class SkipMealRequestModel
    {
        public List<string> MealId { get; set; }
        public List<string> RecipeId { get; set; }
        public List<string> UnSkippedMealId { get; set; }
        public List<string> UnSkippedRecipeId { get; set; }
    }
}
