using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Nutritionix.HelperClass
{
    public class Branded
    {
        public string food_name { get; set; }
        public string serving_unit { get; set; }
        public string nix_brand_id { get; set; }
        public string brand_name_item_name { get; set; }
        public double? serving_qty { get; set; }
        public double nf_calories { get; set; }
        public Photo photo { get; set; }
        public string brand_name { get; set; }
        public int region { get; set; }
        public int brand_type { get; set; }
        public string nix_item_id { get; set; }
        public string locale { get; set; }
    }

    public class Common
    {
        public string food_name { get; set; }
        public string serving_unit { get; set; }
        public string tag_name { get; set; }
        public double? serving_qty { get; set; }
        public object common_type { get; set; }
        public string tag_id { get; set; }
        public Photo photo { get; set; }
        public string locale { get; set; }
    }

    public class Photo
    {
        public string thumb { get; set; }
        public object highres { get; set; }
        public bool is_user_uploaded { get; set; }
    }

    public class NutritionixItemsResponse
    {
        public List<Common> common { get; set; }
        public List<Branded> branded { get; set; }
    }


}
