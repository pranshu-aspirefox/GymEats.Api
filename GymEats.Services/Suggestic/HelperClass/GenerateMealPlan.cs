using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Suggestic.HelperClass
{
    public class GenerateMealPlan
    {
        public bool success { get; set; }
        public string message { get; set; }
    }

    public class GenerateMealPlanData
    {
        public GenerateMealPlan generateMealPlan { get; set; }
    }
    public class GenerateMealRoot
    {
        public GenerateMealPlanData data { get; set; }
    }

    public class GenerateSimpleMealPlan
    {
        public bool success { get; set; }
        public string message { get; set; }
    }

    public class GenerateSimpleMealPlanData
    {
        public GenerateSimpleMealPlan generateSimpleMealPlan { get; set; }
    }
}
