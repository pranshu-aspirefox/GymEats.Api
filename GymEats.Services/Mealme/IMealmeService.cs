using GymEats.Common.Model;
using GymEats.Data.Entity;
using GymEats.Services.Mealme.HelperClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Mealme
{
    public interface IMealmeService
    {
        Task<SearchProductResponse> ProductSearchByName(ProductSearchRequest request);
        Task<List<Product>> GetFinalMealList(string mealName, double calorie, double protein, double fat, double carbs, int day, string mealType);
        Task<List<Product>> GetAllMealList(List<MealListRequest> mealList);
        Task<List<ShoppingList>> AddItemsFromSuggesticToShoppingList(string userId);
    }
}
