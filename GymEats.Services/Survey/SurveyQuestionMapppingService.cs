using AutoMapper;
using GymEats.Common.Constants;
using GymEats.Data;
using GymEats.Data.Entity;
using GymEats.Services.Option;
using GymEats.Services.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Survey
{
    public class SurveyQuestionMapppingService
    {
        private readonly IGenericRepository<SurveyQuestionMapping> _surveyQuestionMappingRepository;

        public SurveyQuestionMapppingService(IGenericRepository<SurveyQuestionMapping> surveyQuestionMappingRepository)
        {
            _surveyQuestionMappingRepository = surveyQuestionMappingRepository;
        }

        public async Task<SurveyQuestionMapping> AddSuerveyQuestion(SurveyQuestionMapping model)
        {
            try
            {
                model.Id = Guid.NewGuid();

                await _surveyQuestionMappingRepository.InsertAsync(model);
                var res = await _surveyQuestionMappingRepository.SaveAsync();
                if((int)res > 0)
                    return (model);
                throw new Exception(ErrorMessage.AddToDb);
            }
            catch (Exception ex)
            {

                throw new Exception(ErrorMessage.AddToDb);
            }
        }
    }
}
