using GymEats.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Common.Model
{
    public class OptionRequestModel
    {
        public string Label { get; set; }
        public bool IsExclusive { get; set; }
        public string CreatedBy { get; set; }
    }
}
