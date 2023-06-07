using GymEats.Common.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymEats.Data.Entity
{
    public class Option : BaseEntity
    {

        public string Label { get; set; }
        public bool IsExclusive { get; set; }
    }
}
