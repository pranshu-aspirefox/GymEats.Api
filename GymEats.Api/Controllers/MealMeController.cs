using GraphQL.Client.Http;
using GymEats.Common.Model;
using GymEats.Services.Auth;
using GymEats.Services.Mealme;
using GymEats.Services.Mealme.HelperClass;
using GymEats.Services.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymEats.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class MealMeController : ControllerBase
    {
        private readonly IMealmeService _mealmeService;
        private readonly IAuthService _authService;
        private readonly SuggesticApiService _suggesticApiService;
        private readonly GraphQLHttpClient _client;

        public MealMeController(IMealmeService mealmeService, IAuthService authService, SuggesticApiService suggesticApiService)
        {
            _mealmeService = mealmeService;
            _authService = authService;
            _suggesticApiService = suggesticApiService;
        }

       
        [HttpPost("product-search-byName")]
        public async Task<IActionResult> ProductSearchByName(ProductSearchRequest request)
        {
            ApiResponse response = new ApiResponse();
            var result = _mealmeService.ProductSearchByName(request);
            if(result != null)
            {
                response.Success = true;
                response.Data = result.Result;
                return Ok(response);
            }
            else
            {
                response.Success = false;
                response.ErrorMessage = "No result found.";
                return BadRequest(response);
            }
        }

        [HttpPost("GetFinalMealList")]
        public async Task<IActionResult> GetFinalMealList(MealmeItemRequest model)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _mealmeService.GetFinalMealList(model.MealName, model.Calorie, model.Protein, model.Fat, model.Carbs, model.Day, model.MealType);
                if(data != null)
                {
                    response.Success = true;
                    response.Data = data;
                    return Ok(response);
                }
                response.Success = false;
                response.ErrorMessage = "No meal found.";
                return BadRequest(response);
            }catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpPost("GetMenuList")]
        public async Task<IActionResult> GetMenuList(GetMenuListRequest model)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var user = await _authService.GetUserById(model.UserId);
                //_client?.HttpClient.DefaultRequestHeaders.Add("sg-user", user.SuggesticId);
                var suggesticMeals = await _suggesticApiService.GetUserMealPlan(user.SuggesticId);
                var mealListReq = new List<MealListRequest>();
                foreach (var suggestic in suggesticMeals.mealPlan)
                {
                    foreach(var meal in suggestic.meals)
                    {
                        var food = new MealListRequest
                        {
                            Day = suggestic.day,
                            MealName = meal.recipe.name,
                            Calorie = meal.calories,
                            Protein = meal.recipe.nutrientsPerServing.protein == null? 0: meal.recipe.nutrientsPerServing.protein,
                            Fat = meal.recipe.nutrientsPerServing.fat == null ? 0 : meal.recipe.nutrientsPerServing.fat,
                            Carbs = meal.recipe.nutrientsPerServing.carbs == null ? 0 : meal.recipe.nutrientsPerServing.carbs,
                            MealType = meal.meal,
                            RecipeId = meal.recipe.id,
                            Serving = meal.numOfServings,
                        };
                        mealListReq.Add(food);
                    }
                }

                var data = await _mealmeService.GetAllMealList(mealListReq);
                if (data != null)
                {
                    response.Success = true;
                    response.Data = data;
                    return Ok(response);
                }
                response.Success = false;
                response.ErrorMessage = "No meal found.";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }

        }

        [HttpGet("AddItemsToShoppingListFromSuggestic")]
        public async Task<IActionResult> AddItemsToShoppingListFromSuggestic(string userId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _mealmeService.AddItemsFromSuggesticToShoppingList(userId);
                if (data != null)
                {
                    response.Success = true;
                    response.Data = data;
                    return Ok(response);
                }
                response.Success = false;
                response.ErrorMessage = "No record found.";
                return BadRequest(response);
            }catch(Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }
    }
}
