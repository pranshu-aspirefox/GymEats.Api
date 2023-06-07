using GymEats.Common.Model;
using GymEats.Services.Auth;
using GymEats.Services.Service;
using GymEats.Services.Suggestic.HelperClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymEats.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SuggesticController : ControllerBase
    {
        private readonly SuggesticApiService _suggesticApiService;
        private readonly IAuthService _authService;

        public SuggesticController(SuggesticApiService suggesticApiService, IAuthService authService)
        {
            _suggesticApiService = suggesticApiService;
            _authService = authService;
        }

        [HttpGet]
        [Route("GenMealPlan/{userId}")]
        public async Task<IActionResult> GenerateMealPlan(string userId, int calorie)
        {
            try
            {
                var user = await _authService.GetUserById(userId);

                if(user != null)
                if (user.MealPlanEndDate == null )
                {
                    SuggesticUserData suggesticUser = await _suggesticApiService.CreateUser(user.Email, user.FirstName);
                    //var programs = await _suggesticApiService.GetAllPrograms();
                    //var dietProgram = programs.programs.edges.Find(prog => prog.node.name.ToLower().Trim() == diet.DietName.ToLower().Trim());
                    //if (dietProgram != null)
                    //    await suggesticApiService.UpdateUserWithProgram(dietProgram.node.id, suggesticUser.createUser.user?.databaseId);
                    var genMealplan = await _suggesticApiService.GenerateSimpleMealPlan(calorie); //todo: reqired calories from UserDetails
                    if (genMealplan.generateSimpleMealPlan == null)
                        genMealplan = await _suggesticApiService.GenerateSimpleMealPlan(calorie, suggesticUser?.createUser?.user?.databaseId);
                    var mealPlan = await _suggesticApiService.GetMeals();
                    var shoppingList = await _suggesticApiService.GetDataForShoppingList(mealPlan);
                    if (genMealplan.generateSimpleMealPlan.success == false)
                    {
                        await _suggesticApiService.RemoveUser();
                    }

                    //UserDetailViewModel userDetailsModel = new UserDetailViewModel();
                    //userDetailsModel.SuggesticId = suggesticUser.createUser.user.databaseId;
                    //userDetailsModel.UserId = id;
                    //await userDetailService.UpdateUserSuggesticId(userDetailsModel);

                    //update SuggesticId of User
                    user.MealPlanEndDate = mealPlan.mealPlan[0].date;
                    user.SuggesticId = suggesticUser.createUser.user.databaseId;
                    var res = await _authService.UpdateUserData(user);
                    if (res)
                        return Ok(new
                        {
                            Success = true,
                            Data = mealPlan.mealPlan
                        });

                }
                return BadRequest(new
                {
                    Succcess = false,
                    ErrorMessage = "Failed to generate meal plan."
                });


            }
            catch (Exception exp)
            {
                throw;
            }
        }


        [HttpGet("GetRecipeDetailById/{userId}")]
        public async Task<IActionResult> GetRecipeDetailById(string userId, string recipeId)
        {
            var response = new ApiResponse();
            try
            {
                var user = await _authService.GetUserById(userId);
                if (user.SuggesticId != null)
                {
                    var result = await _suggesticApiService.GetRecipeById(recipeId, user.SuggesticId);
                    if (result != null)
                    {
                        response.Success = true;
                        response.Data = result;
                        return Ok(response);
                    }
                    else
                    {
                        response.Success = false;
                        response.ErrorMessage = "Invalid recipe.";
                        return BadRequest(response);
                    }
                }
                else
                {
                    response.Success = false;
                    response.ErrorMessage = "User is not registered to suggestic service. please regiser first.";
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }


        [HttpGet("GetMealPlan/{userId}")]
        public async Task<IActionResult> GetMealPlan(string userId, int requiredCallories)
        {
            var response = new ApiResponse();
            try
            {
                var user = await _authService.GetUserById(userId);
                if (user?.SuggesticId != null)
                {
                    var res = await _suggesticApiService.GenerateSimpleMealPlan(requiredCallories, user.SuggesticId);

                    var data = await _suggesticApiService.GetMeals();
                    if (data.mealPlan.Any())
                    {
                        user.MealPlanEndDate = data.mealPlan[0].date;
                        await _authService.UpdateUserData(user);
                        response.Success = true;
                        response.Data = data.mealPlan;
                        return Ok(response);
                    }
                    else
                    {
                        response.Success = false;
                        response.ErrorMessage = "Failed to generate meal plan. Please try again.";
                        return BadRequest(response);
                    }


                }
                else
                {
                    response.Success = false;
                    response.ErrorMessage = "User is not registered to suggestic service. Please register first.";
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }


        [HttpPost("AddToShoppingList/{userId}")]
        public async Task<IActionResult> AddToShoppingList(string userId, AddShoppingListRequest shoppingListRequest)
        {
            var response = new ApiResponse();
            try
            {
                var user = await _authService.GetUserById(userId);
                if (user.SuggesticId != null)
                {
                    var res = await _suggesticApiService.AddToShoppingList(shoppingListRequest.RecipeIds);
                    if (res.addRecipesToShoppingList.success)
                    {
                        response.Success = true;
                        response.Message = res.addRecipesToShoppingList.message;
                        return Ok(response);
                    }
                    else
                    {
                        response.Success = false;
                        response.ErrorMessage = "Failed to add to shopping list. please try again.";
                        return BadRequest(response);
                    }
                }
                else
                {
                    response.Success = false;
                    response.ErrorMessage = "User is not registered to suggestic service. Please register first.";
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpGet]
        [Route("GetShoppingListById/{userId}")]
        public async Task<IActionResult> GetShoppingListById(string userId)
        {
            try
            {
                var result = await _suggesticApiService.GetShoppingListById(userId);
                if (result.shoppingListAggregate != null)
                {
                    return Ok(new
                    {
                        Success = true,
                        Data = result
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "No data found."
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}
