using GymEats.Common.Model;
using GymEats.Data.Entity;

namespace GymEats.Services.Diet
{
    public interface IDietService
    {
        Task<IEnumerable<DietViewModel>> DietList();
        Task<GymEats.Data.Entity.Diet> GetById(Guid id);
        Task<DietViewModel> GetDietViewModelById(Guid id);
        Task<DietViewModel> GetDefaultDiet();
        Task<DietViewModel> AddNewDiet(DietRequestModel model);
        Task<DietViewModel> UpdateDiet(DietViewModel model);
        Task<DietViewModel> DeleteDiet(Guid id);
    }
}