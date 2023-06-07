using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Mealme.HelperClass
{
    public class ProductSearchRequest
    {
        public string query { get; set; } 
        public string store_type { get; set; }
        public double user_latitude { get; set; }
        public double user_longitude { get; set; }
        public bool pickup { get; set; } = false;
        public int budget { get; set; } = 20;
        public string user_street_num { get; set; } 
        public string user_street_name { get; set; } 
        public string user_city { get; set;}
        public string user_state { get; set; }
        public string user_zipcode { get; set;} 
        public string user_country { get; set;}
        public bool fetch_quotes { get; set; } = false;
        public string sort { get; set; }
        public bool fuzzy_search { get; set; } = false;
        public bool open { get; set; } = false;
        public double maximum_miles { get; set; } = 1.5;
        public bool sale { get; set; } = false; 
        public bool autocomplete { get; set; } = true;

    }
}
