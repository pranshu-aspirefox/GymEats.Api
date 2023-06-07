using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Data.Entity
{
    public class Question : BaseEntity
    {
        public string Label { get; set; }
        public bool IsPrimary { get; set; }
        public int AnswerType { get; set; }
    }
}
