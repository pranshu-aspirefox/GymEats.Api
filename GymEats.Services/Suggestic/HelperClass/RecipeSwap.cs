using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Suggestic.HelperClass
{
    public class SwapMealPlanRecipe
    {
        public bool success { get; set; }
    }

    public class SwapMealData
    {
        public SwapMealPlanRecipe swapMealPlanRecipe { get; set; }
    }
}
