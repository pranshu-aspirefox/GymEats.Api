using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Suggestic.HelperClass
{
    public class Ingredient
    {
        public string name { get; set; }
    }

    public class Source
    {
        public string siteUrl { get; set; }
        public string displayName { get; set; }
        public string recipeUrl { get; set; }
    }

    public class ParsedIngredientLine
    {
        public string ingredientLine { get; set; }
        public string ingredient { get; set; }
        public string quantity { get; set; }
        public string unit { get; set; }
        public string other { get; set; }
    }

    public class NutritionalInfo
    {
        public double calories { get; set; }
        public double protein { get; set; }
        public double carbs { get; set; }
        public double fat { get; set; }
    }

    public class Recipe
    {
        public string id { get; set; }
        public NutrientsPerServing nutrientsPerServing { get; set; }
        public List<ParsedIngredientLine> parsedIngredientLines { get; set; }
        public string databaseId { get; set; }
        public string totalTime { get; set; }
        public int totalTimeInSeconds { get; set; }
        public string name { get; set; }
        public int serving { get; set; }
        //public int? numberOfServings { get; set; }
        public List<string> ingredientLines { get; set; }
        public List<Ingredient> ingredients { get; set; }
        public string language { get; set; }

        public List<string> courses { get; set; }
        public List<String> cuisines { get; set; }
        public Source source { get; set; }
        public string mainImage { get; set; }

        public int ingredientsCount { get; set; }
        public double weightInGrams { get; set; }
        public double servingWeight { get; set; }
        public List<String> instructions { get; set; }
        public NutritionalInfo nutritionalInfo { get; set; }
    }

    public class RecipeData
    {
        public Recipe recipe { get; set; }
    }

    public class Root
    {
        public RecipeData data { get; set; }
    }
}
