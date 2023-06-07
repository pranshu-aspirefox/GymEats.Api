using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Suggestic.HelperClass
{
    public class Similar
    {
        public string id { get; set; }
        public string databaseId { get; set; }
        public string name { get; set; }
        public List<string> mealTags { get; set; }
        public int serving { get; set; }
        public int numberOfServings { get; set; }
        public string mainImage { get; set; }
        public NutrientsPerServing nutrientsPerServing { get; set; }
        public List<String> instructions { get; set; }

    }

    public class RecipeSwapOptions
    {
        public List<Similar> similar { get; set; }
    }

    public class RecipeSwapData
    {
        public RecipeSwapOptions recipeSwapOptions { get; set; }
    }
}
