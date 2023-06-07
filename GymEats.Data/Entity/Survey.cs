using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Data.Entity
{
    public class Survey : BaseEntity
    {
        public string Name { get; set; }
        public Guid PrimaryQuestion { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string SurveyJson { get; set; }
    }
}
