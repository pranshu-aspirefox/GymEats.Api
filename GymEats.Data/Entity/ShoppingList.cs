using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Data.Entity
{
    public class ShoppingList : BaseEntity
    {
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public string? ServingUnit { get; set; }
        public string? RecipeId { get; set; }
        public bool IsChecked { get; set; } = false;
    }
}
