using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Common.Model
{
    public class SurveyViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid PrimaryQuestion { get; set; }
        public DateTime DeletedOn { get; set; }
        public string SurveyJson { get; set; }
        public string CreatedBy { get; set; }

    }
}
