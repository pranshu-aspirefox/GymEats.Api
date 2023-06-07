using GymEats.Common.Model;
using GymEats.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Service
{
    public interface IUserShoppingService
    {
        Task<List<ShoppingList>> GetAllListtItems(string userId);
        Task<int> AddItemToShoppingList(UserCartRequestModel model);
        Task<int> UpdateItemToShoppingList(int quantity, string productId, string userId);
        Task<double> GetTotalPriceOfItem(string userId);
    }
}
