using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Suggestic.HelperClass
{
    public class RestaurantNode
    {
        public string name { get; set; }
        public object shortName { get; set; }
        public bool isRecomended { get; set; }
        public string address1 { get; set; }
        public string cityTown { get; set; }
        public string country { get; set; }
        public string recommendation { get; set; }
        public int recommendationsCount { get; set; }
    }

    public class RestaurantEdge
    {
        public RestaurantNode node { get; set; }
    }

    public class RestaurantSearch
    {
        public int totalCount { get; set; }
        public List<RestaurantEdge> edges { get; set; }
    }

    public class RestaurantData
    {
        public RestaurantSearch restaurantSearch { get; set; }
    }
}
