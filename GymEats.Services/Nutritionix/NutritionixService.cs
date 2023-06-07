using GymEats.Services.Nutritionix.HelperClass;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GymEats.Services.Nutritionix
{
    public class NutritionixService : INutritionixService
    {
        private string appId;
        private string appKey;
        public readonly IConfiguration _config;
        public NutritionixService(IConfiguration configuration)
        {
            _config = configuration;
            appId = _config.GetSection("Nutritionix:x-app-id")?.Value.ToString();
            appKey = _config.GetSection("Nutritionix:x-app-key")?.Value.ToString();
        }

        public async Task<List<Branded>> GetNutritionixItemByName(string name)
        {
            var url = _config.GetSection("Nutritionix:BaseUrl").Value.ToString();
            url += "/search/instant";
            try
            {
                UriBuilder builder = new UriBuilder(url);
                builder.Query = $"query={name}";

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("x-app-id", appId);
                    client.DefaultRequestHeaders.Add("x-app-key", appKey);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var result = await client.GetAsync(builder.Uri);
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        var data = await result.Content.ReadAsStringAsync();
                        var returnRes = JsonConvert.DeserializeObject<NutritionixItemsResponse>(data);
                        if (returnRes != null)
                        {
                            return returnRes.branded;
                        }
                    }
                }
            }
            catch (Exception ex) { throw ex; }

            return new List<Branded>();
        }

        public async Task<Food> GetMealDetaisById(string nixItemId)
        {
            var url = _config.GetSection("Nutritionix:BaseUrl").Value.ToString();
            url += "/search/item";
            try
            {
                UriBuilder builder = new UriBuilder(url);
                builder.Query = $"nix_item_id={nixItemId}";

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("x-app-id", appId);
                    client.DefaultRequestHeaders.Add("x-app-key", appKey);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var result = await client.GetAsync(builder.Uri);
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        var data = await result.Content.ReadAsStringAsync();
                        var returnRes = JsonConvert.DeserializeObject<NXMealResponse>(data);
                        if (returnRes.foods.Any())
                        {
                            return returnRes.foods.FirstOrDefault();
                        }
                    }
                }


            }
            catch (Exception ex) { throw ex; }
            return new Food();
        }

        public async Task<Food> GetMealInfoByName(string name)
        {
            var item = new Food();
            var itemList = await GetNutritionixItemByName(name);
            var brandItem = new Branded();
            var nameArr = StringToArrayConvert(name);
            if(nameArr.Length > 0)
            {
                for(int i = 0; i < nameArr.Length; i++)
                {
                    
                    brandItem = itemList.Where(x => x.food_name.ToLower().Contains(nameArr[i].ToString().ToLower())).FirstOrDefault();
                    if(!string.IsNullOrEmpty(brandItem.nix_item_id))
                        break;
                    
                }
            }
            if (!string.IsNullOrEmpty(brandItem.nix_item_id))
            {
                item = await GetMealDetaisById(brandItem.nix_item_id);
            }
            else
            {
                if(itemList.Any())
                    item = await GetMealDetaisById(itemList.FirstOrDefault().nix_item_id);
            }
            return item;    
        }

        private string[] StringToArrayConvert(string str)
        {
            if (str.Contains(" "))
            {
                return str.Split(" ").ToArray<string>();
            }
            return new string[] { str};
        }

    }
}
