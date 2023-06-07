using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Suggestic.HelperClass
{
    public class RemoveMealPlanData
    {
        public RemoveMealPlan removeMealPlan { get; set; }
    }
    public class RemoveMealPlan
    {
        public bool success { get; set; }
        public string message { get; set; }
    }
}
