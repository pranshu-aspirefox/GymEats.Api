using GymEats.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Option
{
    public interface IOptionService
    {
        Task<IEnumerable<OptionViewModel>> OptionList();
        Task<GymEats.Data.Entity.Option> GetById(Guid id);
        Task<OptionViewModel> GetViewModelById(Guid id);
        Task<OptionViewModel> AddNewOption(OptionRequestModel model);
        Task<OptionViewModel> DeleteOption(Guid id);
        Task<OptionViewModel> UpdateOption(OptionViewModel model);
        Task<IList<GymEats.Data.Entity.Option>> GetSurveyOptions(Guid id);
    }
}
