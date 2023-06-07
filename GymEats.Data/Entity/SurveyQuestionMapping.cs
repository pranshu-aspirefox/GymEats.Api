using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Data.Entity
{
    public class SurveyQuestionMapping
    {
        public Guid Id { get; set; }
        public Guid SurveyId { get; set; }
        public Guid QuestionId { get; set; }
    }
}
