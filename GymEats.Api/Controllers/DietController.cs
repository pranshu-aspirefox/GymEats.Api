using GymEats.Common.Constants;
using GymEats.Common.Model;
using GymEats.Services.Diet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymEats.Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class DietController : BaseController
    {
        private readonly IDietService _dietService;

        public DietController(IDietService dietService)
        {
            _dietService = dietService;
        }

        [HttpGet("GetDietList")]
        public async Task<IActionResult> GetDietList()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _dietService.DietList();
                if(data.Any())
                {
                    response.Success = true;
                    response.Data = data;
                    return Ok(response);
                }
                response.Success = false;
                response.ErrorMessage = "No record found.";
                return BadRequest(response);
            }
            catch (Exception ex) 
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpPost("AddNewDiet")]
        public async Task<IActionResult> AddNewDiet(DietRequestModel model)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                model.CreatedBy = GetUserId();
                var data = await _dietService.AddNewDiet(model);
                if(data != null)
                {
                    response.Success = true;
                    response.Data = data;
                    response.Message = "Sucessfully added.";
                    return Ok(response);
                }
                response.Success = false;
                response.ErrorMessage = ErrorMessage.AddToDb;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpGet("GetDietById/{id}")]
        public async Task<IActionResult> GetDietById(Guid id)
        {
            ApiResponse response = new ApiResponse();
            try 
            {
                var data = await _dietService.GetDietViewModelById(id);
                if(data != null)
                {
                    response.Success = true;
                    response.Data = data;
                    return Ok(response);
                }
                response.Success = false;
                response.ErrorMessage = "No record found.";
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpPut("UpdateDiet")]
        public async Task<IActionResult> UpdateDiet(DietViewModel model)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                model.UpdatedBy = GetUserId();
                var data = await _dietService.UpdateDiet(model);
                if (data != null)
                {
                    response.Success = true;
                    response.Data = data;
                    response.Message = "Diet successfully udated.";
                    return Ok(response);
                }
                response.Success = false;
                response.ErrorMessage = "No record found.";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpDelete("RemoveDiet/{id}")]
        public async Task<IActionResult> RemoveDiet(Guid id)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _dietService.DeleteDiet(id);
                if (data != null)
                {
                    response.Success = true;
                    response.Data = data;
                    response.Message = "Successfully deit deleted.";
                    return Ok(response);
                }
                response.Success = false;
                response.ErrorMessage = "Failed to delete diet.";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpGet("GetDefaultDiet")]
        public async Task<IActionResult> GetDefaultDiet()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _dietService.GetDefaultDiet();
                if (data != null)
                {
                    response.Success = true;
                    response.Data = data;
                    return Ok(response);
                }
                response.Success = false;
                response.ErrorMessage = "No diet found.";
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
