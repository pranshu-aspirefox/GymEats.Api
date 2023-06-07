using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Data.Entity
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("CreatedBy")]
        public User? UserCreatedBy { get; set; }

        [ForeignKey("UpdatedBy")]
        public User? UserUpdatedBy { get; set; }
    }
}
