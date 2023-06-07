using GymEats.Common.Model;

namespace GymEats.Services.Survey
{
    public interface ISurveyService
    {
        Task<SurveyViewModel> AddSurvey(SurveyViewModel model);
        Task<SurveyQuestionRequest> GetSurvey();
    }
}
