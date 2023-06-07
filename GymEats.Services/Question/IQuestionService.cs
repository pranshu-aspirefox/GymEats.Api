using GymEats.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Question
{
    public interface IQuestionService
    {
        Task<IEnumerable<QuestionViewModel>> QuestionList();
        Task<GymEats.Data.Entity.Question> GetById(Guid id);
        Task<QuestionViewModel> GetViewModelById(Guid id);
        Task<QuestionViewModel> AddNewQuestion(QuestionRequestModel model);
        Task<QuestionViewModel> DeleteQuestion(Guid id);
        Task<QuestionViewModel> UpdateQuestion(UpdateQuestionRequest model);
    }
}
