using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Suggestic.HelperClass
{
    public class AddRecipesToShoppingList
    {
        public string message { get; set; }
        public bool success { get; set; }
    }

    public class AddToShoppingListData
    {
        public AddRecipesToShoppingList addRecipesToShoppingList { get; set; }
    }
}
