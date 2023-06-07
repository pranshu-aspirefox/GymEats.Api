using GymEats.Common.Model;
using GymEats.Services.Option;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymEats.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class OptionController : BaseController
    {
        private readonly IOptionService _optionService;

        public OptionController(IOptionService optionService)
        {
            _optionService = optionService;
        }

        [HttpGet("GetOptionList")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOptionList()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var optionList = (await _optionService.OptionList()).ToList();
                if (optionList.Any())
                {
                    response.Success = true;
                    response.Data = optionList;
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

        [HttpPost("AddOption")]
        public async Task<IActionResult> AddOption([FromBody] OptionRequestModel model)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                model.CreatedBy = GetUserId();
                if(ModelState.IsValid)
                {
                    var result = await _optionService.AddNewOption(model);
                    if(result != null)
                    {
                        response.Success = true;
                        response.Data = result;
                        return Ok(response);
                    }
                }
                response.Success = false;
                response.ErrorMessage = "Failed to add option.";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpGet("GetOptionById/{id}")]
        public async Task<IActionResult> GetOptionById(Guid id)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var option = await _optionService.GetViewModelById(id);
                if(option != null)
                {
                    response.Success = true;
                    response.Data = option;
                    return Ok(response);
                }
                response.Success = false;
                response.ErrorMessage = "No option found.";
                return BadRequest(response);
            }catch(Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpPut("UpdateOption")]
        public async Task<IActionResult> UpdateOption([FromBody] OptionViewModel model)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var result = await _optionService.UpdateOption(model);
                if(result != null)
                {
                    response.Success = true;
                    response.Data = result;
                    return Ok(response);
                }
                response.Success = false;
                response.ErrorMessage = "Failed to update option.";
                return BadRequest(response);
            }catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpDelete("DeleteOption/{id}")]
        public async Task<IActionResult> DeleteOption(Guid id)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var result = await _optionService.DeleteOption(id);
                if( result != null)
                {
                    response.Success = true;
                    response.Message = "Successfully deleted.";
                    response.Data = result;
                    return Ok(response);
                }
                response.Success = false;
                response.ErrorMessage = "Failed to delete.";
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }
    }
}
