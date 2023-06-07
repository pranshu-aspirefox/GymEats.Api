using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Common.Model
{
    public class DietViewModel
    {
        public Guid Id { get; set; }

        public string? DietName { get; set; }

        public int ProteinPercentage { get; set; }

        public int CarbsPercentage { get; set; }


        public int FatPercentage { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDefault { get; set; }

        public int SurplusPercentage { get; set; }

        public int DeficitPercentage { get; set; }

        public string? MealSchedule { get; set; }
    }
}
