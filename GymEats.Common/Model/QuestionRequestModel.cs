using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Common.Model
{
    public class QuestionRequestModel 
    {
        public string Label { get; set; }
        public bool IsPrimary { get; set; }
        public string CreatedBy { get; set; }
    }
}
