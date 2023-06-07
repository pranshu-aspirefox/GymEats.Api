using GymEats.Common.Model;
using GymEats.Services.Mealme.HelperClass;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using System.Security.Cryptography.Xml;
using RestSharp;
using Newtonsoft.Json;
using GymEats.Services.Nutritionix;
using GymEats.Services.Service;
using GymEats.Services.Nutritionix.HelperClass;
using EllipticCurve.Utils;
using GymEats.Services.Suggestic.HelperClass;
using GymEats.Data.Entity;
using GymEats.Services.Auth;

namespace GymEats.Services.Mealme
{
    public class MealmeService : IMealmeService
    {
        private string token;
        public readonly IConfiguration _config;
        private readonly INutritionixService _nutritionixService;
        private readonly SuggesticApiService _suggesticApiService;
        private readonly IUserShoppingService _userShoppingService;
        private readonly IAuthService _authService;

        public MealmeService(IConfiguration configuration, INutritionixService nutritionixService, SuggesticApiService suggesticApiService, IUserShoppingService userShoppingService, IAuthService authService)
        {
            _config = configuration;
            _nutritionixService = nutritionixService;
            _suggesticApiService = suggesticApiService;
            _userShoppingService = userShoppingService;
            _authService = authService;
            token = _config.GetSection("Mealme:ApiKey").Value.ToString();
        }


        public async Task<SearchProductResponse> ProductSearchByName(ProductSearchRequest request)
        {
            var url = _config.GetSection("Mealme:BaseUrl").Value.ToString();
            url += "/search/product/v4";
            try
            {
                UriBuilder builder = new UriBuilder(url);
                builder.Query = $"query=[\"{request.query}\"]&store_type={request.store_type}&user_latitude={request.user_latitude}&user_longitude={request.user_longitude}&pickup={request.pickup}&budget={request.budget}&user_street_num={request.user_street_num}&user_street_name={request.user_street_name}&user_city={request.user_city}&user_state={request.user_state}&user_zipcode={request.user_zipcode}&fetch_quotes={request.fetch_quotes}&fuzzy_search={request.fuzzy_search}&open={request.open}&maximum_miles={request.maximum_miles}&sale={request.sale}&autocomplete={request.autocomplete}";

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Id-Token", token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var result = client.GetAsync(builder.Uri);
                    if (result.Result.StatusCode == HttpStatusCode.OK)
                    {
                        var data = await result.Result.Content.ReadAsStringAsync();
                        var returnRes = JsonConvert.DeserializeObject<SearchProductResponse>(data);
                        if (returnRes != null)
                        {
                            return returnRes;
                        }
                    }
                }
            }
            catch (Exception ex) { throw ex; }
            return new SearchProductResponse();
        }

        public async Task<List<Product>> GetFinalMealList(string mealName, double calorie, double protein, double fat, double carbs, int day, string mealType)
        {
            var searchProductOnMealme = new ProductSearchRequest
            {
                query = mealName,
                store_type= "restaurant",
                user_latitude = 37.7786357,
                user_longitude = -122.3918135,
                pickup = false,
                budget = 20,
                user_street_num = "188",
                user_street_name = "King Street",
                user_city = "San Francisco",
                user_state = "CA",
                user_zipcode =  "94107",
                user_country =  "US",
                fetch_quotes = false,
                sort = "relevance",
                fuzzy_search = false,
                open = false,
                maximum_miles = 1.5,
                sale =  false,
                autocomplete =  true
            };

            var caloriMinRange = CalculateMinPercent(calorie, 20);
            var caloriMaxRange = CalculateMaxPercent(calorie, 20);

            var finalItemList = new List<Product>();

            //Search meal on Mealme from Suggestic
            var mealmeResult = await ProductSearchByName(searchProductOnMealme);
            if (mealmeResult.products.Any())
            {
                foreach(var product in mealmeResult.products)
                {
                    //case 1:meal from suggestic matches exactly with mealme
                    if(mealName == product.item_name)
                    {
                        product.EatableType = EatbleFoodType.Green.ToString();
                        finalItemList.Add(product);
                    }

                    //case 2: finds its calorie and protein from NX
                    var nixItemResult = await _nutritionixService.GetNutritionixItemByName(product.item_name);
                    if(nixItemResult.Any())
                    {
                        foreach(var item in nixItemResult)
                        {
                            //checking calorie range on NX
                            if (item.nf_calories >= (double)caloriMinRange && item.nf_calories <= (double)caloriMaxRange)
                            {
                                if (product.item_name == item.food_name)
                                {
                                    product.EatableType = EatbleFoodType.Green.ToString();
                                    finalItemList.Add(product);
                                }
                                // if name partially match
                                if (item.food_name.ToLower().Contains(product.item_name.ToLower()))
                                {
                                    //find nutrition details in NX
                                    var getMicroDetailsOfMeal = await _nutritionixService.GetMealDetaisById(item.nix_item_id);
                                    if((int)item.nf_calories  >= CalculateMinPercent(calorie, 10) && (int)item.nf_calories <= CalculateMaxPercent(calorie, 10))
                                    {
                                        if(getMicroDetailsOfMeal != null && getMicroDetailsOfMeal.nf_protein != null) 
                                        {
                                            if(getMicroDetailsOfMeal.nf_protein >= (double)CalculateMinPercent(protein, 20) && getMicroDetailsOfMeal.nf_protein <= (double)CalculateMaxPercent(protein, 20))
                                            {
                                                product.EatableType = EatbleFoodType.Green.ToString();
                                                product.Calorie = item.nf_calories;
                                                product.Protein = getMicroDetailsOfMeal.nf_protein;
                                                product.Fat = getMicroDetailsOfMeal.nf_total_fat;
                                                product.Carbs = getMicroDetailsOfMeal.nf_total_carbohydrate;
                                                product.MealType = mealType;
                                                product.Day = day;

                                                finalItemList.Add(product);
                                            }
                                        }
                                    }
                                    else if (item.nf_calories > (double)CalculateMinPercent(calorie, 20) && item.nf_calories <= (double)CalculateMaxPercent(calorie, 20))
                                    {
                                        if (getMicroDetailsOfMeal != null && getMicroDetailsOfMeal.nf_protein != null)
                                        {
                                            if (getMicroDetailsOfMeal.nf_protein >= (double)CalculateMinPercent(protein, 30) && getMicroDetailsOfMeal.nf_protein <= (double)CalculateMaxPercent(protein, 30))
                                            {
                                                product.EatableType = EatbleFoodType.Yellow.ToString();
                                                product.Calorie = item.nf_calories;
                                                product.Protein = getMicroDetailsOfMeal.nf_protein;
                                                product.Fat = getMicroDetailsOfMeal.nf_total_fat;
                                                product.Carbs = getMicroDetailsOfMeal.nf_total_carbohydrate;
                                                product.MealType = mealType;
                                                product.Day = day;

                                                finalItemList.Add(product);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        product.EatableType = EatbleFoodType.Red.ToString();
                                        product.Calorie = item.nf_calories;
                                        product.Protein = getMicroDetailsOfMeal.nf_protein;
                                        product.Fat = getMicroDetailsOfMeal.nf_total_fat;
                                        product.Carbs = getMicroDetailsOfMeal.nf_total_carbohydrate;
                                        product.MealType = mealType;
                                        product.Day = day;

                                        finalItemList.Add(product);
                                    }
                                }
                            }


                         }
                    }
                }
                if(finalItemList.Count == 0)
                {
                    var meal = mealmeResult.products?.FirstOrDefault();
                    meal.EatableType = EatbleFoodType.Green.ToString();
                    meal.MealType = mealType;
                    meal.Day = day;
                    finalItemList.Add(meal);
                    
                }

            }
            return finalItemList;
        }

        public async Task<List<Product>> GetAllMealList(List<MealListRequest> mealList)
        {
            bool isSuggestic = true; 
            var finalItemList = new List<Product>();
            foreach(var meal in mealList)
            {
                var res = await GetFinalMealList(meal.MealName, meal.Calorie, meal.Protein, meal.Fat, meal.Carbs, meal.Day, meal.MealType);
                if(res.Any())
                    finalItemList.AddRange(res);
                var swapList = new RecipeSwapData();
                
                swapList = await _suggesticApiService.GetSwapOptions(meal.RecipeId, (int)Math.Ceiling(meal.Serving));
                isSuggestic = false;
                if (swapList.recipeSwapOptions != null)
                {
                    foreach (var item in swapList.recipeSwapOptions.similar)
                    {
                        var resList = await GetFinalMealList(item.name, item.nutrientsPerServing.calories, item.nutrientsPerServing.protein, item.nutrientsPerServing.fat, item.nutrientsPerServing.carbs, meal.Day, meal.MealType);
                        if (resList.Any())
                        {
                            finalItemList.AddRange(resList);
                        }
                    }
                }
            }
            
            return finalItemList;

        }

        public async Task<List<ShoppingList>> AddItemsFromSuggesticToShoppingList(string userId)
        {
            var user = await _authService.GetUserById(userId);
            if (user.SuggesticId != null)
            {
                var res = (await _suggesticApiService.GetShoppingList(user.SuggesticId)).shoppingListAggregate.edges;
                var itemList = res.Select(x=>x.node.ingredient).Distinct();
                if (itemList.Any())
                {
                    foreach (var item in itemList)
                    {
                        var productSearch = new ProductSearchRequest
                        {
                            query = item,
                            store_type = "grocery",
                            user_latitude = 37.7786357,
                            user_longitude = -122.3918135,
                            pickup = false,
                            budget = 20,
                            user_street_num = "188",
                            user_street_name = "King Street",
                            user_city = "San Francisco",
                            user_state = "CA",
                            user_zipcode = "94107",
                            user_country = "US",
                            fetch_quotes = false,
                            sort = "relevance",
                            fuzzy_search = false,
                            open = false,
                            maximum_miles = 1.5,
                            sale = false,
                            autocomplete = true
                        };
                        var searchData = await ProductSearchByName(productSearch);
                        if (searchData.products.Any())
                        {
                            var mealRes = searchData.products.Where(x=>x.item_name.ToLower() ==item.ToLower()).FirstOrDefault();
                            if(mealRes != null)
                            {
                                var resData = new UserCartRequestModel
                                {
                                    UserId = userId,
                                    ProductName = mealRes.item_name,
                                    ProductId = mealRes.product_id,
                                    Price = mealRes.price,
                                    ServingUnit = mealRes.unit_size.ToString(),
                                    Quantity = 1,
                                    IsChecked = false
                                };
                                var result = await _userShoppingService.AddItemToShoppingList(resData);
                            }
                        }
                    }
                }
            }
                return await _userShoppingService.GetAllListtItems(userId);
        }

        private double CalculateMaxPercent(double num, int percent)
        {
            return (double)Math.Round((decimal)num + (decimal)num * (decimal)((decimal)percent / 100)); 
        }
        private double CalculateMinPercent(double num, int percent)
        {
            return (double)Math.Round((decimal)num - (decimal)num * (decimal)((decimal)percent / 100));
        }

    }
}
