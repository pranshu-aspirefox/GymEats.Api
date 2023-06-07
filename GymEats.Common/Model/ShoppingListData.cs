using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Common.Model
{
    public class ShoppingListNode
    {
        public string databaseId { get; set; }
        public string ingredient { get; set; }
        public string aisleName { get; set; }
        public int quantity { get; set; }
        public string unit { get; set; }
        public double grams { get; set; }
        public bool isDone { get; set; }
    }

    public class ShoppingListEdge
    {
        public ShoppingListNode node { get; set; }
    }

    public class ShoppingListAggregate
    {
        public List<ShoppingListEdge> edges { get; set; }
    }

    public class ShoppingListData
    {
        public ShoppingListAggregate shoppingListAggregate { get; set; }
    }
}
