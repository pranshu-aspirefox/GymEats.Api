using AutoMapper;
using GymEats.Common.Constants;
using GymEats.Common.Model;
using GymEats.Data;
using GymEats.Services.Repository.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Question
{
    public class QuestionService : IQuestionService
    {
        private readonly GymEatsDbContext _context;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Data.Entity.Question> _questionRepository;

        public QuestionService(GymEatsDbContext context, IMapper mapper, IGenericRepository<GymEats.Data.Entity.Question> questionRepository)
        {
            _context = context;
            _mapper = mapper;
            _questionRepository = questionRepository;
        }

        public async Task<IEnumerable<QuestionViewModel>> QuestionList()
        {
            var questionList = (await _questionRepository.GetAsync(x => x.IsDeleted == false)).Include(x => x.UserCreatedBy).ToList();
            var questionViewList = _mapper.Map<IList<GymEats.Data.Entity.Question>, IList<QuestionViewModel>>(questionList);
            return questionViewList;
        }

        public async Task<GymEats.Data.Entity.Question> GetById(Guid id)
        {
            var question = await  _questionRepository.GetByIdAsync(id);
            return question;
        }

        public async Task<QuestionViewModel> GetViewModelById(Guid id)
        {
            var question = await GetById(id);
            return _mapper.Map<GymEats.Data.Entity.Question, QuestionViewModel>(question);
        }

        public async Task<QuestionViewModel> AddNewQuestion(QuestionRequestModel model)
        {
            try
            {
                var question = new GymEats.Data.Entity.Question {
                    Id = Guid.NewGuid(),
                    Label = model.Label,
                    IsPrimary = model.IsPrimary,
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = model.CreatedBy
                };
                await _questionRepository.InsertAsync(question);
                var result = await _questionRepository.SaveAsync();
                return _mapper.Map<GymEats.Data.Entity.Question, QuestionViewModel>(question);
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorMessage.AddToDb);
            }
        }

        public async Task<QuestionViewModel> DeleteQuestion(Guid id)
        {
            try
            {
                var question =await _questionRepository.GetByIdAsync(id);
                if (question != null) 
                {
                    await _questionRepository.DaleteAsync(id);
                    var result = await _questionRepository.SaveAsync();
                    if((int)result > 0)
                        return _mapper.Map<GymEats.Data.Entity.Question, QuestionViewModel>(question); 
                }
                throw new Exception("Failed to delete. Invalid Id.");
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorMessage.DeleteToDb);
            }
        }

        public async Task<QuestionViewModel> UpdateQuestion(UpdateQuestionRequest model)
        {
            try
            {
                var question = await _questionRepository.GetByIdAsync(model.Id);
                if (question != null)
                {
                    if(!string.IsNullOrEmpty(model.Label))
                        question.Label = model.Label;
                    if(model.IsPrimary != null)
                        question.IsPrimary = (bool)model.IsPrimary;
                    question.UpdatedBy = model.UpdatedBy;
                    question.UpdatedOn = DateTime.UtcNow;
                    await _questionRepository.UpdateAsync(question);
                    var result = await _questionRepository.SaveAsync();
                    if((int)result > 0)
                        return _mapper.Map<QuestionViewModel>(question);
                }
                throw new Exception("Failed to update. please try again.");
            }
            catch (Exception ex) 
            {
                throw new Exception(ErrorMessage.UpdateToDb);
            }
        }
    }
}
