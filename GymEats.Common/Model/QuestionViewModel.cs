using GymEats.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Common.Model
{
    public class QuestionViewModel
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public bool IsPrimary { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
