using GymEats.Common.Model;
using GymEats.Services.Blob;
using GymEats.Services.Survey;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GymEats.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SurveyController : BaseController
    {
        private readonly ISurveyService _surveyService;
        private readonly IBlobService _blobService;
        public string containerName = "survey";

        public SurveyController(ISurveyService surveyService, IBlobService blobService)
        {
            _surveyService = surveyService;
            _blobService = blobService;
        }

        [HttpGet]
        [Route("GetSurvey")]
        public async Task<IActionResult> GetSurvey()
        {
            var survey = await _surveyService.GetSurvey();
            if (survey == null)
            {
                return BadRequest(new
                {
                    Success = false,
                    ErrorMessage = "Record not found."
                }); 
            }
            return Ok(new
            {
                Success = true,
                Data = survey
            });
        }

        [HttpPost("AddSurvey")]
        public async Task<IActionResult> AddSurvey([FromBody] SurveyQuestionRequest model)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                model.CreatedBy = GetUserId();
                var json = JsonConvert.SerializeObject(model);
                model.CreatedBy = GetUserId();
                SurveyViewModel survey  = new SurveyViewModel();
                survey.PrimaryQuestion = Guid.Parse(model.QuestionId);
                survey.SurveyJson = await _blobService.UploadJsonAsync(json, containerName);
                survey.CreatedBy = GetUserId();
                var data = await _surveyService.AddSurvey(survey);
                if(data != null)
                {
                    response.Success = true;
                    response.Message = "Succefully survey added.";
                    response.Data = data;
                    return Ok(response);
                }
                response.Success = false;
                response.ErrorMessage = "Failed to add survey. please try again.";
                return BadRequest(response);
            }catch (Exception ex) 
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }
    }
}
