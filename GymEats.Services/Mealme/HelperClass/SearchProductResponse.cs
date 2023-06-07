using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Mealme.HelperClass
{
    public class Address
    {
        public string street_addr { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
        public string country { get; set; }
        public string street_addr_2 { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Delivery
    {
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }
    }

    public class DineIn
    {
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }
    }

    public class LocalHours
    {
        public Operational operational { get; set; }
        public Delivery delivery { get; set; }
        public Pickup pickup { get; set; }
        public DineIn dine_in { get; set; }
    }

    public class Operational
    {
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }
    }

    public class Pickup
    {
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }
    }

    public class Product
    {
        public string product_id { get; set; }
        public string item_name { get; set; }
        public string image { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public int price { get; set; }
        public string formatted_price { get; set; }
        public int original_price { get; set; }
        public List<object> upc_codes { get; set; }
        public object unit_size { get; set; }
        public string unit_of_measurement { get; set; }
        public List<object> attributes { get; set; }
        public bool should_fetch_customizations { get; set; }
        public string menu_id { get; set; }
        public int grand_total { get; set; }
        public Store store { get; set; }
        public double? Calorie { get; set; }
        public double? Protein { get; set; }
        public double? Fat { get; set; }
        public double? Carbs { get; set; }
        public string? MealType { get; set; }
        public int? Day { get; set; }
        public string? EatableType { get; set; }
    }

    public class SearchProductResponse
    {
        public List<Product> products { get; set; }
    }

    public class Store
    {
        public string _id { get; set; }
        public string name { get; set; }
        public object phone_number { get; set; }
        public Address address { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public LocalHours local_hours { get; set; }
        public int? dollar_signs { get; set; }
        public bool pickup_enabled { get; set; }
        public bool delivery_enabled { get; set; }
        public bool is_open { get; set; }
        public List<string> logo_photos { get; set; }
        public bool offers_first_party_delivery { get; set; }
        public bool offers_third_party_delivery { get; set; }
        public double miles { get; set; }
        public double weighted_rating_value { get; set; }
        public int aggregated_rating_count { get; set; }
    }


}
