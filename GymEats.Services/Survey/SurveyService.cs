using AutoMapper;
using GymEats.Common.Constants;
using GymEats.Common.Model;
using GymEats.Data;
using GymEats.Data.Entity;
using GymEats.Services.Blob;
using GymEats.Services.Diet;
using GymEats.Services.Option;
using GymEats.Services.Question;
using GymEats.Services.Repository.Common;
using Newtonsoft.Json;

namespace GymEats.Services.Survey
{
    public class SurveyService : ISurveyService
    {
        private readonly IGenericRepository<Data.Entity.Survey> _surveyRepository;
        private readonly IMapper _mapper;
        private readonly IBlobService _blobService;
        public string containerName = "survey";

        public SurveyService(IGenericRepository<GymEats.Data.Entity.Survey> surveyRepository, IMapper mapper, IBlobService blobService)
        {
            _surveyRepository = surveyRepository;
            _mapper = mapper;
            _blobService = blobService;
        }

        public async Task<SurveyViewModel> AddSurvey(SurveyViewModel model)
        {
            try
            {
                model.Id = Guid.NewGuid();
                model.Name = "Admin Survey";

                GymEats.Data.Entity.Survey survey = _mapper.Map<SurveyViewModel, Data.Entity.Survey>(model);
                survey.CreatedOn = DateTime.UtcNow;
                survey.IsDeleted = false;
                survey.DeletedOn = null;
                await _surveyRepository.InsertAsync(survey);
                var result = await _surveyRepository.SaveAsync();
                if ((int)result > 0)
                    return _mapper.Map<Data.Entity.Survey, SurveyViewModel>(survey);

                throw new Exception(ErrorMessage.AddToDb);
            }
            catch (Exception ex) 
            {
                throw new Exception(ErrorMessage.AddToDb);
            }
        }

        public async Task<SurveyQuestionRequest> GetSurvey()
        {
            var surveyQuestion = new SurveyQuestionRequest();
            var data = (await _surveyRepository.GetAsync(x => x.IsDeleted == false)).ToList();
            var survey = data.FirstOrDefault();
            survey.SurveyJson = await _blobService.DownloadJsonAsync(survey.SurveyJson, containerName);
            if (survey != null)
            {
                surveyQuestion = JsonConvert.DeserializeObject<SurveyQuestionRequest>(survey.SurveyJson);
            }

            return surveyQuestion;

        }

       
    }
}
