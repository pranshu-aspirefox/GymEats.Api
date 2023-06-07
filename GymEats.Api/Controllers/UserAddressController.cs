using GymEats.Common.Constants;
using GymEats.Common.Model;
using GymEats.Services.UserAddress;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymEats.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class UserAddressController : BaseController
    {
        private readonly IUserAddressService _userAddressService;

        public UserAddressController(IUserAddressService userAddressService)
        {
            _userAddressService = userAddressService;
        }

        [HttpGet("GetUserAddressById/{addressId}")]
        public async Task<IActionResult> GetUserAddress(int addressId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _userAddressService.GetAddressById(addressId);
                if(data != null)
                {
                    response.Success = true;
                    response.Data = data;
                    return Ok(response);
                }
                response.Success = false;
                response.ErrorMessage = "No address found.";
                return BadRequest(response);
            }catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpGet("GetAllUserAddress")]
        public async Task<IActionResult> GetUserAddress()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                string userId = GetUserId();
                var data = await _userAddressService.GetAllAddressByUserId(userId);
                if (data != null)
                {
                    response.Success = true;
                    response.Data = data;
                    return Ok(response);
                }
                response.Success = false;
                response.ErrorMessage = "No address found.";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpPost("AddNewAddress")]
        public async Task<IActionResult> AddNewAddress(UserAddressRequest model)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                model.UserId = GetUserId();
                var result = await _userAddressService.AddNewAddress(model);
                if(result != null)
                {
                    response.Success = true;
                    response.Data = result;
                    return Ok(response);
                }
                response.Success = false;
                response.ErrorMessage = "Failed to add address. Please try again.";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpPut("UpdateAddress")]
        public async Task<IActionResult> UpdateAddress(UserAddressRequest model, int addressId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var result = await _userAddressService.UpdateAddress(model, addressId);
                if(result != null)
                {
                    response.Success = true;
                    response.Data = result;
                    response.Message = "Successfully added..";
                    return Ok(response);
                }
                response.Success = false;
                response.ErrorMessage = ErrorMessage.UpdateToDb;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpDelete("RemoveAddress")]
        public async Task<IActionResult> RemoveAddress(int addressId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var result = await _userAddressService.RemoveAddress(addressId);
                if (result)
                {
                    response.Success = true;
                    response.Message = "Successfully removed address.";
                    return Ok(response);
                }
                response.Success = false;
                response.ErrorMessage = "Failed to remove the address.";
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
