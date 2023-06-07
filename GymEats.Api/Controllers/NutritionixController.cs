using GymEats.Common.Model;
using GymEats.Services.Nutritionix;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymEats.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class NutritionixController : ControllerBase
    {
        private readonly INutritionixService _nutritionixService;

        public NutritionixController(INutritionixService nutritionixService)
        {
            _nutritionixService = nutritionixService;
        }

        [HttpGet("Serch-NXMeal-byName")]
        public async Task<IActionResult> SearchMealOnNXByName(string name)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _nutritionixService.GetNutritionixItemByName(name);
                if(data == null)
                {
                    response.Success = false;
                    response.ErrorMessage = "No meal found.";
                    return BadRequest(response);
                }
                response.Success = true;
                response.Data = data;
                return Ok(response);
            }
            catch (Exception ex) 
            {
                response.Success = false;
                response.ErrorMessage = ex.Message; 
                return BadRequest(response);
            }
        }

        [HttpGet("Get-NXMealDetails-ById")]
        public async Task<IActionResult> GetNXMealDetailsById(string nixItemId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _nutritionixService.GetMealDetaisById(nixItemId);
                if(data == null)
                {
                    response.Success = false;
                    response.ErrorMessage = "No details found.";
                    return BadRequest(response);
                }
                response.Success = true;
                response.Data = data;
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }
        [HttpGet("Get-NxMealInfo-ByName")]
        public async Task<IActionResult> GetNxMealInfoByName(string name)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _nutritionixService.GetMealInfoByName(name);
                if(data != null)
                {
                    response.Success = true;
                    response.Data = data;
                    return Ok(response);
                }
                response.Success = false;
                response.ErrorMessage = "No match found on NX with this item.";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }
    }
}
