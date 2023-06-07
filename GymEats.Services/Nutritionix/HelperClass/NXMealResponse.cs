using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Nutritionix.HelperClass
{
    public class Food
    {
        public string food_name { get; set; }
        public string brand_name { get; set; }
        public double? serving_qty { get; set; }
        public string serving_unit { get; set; }
        public int? serving_weight_grams { get; set; }
        public int? nf_metric_qty { get; set; }
        public string nf_metric_uom { get; set; }
        public double? nf_calories { get; set; }
        public double? nf_total_fat { get; set; }
        public double? nf_saturated_fat { get; set; }
        public int? nf_cholesterol { get; set; }
        public int? nf_sodium { get; set; }
        public double nf_total_carbohydrate { get; set; }
        public int? nf_dietary_fiber { get; set; }
        public double? nf_sugars { get; set; }
        public double? nf_protein { get; set; }
        public object nf_potassium { get; set; }
        public object nf_p { get; set; }
        public List<FullNutrient> full_nutrients { get; set; }
        public string nix_brand_name { get; set; }
        public string nix_brand_id { get; set; }
        public string nix_item_name { get; set; }
        public string nix_item_id { get; set; }
        public Metadata metadata { get; set; }
        public int? source { get; set; }
        public object ndb_no { get; set; }
        public object tags { get; set; }
        public object alt_measures { get; set; }
        public object lat { get; set; }
        public object lng { get; set; }
        public Photo photo { get; set; }
        public object note { get; set; }
        public object class_code { get; set; }
        public object brick_code { get; set; }
        public object tag_id { get; set; }
        public DateTime updated_at { get; set; }
        public string nf_ingredient_statement { get; set; }
    }

    public class FullNutrient
    {
        public int attr_id { get; set; }
        public double value { get; set; }
    }

    public class Metadata
    {
    }

    public class NXMealResponse
    {
        public List<Food> foods { get; set; }
    }


}
