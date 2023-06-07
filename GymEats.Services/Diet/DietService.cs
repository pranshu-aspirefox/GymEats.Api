using AutoMapper;
using GymEats.Common.Constants;
using GymEats.Common.Model;
using GymEats.Data;
using GymEats.Data.Entity;
using GymEats.Services.Repository.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Diet
{
    public class DietService : IDietService
    {
        private readonly IGenericRepository<Data.Entity.Diet> _dietRepository;
        private readonly IMapper _mapper;

        public DietService(IGenericRepository<GymEats.Data.Entity.Diet> dietRepository, IMapper mapper)
        {
            _dietRepository = dietRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DietViewModel>> DietList()
        {
            var dietList = (await _dietRepository.GetAsync(x => x.IsDeleted == false)).Include(x => x.UserCreatedBy).ToList();

            var dietViewList = _mapper.Map<IList<GymEats.Data.Entity.Diet>, IList<DietViewModel>>(dietList);
            return dietViewList;
        }

        public async Task<GymEats.Data.Entity.Diet> GetById(Guid id)
        {
            var diet = (await _dietRepository.GetAsync(x =>x.IsDeleted == false)).FirstOrDefault(obj => obj.Id == id);
            return diet;
        }

        public async Task<DietViewModel> GetDietViewModelById(Guid id)
        {
            var diet =await GetById(id);
            return _mapper.Map<DietViewModel>(diet);
        }

        public async Task<DietViewModel> GetDefaultDiet()
        {
            var diet = (await _dietRepository.GetAsync(x => x.IsDefault == true)).FirstOrDefault();
            return _mapper.Map<DietViewModel>(diet);
        }

        public async Task<DietViewModel> AddNewDiet(DietRequestModel model)
        {
            try
            {
                var valid = CheckTotalVAlue(model); //checkValid
                if (valid)
                {

                    var defaultdiet = (await _dietRepository.GetAsync(x => x.IsDefault == true)).FirstOrDefault();
                    if (defaultdiet != null)
                    {
                        defaultdiet.IsDefault = false;
                        await _dietRepository.UpdateAsync(defaultdiet);
                        await _dietRepository.SaveAsync();
                    }
                    else
                    {
                        model.IsDefault = true;
                    }

                    GymEats.Data.Entity.Diet diet = _mapper.Map<DietRequestModel, GymEats.Data.Entity.Diet>(model);
                    diet.Id = Guid.NewGuid();
                    diet.CreatedOn = DateTime.UtcNow;
                    diet.CreatedBy = model.CreatedBy;
                    diet.IsActive = true;
                    diet.IsDeleted = false;
                    await _dietRepository.InsertAsync(diet);
                    var result = await _dietRepository.SaveAsync();
                    if((int)result > 0)
                        return _mapper.Map<GymEats.Data.Entity.Diet, DietViewModel>(diet);
                }
                throw new Exception(ErrorMessage.AddToDb);
            }
            catch (Exception exp)
            {

                throw new Exception(ErrorMessage.AddToDb);
            }
        }

        public async Task<DietViewModel> UpdateDiet(DietViewModel model)
        {
            try
            {
                var diet =await GetById(model.Id);

                if (model.IsDefault == true && diet.IsDefault != true)
                {
                    var defaultdiet = (await _dietRepository.GetAsync(x => x.IsDefault == true)).FirstOrDefault();
                    if (defaultdiet != null)
                    {
                        defaultdiet.IsDefault = false;
                        await _dietRepository.UpdateAsync(defaultdiet);
                    }
                }
                if(!string.IsNullOrEmpty(model.DietName))
                    diet.DietName = model.DietName;
                if(model.ProteinPercentage > 0)
                    diet.ProteinPercentage = model.ProteinPercentage;
                if(model.FatPercentage > 0)
                    diet.FatPercentage = model.FatPercentage;
                if(model.CarbsPercentage > 0)
                    diet.CarbsPercentage = model.CarbsPercentage;
                if(model.SurplusPercentage > 0)
                    diet.SurplusPercentage = model.SurplusPercentage;
                if(model.DeficitPercentage > 0)
                    diet.DeficitPercentage = model.DeficitPercentage;
                if(!string.IsNullOrEmpty(model.MealSchedule))
                    diet.MealSchedule = model.MealSchedule;
                diet.UpdatedBy = model.UpdatedBy;
                diet.UpdatedOn = DateTime.UtcNow;
                await _dietRepository.UpdateAsync(diet);
                var res = await _dietRepository.SaveAsync();
                if((int)res > 0)
                    return _mapper.Map<GymEats.Data.Entity.Diet, DietViewModel>(diet);
                return new DietViewModel();
            }
            catch (Exception exp)
            {

                throw new Exception(ErrorMessage.UpdateToDb);
            }
        }

        public async Task<DietViewModel> DeleteDiet(Guid id)
        {
            try
            {
                var diet = await GetById(id);
                diet.IsDeleted = true;
                await _dietRepository.UpdateAsync(diet);
                var res = await _dietRepository.SaveAsync();
                if((int)res > 0) 
                    return _mapper.Map<DietViewModel>(diet);
                throw new Exception(ErrorMessage.DeleteToDb);
            }
            catch (Exception exp)
            {

                throw new Exception(ErrorMessage.DeleteToDb);
            }
        }

        private bool CheckTotalVAlue(DietRequestModel model)
        {
            var sum = model.FatPercentage + model.ProteinPercentage + model.CarbsPercentage;
            if (sum > 100)
            {
                return false;
            }
            return true;
        }

    }
}
