using GymEats.Services.Nutritionix.HelperClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Nutritionix
{
    public interface INutritionixService
    {
        Task<List<Branded>> GetNutritionixItemByName(string name);
        Task<Food> GetMealDetaisById(string nixItemId);
        Task<Food> GetMealInfoByName(string name);
    }
}
