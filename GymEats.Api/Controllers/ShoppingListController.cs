using GymEats.Common.Model;
using GymEats.Services.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymEats.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class ShoppingListController : ControllerBase
    {
        private readonly IUserShoppingService _userCartService;

        public ShoppingListController(IUserShoppingService userCartService)
        {
            _userCartService = userCartService;
        }

        [HttpGet("GetAllItemFromShoppingList")]
        public async Task<IActionResult> GetAllItemFromShoppingList(string userId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _userCartService.GetAllListtItems(userId);
                if(data == null)
                {
                    response.Success = false;
                    response.ErrorMessage = "No item found.";
                    return BadRequest(response);
                }
                response.Success = true;
                response.Data = data;
                return Ok(response);
            }catch (Exception ex) 
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
            
        }

        [HttpPost("AddItemShoppingList")]
        public async Task<IActionResult> AddItemShoppingList(UserCartRequestModel model)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _userCartService.AddItemToShoppingList(model);
                if (data <= 0)
                {
                    response.Success = false;
                    response.ErrorMessage = "Failed to add item. Please try agian.";
                    return BadRequest(response);
                }
                response.Success = true;
                response.Message = "Item successfully added to cart.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpGet("CalcualteTotalPrice")]
        public async Task<IActionResult> CalcualteTotalPrice(string userId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _userCartService.GetTotalPriceOfItem(userId);
                if (data <= 0)
                {
                    response.Success = false;
                    response.ErrorMessage = "No item found of this user.";
                    return BadRequest(response);
                }
                response.Success = true;
                response.Data = new { TotalPrice = data};
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpPost("UpdateQuantityOfItem")]
        public async Task<IActionResult> UpdateQuantityOfItem(int quantity, string productId, string userId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _userCartService.UpdateItemToShoppingList(quantity, productId, userId);
                if (data <= 0)
                {
                    response.Success = false;
                    response.ErrorMessage = "Failed to update item. Please try agian.";
                    return BadRequest(response);
                }
                response.Success = true;
                response.Message = "Successfully updated item data.";
                return Ok(response);
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
