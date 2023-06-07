using GymEats.Common.Model;
using GymEats.Services.Question;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymEats.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class QuestionController : BaseController
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet("GetQuestionList")]
        [AllowAnonymous]
        public async Task<IActionResult> GetQuestionList()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var questionList = (await _questionService.QuestionList()).ToList();
                if (questionList.Any())
                {
                    response.Success = true;
                    response.Data = questionList;
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

        [HttpPost("AddQuestion")]
        public async Task<IActionResult> AddNewQuestion([FromBody] QuestionRequestModel model)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                model.CreatedBy = GetUserId();
                if (ModelState.IsValid)
                {
                    var result = await _questionService.AddNewQuestion(model);
                    if(result != null)
                    {
                        response.Success = true;
                        response.Data = result;
                        return Ok(response);
                    }
                }
                response.Success = false;
                response.ErrorMessage = "Failed to add question.";
                return BadRequest(response);
            }catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpGet("GetQuestionById/{id}")]
        public async Task<IActionResult> GetQuestionById(Guid id)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var question = await _questionService.GetViewModelById(id);
                if (question != null)
                {
                    response.Success = true;
                    response.Data = question;
                    return Ok(response);
                }
                response.Success = false;
                response.ErrorMessage = "No question found.";
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpPut("UpdateQuestion")]
        public async Task<IActionResult> UpdateQuestion([FromBody] UpdateQuestionRequest model)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                model.UpdatedBy = GetUserId();
                if (ModelState.IsValid)
                {
                    var result = await _questionService.UpdateQuestion(model);
                    if (result != null)
                    {
                        response.Success = true;
                        response.Data = result;
                        return Ok(response);
                    }
                }
                response.Success = false;
                response.ErrorMessage = "Failed to update question. Please try again.";
                return BadRequest(response);
            }catch(Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpDelete("DeleteQuestion/{id}")]
        public async Task<IActionResult> DeleteQuestion(Guid id)
        {
            var response = new ApiResponse();
            try
            {
                var result = await _questionService.DeleteQuestion(id);
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Successfully deleted.";
                    response.Data = result;
                    return Ok(response);
                }
                response.Success = false;
                response.ErrorMessage = "Failed to delete question.";
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
