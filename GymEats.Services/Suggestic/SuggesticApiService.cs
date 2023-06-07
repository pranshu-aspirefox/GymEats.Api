using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using GymEats.Common.Model;
using GymEats.Data.Entity;
using GymEats.Services.Auth;
using GymEats.Services.Diet;
using GymEats.Services.Suggestic.HelperClass;
using Microsoft.AspNetCore.Identity;
using ModernHttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Service
{
    public class SuggesticApiService
    {
        private readonly GraphQLHttpClient _client;
        private readonly IAuthService _authService;
        private readonly IDietService _dietService;
        private readonly UserManager<User> _userManager;
        private readonly GraphQLHttpClient _Jwtclient;

        public SuggesticApiService(IAuthService authService, IDietService dietService, UserManager<User> userManager)
        {
            _authService = authService;
            _dietService = dietService;
            _userManager = userManager;
            var uri = new Uri("https://production.suggestic.com/graphql");
            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = uri,
                HttpMessageHandler = new NativeMessageHandler(),

            };

            _client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer());
            _client.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.HttpClient.DefaultRequestHeaders.Add("Authorization", "Token ff72091d07d6619830c952a70fe1339a22ff5ecf");
        }

        public async Task<SuggesticUserData> CreateUser(string email, string name)
        {
            try
            {


                var query = new GraphQLRequest
                {
                    Query = @"
                mutation($email:String! ,$name:String!) {
                createUser(name: $name, email:$email ) {
                success
                message
                user{
                    id
                    databaseId
                    email
                    name
                    }
                }
               }",
                    Variables = new { email = email, name = name }

                };

                var response = await _client.SendMutationAsync<SuggesticUserData>(query);
                var userData = response.Data;

                return userData;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        public async Task<MealPlanData> GetUserMealPlan(String id)
        {

            try
            {
                _client.HttpClient.DefaultRequestHeaders.Add("sg-user", id);

                var query = new GraphQLRequest
                {
                    Query = @"query{
                        mealPlan {day, date(useDatetime: false) ,calories
                       meals { id,calories,meal,numOfServings
                              recipe {id,databaseId,name,serving, numberOfServings,mainImage ,nutrientsPerServing {calories fat protein carbs } }
                                }
                                 }
                                }"
                };

                var response = await _client.SendMutationAsync<MealPlanData>(query);
                var userData = response.Data;

                return userData;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        public async Task<MealPlanData> GetMeals()
        {
            try
            {
                var query = new GraphQLRequest
                {
                    Query = @"query{
                        mealPlan {day, date(useDatetime: false) ,calories
                       meals { id,calories,meal,numOfServings
                              recipe {id,databaseId,name, numberOfServings,mainImage ,nutrientsPerServing {calories fat protein carbs } }
                                }
                                 }
                                }"
                };

                var response = await _client.SendMutationAsync<MealPlanData>(query);
                var userData = response.Data;

                return userData;
            }
            catch (Exception exp)
            {

                throw exp;
            }
        }

        public async Task<RecipeData> GetRecipeById(string id, string suggesticUserId="")
        {
            if(!string.IsNullOrEmpty(suggesticUserId))
                _client.HttpClient.DefaultRequestHeaders.Add("sg-user", suggesticUserId);
            try
            {

                var query = new GraphQLRequest
                {
                    Query = @"
                query recipeQuery($id:ID!){
                      recipe(id:$id) { id,nutrientsPerServing{ calories}
                        databaseId, totalTime,totalTimeInSeconds,name,serving, numberOfServings,ingredientLines,
                        ingredients { name },language, courses, cuisines,
                        source {siteUrl, displayName, recipeUrl },
                        mainImage,  ingredientsCount, weightInGrams, servingWeight, instructions,
                        nutrientsPerServing { calories, protein, carbs,fat }
                        parsedIngredientLines{ ingredientLine,ingredient, quantity, unit ,other }
                       }
                    }",
                    Variables = new { id = id }

                };

                var response = await _client.SendQueryAsync<RecipeData>(query);
                var recipeData = response.Data;

                return recipeData;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        public async Task<RecipeSwapData> GetSwapOptions(string recipeId, int serving, string suggesticUserId = "")
        {
            if (!string.IsNullOrEmpty(suggesticUserId))
                _client.HttpClient.DefaultRequestHeaders.Add("sg-user", suggesticUserId);
            try
            {

                var query = new GraphQLRequest
                {
                    Query = @"
                query recipeQuery($id:String,, $serving:Int){
                      recipeSwapOptions( recipeId: $id , serving:$serving ) {
                       similar {id, databaseId, name,mainImage, mealTags,serving,numberOfServings,
                      nutrientsPerServing { calories, protein, carbs,fat },instructions}
                      }
                     }",
                    Variables = new { id = recipeId, serving = serving }

                };

                var swapData = new RecipeSwapData();
                for (int i = 0; i < 3; i++)
                {
                    var response = await _client.SendQueryAsync<RecipeSwapData>(query);
                    var recipeData = response.Data;
                    var recipeList = recipeData?.recipeSwapOptions?.similar.FindAll(x => x.id != recipeId);
                    recipeList = recipeList?.GroupBy(x => x.name).Select(g => g.First()).ToList();
                    if (recipeList != null)
                    {
                        swapData = recipeData;
                        swapData.recipeSwapOptions.similar = recipeList;
                        break;
                    }
                }
                return swapData;
            }
            catch (Exception exp)
            {
                throw;
            }
        }

        public async Task<RecipeSearchData> GetRecipeSearchOption(string recipeName, string recipeId, int calories, string suggesticUserId)
        {
            _client.HttpClient.DefaultRequestHeaders.Add("sg-user", suggesticUserId);
            try
            {

                var query = new GraphQLRequest
                {
                    Query = @"
                query recipeQuery($Name:String, $Gtcalories:Int, $Ltcalories:Int){
                      recipeSearch(query:$Name ,
                                    macroNutrientsRange: {
                                                calories: { gte: $Gtcalories, lte: $Ltcalories},
                                    }
                                    first: 3) {
                                            edges {
                                                node {
                                                    id
                                                    name
                                                    mainImage
                                                    instructions
                                nutrientsPerServing {
                                                        calories
                                                        protein
                                                        fat
                                                        carbs
                                                        omega3
                                                    }
                        }
                    }
                }
            }",
                    Variables = new { Name = recipeName, Gtcalories = ((int)Math.Round(calories / 10.0)) * 10, Ltcalories = ((int)Math.Floor(calories / 10.0)) }

                };

                var swapData = new RecipeSearchData();
                for (int i = 0; i < 3; i++)
                {
                    var response = await _client.SendQueryAsync<RecipeSearchData>(query);
                    var recipeData = response.Data;
                    var recipeList = recipeData.recipeSearch.edges.FindAll(x => x.node.instructions != null && x.node.instructions.Count != 0 && x.node.id != recipeId);
                    recipeList = recipeList.GroupBy(x => x.node.name).Select(g => g.First()).ToList();
                    if (recipeList != null)
                    {
                        swapData = recipeData;
                        swapData.recipeSearch.edges = recipeList;
                        break;
                    }
                }
                return swapData;
            }
            catch (Exception exp)
            {
                throw;
            }
        }

        public async Task<SwapMealData> MealSwap(string suggesticUserId, string recipeiD, string mealId)
        {

            try
            {
                _client.HttpClient.DefaultRequestHeaders.Add("sg-user", suggesticUserId);

                var query = new GraphQLRequest
                {
                    Query = @"
                mutation($recipeId:String! ,$mealId:String) {
                swapMealPlanRecipe( recipeId:$recipeId, mealId:$mealId ) {
                success
              }
            }",
                    Variables = new { recipeId = recipeiD, mealId = mealId }

                };

                var response = await _client.SendMutationAsync<SwapMealData>(query);
                var userData = response.Data;

                return userData;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        public async Task<RestrictionData> GetAllRestrictions(string suggesticUserId)
        {
            _client.HttpClient.DefaultRequestHeaders.Add("sg-user", suggesticUserId);
            try
            {

                var query = new GraphQLRequest
                {
                    Query = @"
                query {
                      restrictions{ edges { node { id, name, subcategory, slugname, isOnProgram } } }
                    }"

                };

                var response = await _client.SendQueryAsync<RestrictionData>(query);
                var recipeData = response.Data;

                return recipeData;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        public async Task<GenerateMealPlanData> GenerateMealPlan()
        {
            //_client.HttpClient.DefaultRequestHeaders.Add("sg-user", suggesticUserId);
            try
            {

                var query = new GraphQLRequest
                {
                    Query = @"
                mutation {
                          generateMealPlan {
                            success
                            message
                          }
                        }"

                };

                var response = await _client.SendMutationAsync<GenerateMealPlanData>(query);
                var recipeData = response.Data;

                return recipeData;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        public async Task<ProfileRestrictionsUpdateData> AddUserRestriction(string suggesticUserId, string[] id)
        {
            _client.HttpClient.DefaultRequestHeaders.Add("sg-user", suggesticUserId);
            try
            {

                var query = new GraphQLRequest
                {
                    Query = @"
                mutation($id:[ID]) {
                          profileRestrictionsUpdate(restrictions: $id) 
                          {
                            success
                          }
                        }",
                    Variables = new { id = id }
                };

                var response = await _client.SendMutationAsync<ProfileRestrictionsUpdateData>(query);
                var recipeData = response.Data;
                //var remove = RemoveMealPlan();
                //var genMeal=GenerateMealPlan();
                return recipeData;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        public async Task<LoginData> UserLogin(string id)
        {

            try
            {

                var query = new GraphQLRequest
                {
                    Query = @"
                mutation($id:String!) {
                          login(userId: $id) {
                            accessToken
                            refreshToken
                                        }
                        }",
                    Variables = new { id = id }
                };

                var response = await _client.SendMutationAsync<LoginData>(query);
                var recipeData = response.Data;

                return recipeData;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        public async Task<SkipMealData> SkipMeal(string token, SkipMealRequestModel model)
        {
            this._Jwtclient.HttpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));
            SkipMealData recipeData = new SkipMealData();
            try
            {
                foreach (var id in model.MealId)
                {
                    var query = new GraphQLRequest
                    {
                        Query = @"
                mutation($id:ID!) {
                          createMealEntry(mealId:$id value: SKIPPED)
                             {
                                success
                                message
                             }
                           }",
                        Variables = new { id = id }
                    };

                    var response = await _Jwtclient.SendMutationAsync<SkipMealData>(query);
                    recipeData = response.Data;
                }
                return recipeData;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        public async Task<SuggesticProgramsData> GetAllPrograms()
        {
            try
            {

                var query = new GraphQLRequest
                {
                    Query = @"
                query {
                      programs {
                        edges {
                          node {
                            id
                            databaseId
                            name
                            author
                            isActive
                            isPremium
                          }
                        }
                      }
                    }"

                };

                var response = await _client.SendMutationAsync<SuggesticProgramsData>(query);
                var recipeData = response.Data;

                return recipeData;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        public async Task<UpdateUserWithProgramData> UpdateUserWithProgram(String programId, string suggesticUserId)
        {
            _client.HttpClient.DefaultRequestHeaders.Add("sg-user", suggesticUserId);
            try
            {

                var query = new GraphQLRequest
                {
                    Query = @"
                mutation($id:String!) {
                          updateUserProgram(
                            programId:$id 
                            ) {
                                success
                                message
                            }
                          }",
                    Variables = new { id = programId }
                };

                var response = await _client.SendMutationAsync<UpdateUserWithProgramData>(query);

                return response.Data;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        public async Task<GenerateSimpleMealPlanData> GenerateSimpleMealPlan(int totalCalories, string suggesticUserId = null)
        {
            try
            {
                if (suggesticUserId != null)
                    _client.HttpClient.DefaultRequestHeaders.Add("sg-user", suggesticUserId);
                var query = new GraphQLRequest
                {
                    Query = @"
                    mutation($breakfastMin:Int, $breakfastMax:Int, $lunchMin:Int, $lunchMax:Int, $snackMin:Int, $snackMax:Int, $dinnerMin:Int, $dinnerMax:Int) {
                      generateSimpleMealPlan(
                        filters: {
                          kcalRange: {
                            breakfast: { min: $breakfastMin, max: $breakfastMax }
                            lunch: { min: $lunchMin, max: $lunchMax }
                            snack: { min: $snackMin, max: $snackMax }
                            dinner: { min: $dinnerMin, max: $dinnerMax }
                          }
                        }
                      ) {
                        success
                        message
                      }
                    }",
                    Variables = new
                    {
                        breakfastMin = CalculatePercentageValueOfTotal(totalCalories, 15),
                        breakfastMax = CalculatePercentageValueOfTotal(totalCalories, 30),
                        lunchMin = CalculatePercentageValueOfTotal(totalCalories, 25),
                        snackMin = CalculatePercentageValueOfTotal(totalCalories, 5),
                        snackMax = CalculatePercentageValueOfTotal(totalCalories, 20),
                        dinnerMin = CalculatePercentageValueOfTotal(totalCalories, 25),
                        dinnerMax = CalculatePercentageValueOfTotal(totalCalories, 40),
                    }
                };

                var response = await _client.SendMutationAsync<GenerateSimpleMealPlanData>(query);
                var recipeData = response.Data;

                return recipeData;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        public async Task<AddToShoppingListData> GetDataForShoppingList(MealPlanData data)
        {
            try
            {
                var recipeIds = ConvertRecepiIdsToArray(data);
                var response = await AddToShoppingList(recipeIds);

                return response != null ? response : new AddToShoppingListData();
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        public async Task<AddToShoppingListData> AddToShoppingList(string[] id)
        {
            //_client.HttpClient.DefaultRequestHeaders.Add("sg-user", suggesticUserId)
            try
            {
                AddToShoppingListData data = new AddToShoppingListData();
                for (int i = 0; i < id.Length; i = i + 10)
                {
                    List<string> newIdArr = new List<string>();
                    for (int j = i; j < i + 10; j++)
                    {
                        if (j < id.Length)
                        {
                            newIdArr.Add(id[j]);
                        }
                    }


                    var query = new GraphQLRequest
                    {
                        Query = @"
                mutation($id:[String]!) {
                          addRecipesToShoppingList(recipeIds: $id) {
                            message
                            success
                          }
                        
                        }",
                        Variables = new { id = newIdArr }
                    };

                    var response = await _client.SendMutationAsync<AddToShoppingListData>(query);
                    data = response.Data;
                }


                return data;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        public async Task<RemoveUserData> RemoveUser()
        {
            try
            {

                var query = new GraphQLRequest
                {
                    Query = @"
                mutation {
                    deleteMyProfile {
                    success
                    }
                }"
                };

                var response = await _client.SendMutationAsync<RemoveUserData>(query);
                var recipeData = response.Data;

                return recipeData;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        private int CalculatePercentageValueOfTotal(long total, int percent)
        {
            return Convert.ToInt32(percent * 0.01 * total);
        }

        public async Task<ShoppingListData> GetShoppingListById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (!string.IsNullOrEmpty(user?.SuggesticId))
            {
                try
                {
                    _client.HttpClient.DefaultRequestHeaders.Add("sg-user", user.SuggesticId);

                    var query = new GraphQLRequest
                    {
                        Query = @"
                query {
                        shoppingListAggregate {
                            edges {
                                node {
                                databaseId
                                ingredient        
                                aisleName
                                quantity
        
                                unit
                                grams
                                isDone
                                }
                            }
                            }
                        }"

                    };

                    var response = await _client.SendQueryAsync<ShoppingListData>(query);
                    var recipeData = response.Data;

                    return recipeData;
                }
                catch (Exception exp)
                {

                    return new ShoppingListData();
                }
            }
            throw new Exception("User is not registered to suggestic service. Please register first.");
        }

        private async Task<AddToShoppingListData> RemoveItemsFromShoppingList(string[] id)
        {
            //_client.HttpClient.DefaultRequestHeaders.Add("sg-user", suggesticUserId)
            try
            {
                AddToShoppingListData data = new AddToShoppingListData();
                for (int i = 0; i < id.Length; i = i + 10)
                {
                    List<string> newIdArr = new List<string>();
                    for (int j = i; j < i + 10; j++)
                    {
                        if (j < id.Length)
                        {
                            newIdArr.Add(id[j]);
                        }
                    }


                    var query = new GraphQLRequest
                    {
                        Query = @"
                mutation($id:[String]!) {
                          removeFromShoppingList(recipeIds: $id) {
                            success
                          }
                        
                        }",
                        Variables = new { id = newIdArr }
                    };

                    var response = await _client.SendMutationAsync<AddToShoppingListData>(query);
                    data = response.Data;
                }


                return data;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        private string[] ConvertRecepiIdsToArray(MealPlanData data)
        {
            
            List<string> recipeIds = new List<string>();
            if (data.mealPlan.Any())
            {
                foreach (var plan in data.mealPlan)
                {
                    foreach (var meal in plan.meals)
                    {
                        recipeIds.Add(meal.recipe.databaseId);
                    }
                }
            }

            return  recipeIds?.ToArray();
        }


        public async Task<MealTrackerData> GetUserSkippedMeals(string token, SkipMealRequest model)
        {
            this._Jwtclient.HttpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));
            try
            {

                var query = new GraphQLRequest
                {
                    Query = @"
                 query($startDate:Date! ,$endDate:Date!) {
                mealTracker(startDate:$startDate,  endDate:$endDate ) 
                        {
                        mealId
                        meal{
                              id
                          }
                        value
                        date
                        }
                      }",
                    Variables = new { startDate = model.StartDate, endDate = model.EndDate }
                };

                var response = await _Jwtclient.SendQueryAsync<MealTrackerData>(query);
                var recipeData = response.Data;

                return recipeData;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        public async Task<ProfileSettingData> UpdateUserProfileSettings(string suggesticUserId, string dietId, decimal calories)
        {


            try
            {
                GymEats.Data.Entity.Diet diet = await _dietService.GetById(Guid.Parse(dietId));
                var format = createFormat(diet.MealSchedule);
                _client.HttpClient.DefaultRequestHeaders.Add("sg-user", suggesticUserId);
                var query = new GraphQLRequest
                {
                    Query = @"
                mutation($calories:Int!,$carbs:Float!,$protein:Float!,$fat:Float!,$format:[MealTime]) {
                         profileMealPlanSettings (
                        calories: $calories
                        carbsPerc: $carbs
                        proteinPerc: $protein
                        fatPerc: $fat
                       
	                        format:$format 
	                        error: 0.05
                        ) {
                            success
                        }
                        }",
                    Variables = new
                    {
                        calories = Convert.ToInt32(calories),
                        carbs = (float)diet.CarbsPercentage / 100,
                        protein = (float)diet.ProteinPercentage / 100,
                        fat = (float)diet.FatPercentage / 100,
                        format = format
                    }
                };

                var response = await _client.SendMutationAsync<ProfileSettingData>(query);
                var responseData = response.Data;
                //if (responseData.profileMealPlanSettings.success == true)
                //{
                //    var mealplan=await GenerateMealPlan();
                //    if (mealplan.generateMealPlan.success == false){
                //        throw new Exception(ErrorMessage.GenrateError);
                //    }
                //}
                return responseData;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        public string[] createFormat(string meals)
        {
            List<string> format = new List<string>();
            var mealstime = meals.Split(",");
            foreach (var mealtime in mealstime)
            {
                switch (mealtime)
                {
                    case "0":
                        {
                            format.Add("SNACK");
                            break;
                        }
                    case "1":
                        {
                            format.Add("BREAKFAST");
                            break;
                        }
                    case "2":
                        {
                            format.Add("LUNCH");
                            break;
                        }
                    case "3":
                        {
                            format.Add("DINNER");
                            break;
                        }
                    default: break;
                }
            }
            return format.ToArray();
        }

        public async Task<UserRestrictionData> GetUserRestrictions(string suggesticUserId)
        {
            _client.HttpClient.DefaultRequestHeaders.Add("sg-user", suggesticUserId);
            try
            {

                var query = new GraphQLRequest
                {
                    Query = @"
                query {
                          myProfile {
                            programName
                            debugMealPlanVariables {
                              restrictions
                            }
                            restrictions{
                              id
                              name
                              isOnProgram
                              subcategory
                              slugname 
                            }
                          }
                        }"

                };

                var response = await _client.SendMutationAsync<UserRestrictionData>(query);
                var recipeData = response.Data;

                return recipeData;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        public async Task<RemoveMealPlanData> RemoveMealPlan()
        {
            try
            {

                var query = new GraphQLRequest
                {
                    Query = @"
                mutation {
  removeMealPlan {
    success
    message
  }
}"
                };

                var response = await _client.SendMutationAsync<RemoveMealPlanData>(query);
                var recipeData = response.Data;

                return recipeData;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        public async Task<MealTrackerData> GetRestaurentFood(string token, SkipMealRequest model)
        {
            this._Jwtclient.HttpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));
            try
            {

                var query = new GraphQLRequest
                {
                    Query = @"
                 query($startDate:Date! ,$endDate:Date!) {
                mealTracker(startDate:$startDate,  endDate:$endDate ) 
                        {
                        mealId
                        meal{
                              id
                          }
                        value
                        date
                        }
                      }",
                    Variables = new { startDate = model.StartDate, endDate = model.EndDate }
                };

                var response = await _Jwtclient.SendQueryAsync<MealTrackerData>(query);
                var recipeData = response.Data;

                return recipeData;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        public async Task<RestaurantData> RestuarantSearch(double lat, double lon, string suggesticUserId, string userId)
        {
            _client.HttpClient.DefaultRequestHeaders.Add("sg-user", suggesticUserId);
            try
            {

                var query = new GraphQLRequest
                {
                    Query = @"
                query recipeQuery($lat:Float,$lon:Float){
                  restaurantSearch(
                    distance: 30
                    lat:$lat
                    lon:$lon
                    isOpen:true
                     )
                  {
                   totalCount
                edges
                    {
                      node{
                        name
                        shortName
        
                        isRecomended
                        address1
                        cityTown
                        country
                        recommendation
                        recommendationsCount
                      }
                    }
                }
                }",
                    Variables = new { lat = lat, lon = lon }

                };

                var response = await _client.SendQueryAsync<RestaurantData>(query);
                var recipeData = response.Data;

                return recipeData;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        public async Task<ShoppingListData> GetShoppingList(string suggesticUserId)
        {
            _client.HttpClient.DefaultRequestHeaders.Add("sg-user", suggesticUserId);
            try
            {

                var query = new GraphQLRequest
                {
                    Query = @"
                query {
                        shoppingListAggregate {
                            edges {
                                node {
                                databaseId
                                ingredient        
                                aisleName
                                quantity
        
                                unit
                                grams
                                isDone
                                }
                            }
                            }
                        }"

                };

                var response = await _client.SendQueryAsync<ShoppingListData>(query);
                var recipeData = response.Data;

                return recipeData;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        public async Task<SkipMealData> UnSkipSkippedMeal(string token, SkipMealRequestModel model)
        {
            if (model.MealId.Count == 0)
            {
                this._Jwtclient.HttpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));
            }
            SkipMealData recipeData = new SkipMealData();
            try
            {
                foreach (var id in model.UnSkippedMealId)
                {
                    var query = new GraphQLRequest
                    {
                        Query = @"
                mutation($id:ID!) {
                          createMealEntry(mealId:$id value: DELETE)
                             {
                                success
                                message
                             }
                           }",
                        Variables = new { id = id }
                    };

                    var response = await _Jwtclient.SendMutationAsync<SkipMealData>(query);
                    recipeData = response.Data;
                }
                return recipeData;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

    }
}
