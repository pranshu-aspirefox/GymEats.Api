using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Common.Model
{
    public class QuestionSurveyRequest
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public bool IsPrimary { get; set; }
        public List<OptionViewModel> Options { get; set; }
        public string createdBy { get; set; }
        public Guid SurveyId { get; set; }
    }
}
