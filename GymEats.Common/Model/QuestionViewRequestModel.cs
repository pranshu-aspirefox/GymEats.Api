using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Common.Model
{
    public class QuestionViewRequestModel
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public bool IsPrimary { get; set; }
        public List<OptionViewModel> Options { get; set; }
        public Guid SurveyQuestionId { get; set; }
        public QuestionViewModel Question { get; set; }
    }
}
