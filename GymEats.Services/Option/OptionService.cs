using AutoMapper;
using GymEats.Common.Constants;
using GymEats.Common.Model;
using GymEats.Data;
using GymEats.Data.Entity;
using GymEats.Services.Repository.Common;
using Microsoft.EntityFrameworkCore;

namespace GymEats.Services.Option
{
    public class OptionService : IOptionService
    {
        private readonly IGenericRepository<Data.Entity.Option> _optionRepository;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<SurveyOption> _surveyOptionRepository;

        public OptionService(IGenericRepository<GymEats.Data.Entity.Option> optionRepository, IMapper mapper, IGenericRepository<GymEats.Data.Entity.SurveyOption> surveyOptionRepository)
        {
            _optionRepository = optionRepository;
            _mapper = mapper;
            _surveyOptionRepository = surveyOptionRepository;
        }

        public async Task<IEnumerable<OptionViewModel>> OptionList()
        {
            var data = await _optionRepository.GetAsync(x => x.IsDeleted == false);
            var optionList = data.Include(x => x.UserCreatedBy).ToList();   
            var questionViewList = _mapper.Map<IList<GymEats.Data.Entity.Option>, IList<OptionViewModel>>(optionList);
            return questionViewList;
        }

        public async Task<GymEats.Data.Entity.Option> GetById(Guid id)
        {
            var option = await _optionRepository.GetByIdAsync(id);
            return option;
        }

        public async Task<OptionViewModel> GetViewModelById(Guid id)
        {
            var option = await GetById(id);
            return _mapper.Map<OptionViewModel>(option);
        }

        public async Task<OptionViewModel> AddNewOption(OptionRequestModel model)
        {
            try
            {
                var option = new GymEats.Data.Entity.Option() {
                    Id = Guid.NewGuid(),
                    Label = model.Label,
                    IsExclusive = model.IsExclusive,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = model.CreatedBy
                };
                await _optionRepository.InsertAsync(option);
                var result = await _optionRepository.SaveAsync();
                if((int)result > 0)
                    return _mapper.Map<GymEats.Data.Entity.Option, OptionViewModel>(option);
                throw new Exception(ErrorMessage.AddToDb);
            }
            catch (Exception ex) 
            {
                throw new Exception(ErrorMessage.AddToDb);
            }
        }

        public async Task<OptionViewModel> DeleteOption(Guid id)
        {
            try
            {
                var option = await _optionRepository.GetByIdAsync(id);
                if (option != null)
                {
                    await _optionRepository.DaleteAsync(id);
                    var result = await _optionRepository.SaveAsync();
                    if((int)result > 0)
                        return _mapper.Map<GymEats.Data.Entity.Option, OptionViewModel>(option);
                }
                throw new Exception("Failed to delete. Invalid Id.");
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorMessage.DeleteToDb);
            }
        }

        public async Task<OptionViewModel> UpdateOption(OptionViewModel model)
        {
            try
            {
                var option = await _optionRepository.GetByIdAsync(model.Id);
                if (option != null)
                {
                    if(!string.IsNullOrEmpty(model.Label))
                        option.Label = model.Label;
                    option.IsExclusive = model.IsExclusive;
                    option.UpdatedOn = DateTime.UtcNow;
                    await _optionRepository.UpdateAsync(option);
                    var result = await _optionRepository.SaveAsync();
                    if((int)result > 0)
                        return _mapper.Map<GymEats.Data.Entity.Option, OptionViewModel>(option);
                }
                throw new Exception("Failed to update. Invalid Id.");
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorMessage.UpdateToDb);
            }
        }

        public async Task<IList<GymEats.Data.Entity.Option>> GetSurveyOptions(Guid id)
        {
            var optionData = (await _surveyOptionRepository.GetAsync(x=>x.SurveyQuestionId == id));
            var optionList = await _optionRepository.GetAsync(x => x.IsDeleted == false);
            var data = from c in optionList join pt in optionData on c.Id equals pt.OptionId select c;
            return data.ToList();
        }

    }
}
