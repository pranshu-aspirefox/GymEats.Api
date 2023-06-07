using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Common.Model
{
    public class OptionSurveyViewModel
    {
        public int Question_Diet { get; set; }
        public SurveyQuestionRequestModel? Question { get; set; }
        public DeitSurveyViewModel? Diet { get; set; }
    }
}
