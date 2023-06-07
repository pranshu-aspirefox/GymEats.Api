using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Common.Model
{
    public class SurveyQuestionRequest
    {
        public string QuestionId { get; set; }
        public string Label { get; set; }
        public bool IsPrimary { get; set; }
        public List<OptionSurveyViewModel> Options { get; set; }
        public string? CreatedBy { get; set; }
        
    }
}
