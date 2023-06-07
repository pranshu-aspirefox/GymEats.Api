using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Data.Entity
{
    public class SurveyDiet : BaseEntity
    {
        public Guid DietId { get; set; }
        public Guid OptionId { get; set; }

        [ForeignKey("OptionId")]
        public Option Option { get; set; }
        [ForeignKey("DietId")]
        public Diet Diet { get; set; }
    }
}
