using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Suggestic.HelperClass
{
    public class CreateMealEntry
    {
        public bool success { get; set; }
        public string message { get; set; }
    }

    public class SkipMealData
    {
        public CreateMealEntry createMealEntry { get; set; }
    }
}
