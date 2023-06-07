using GymEats.Common.Model;
using GymEats.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResult<JwtResult>> LogIn(LoginViewModel loginModel);
        Task<JwtResult> ValidUser(LoginViewModel model);
        Task<JwtResult> CreateToken(User user);
        Task<AuthResult<JwtResult>> ChangePassword(ChangePasswordViewModel model);
        Task<AuthResult<Guid>> RequestPassword(RequestPasswordViewModel requestPasswordModel);
        Task<AuthResult<JwtResult>> RestorePassword(RestorePasswordViewModel restorePasswordDto);
        Task<User> GetUserByToken(string token);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserById(string userId);
        Task SignOut();
        Task<UserViewModel> Register(RegisterViewModel model);
        Task<AuthResult<JwtResult>> CreateToken(RegisterViewModel model);
        Task<bool> UpdateUserData(User user);
        Task<IEnumerable<UserDetailViewModel>> GetUserList(int pageSize = 10, int pageNo = 1, string sortOrder = "", string search = "");
        

    }
}
