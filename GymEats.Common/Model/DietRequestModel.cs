using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Common.Model
{
    public class DietRequestModel
    {

        public string DietName { get; set; }
        public int ProteinPercentage { get; set; }
        public int CarbsPercentage { get; set; }
        public int FatPercentage { get; set; }
        public int SurplusPercentage { get; set; }
        public int DeficitPercentage { get; set; }
        public string MealSchedule { get; set; }
        public bool IsDefault { get; set; } 
        public string CreatedBy { get; set; }
    }
}
