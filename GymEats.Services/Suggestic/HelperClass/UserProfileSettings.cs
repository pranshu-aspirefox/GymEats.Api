using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Suggestic.HelperClass
{
    public class ProfileMealPlanSettings
    {
        public bool success { get; set; }
    }

    public class ProfileSettingData
    {
        public ProfileMealPlanSettings profileMealPlanSettings { get; set; }
    }
}
