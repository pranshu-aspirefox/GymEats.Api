using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Suggestic.HelperClass
{
    public class Restriction
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool isOnProgram { get; set; }
        public string subcategory { get; set; }
        public string slugname { get; set; }
    }

    public class MyProfile
    {
        public string programName { get; set; }
        public DebugMealPlanVariables debugMealPlanVariables { get; set; }
        public List<Restriction> restrictions { get; set; }
    }

    public class DebugMealPlanVariables
    {
        public List<string> restrictions { get; set; }
    }


    public class UserRestrictionData
    {
        public MyProfile myProfile { get; set; }
    }
}
