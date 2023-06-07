using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Suggestic.HelperClass
{
    public class NutrientsPerServing
    {
        public double calories { get; set; }
        public double fat { get; set; }
        public double protein { get; set; }
        public double carbs { get; set; }
    }

    //public class Recipe
    //{
    //    public string id { get; set; }
    //    public string name { get; set; }
    //    public int numberOfServings { get; set; }
    //    public NutrientsPerServing nutrientsPerServing { get; set; }
    //    public string mainImage { get; set; }
    //}

    public class Meal
    {
        public string id { get; set; }
        public double calories { get; set; }
        public string meal { get; set; }
        public int numOfServings { get; set; }
        public Recipe recipe { get; set; }
        public bool IsSkipped { get; set; }

    }

    public class MealPlan
    {
        public int day { get; set; }
        public string date { get; set; }
        public double calories { get; set; }
        public List<Meal> meals { get; set; }
    }

    public class MealPlanData
    {
        public List<MealPlan> mealPlan { get; set; }
    }
}
