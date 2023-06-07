using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Data.Entity
{
    public class SurveyQuestion : BaseEntity
    {
        public Guid SurveyId { get; set; }
        public Guid QuestionId { get; set; }
        
        public IEnumerable<Option> Options { get; set; }

        [ForeignKey("QuestionId")]
        public Question Question { get; set; }
        [ForeignKey("SurveyId")]
        public Survey Survey { get; set; }
    }
}
