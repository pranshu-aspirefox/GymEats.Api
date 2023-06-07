using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Suggestic.HelperClass
{
    public class RecipeSearchNode
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<string> instructions { get; set; }
        public string mainImage { get; set; }
        public NutrientsPerServing nutrientsPerServing { get; set; }
    }

    public class RecipeSearchEdge
    {
        public RecipeSearchNode node { get; set; }
    }

    public class RecipeSearch
    {
        public List<RecipeSearchEdge> edges { get; set; }
    }

    public class RecipeSearchData
    {
        public RecipeSearch recipeSearch { get; set; }
    }
}
