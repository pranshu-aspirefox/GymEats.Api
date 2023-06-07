using AutoMapper;
using GymEats.Common.Model;
using GymEats.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Data.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<RegisterViewModel, User>();
            CreateMap<User, RegisterViewModel>();

            CreateMap<UserViewModel, User>();
            CreateMap<User, UserViewModel>();

            CreateMap<User, UserListModel>();
            CreateMap<UserListModel, User>();

            CreateMap<DietViewModel, Diet>();
            CreateMap<Diet, DietViewModel>();

            CreateMap<Diet, DietRequestModel>();
            CreateMap<DietRequestModel, Diet>();

            CreateMap<Question, QuestionViewModel>();
            CreateMap<QuestionViewModel, Question>();

            CreateMap<Option, OptionViewModel>();
            CreateMap<OptionViewModel, Option>();

            CreateMap<SurveyViewModel, Survey>();
            CreateMap<Survey, SurveyViewModel> ();

            CreateMap<UserAddress, UserAddressRequest> ();
            CreateMap<UserAddressRequest, UserAddress> ();

            CreateMap<UserDetailViewModel, UserDetails> ();
            CreateMap<UserDetails, UserDetailViewModel> ();

            CreateMap<QuestionViewRequestModel, SurveyOption> ();
            CreateMap<SurveyOption, QuestionViewRequestModel> ();

            CreateMap<Question, QuestionViewRequestModel> ();
            CreateMap<QuestionViewRequestModel, Question> ();
        }
    }
}
