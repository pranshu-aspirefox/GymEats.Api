using AutoMapper;
using GymEats.Common.Constants;
using GymEats.Common.Enum;
using GymEats.Common.Model;
using GymEats.Data;
using GymEats.Data.Entity;
using GymEats.Services.Common;
using GymEats.Services.Repository.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly GymEatsDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICommonService _commonService;
        private readonly GymEatsDbContext _dbContext;
        private readonly IGenericRepository<UserDetails> _userDetailsRepository;

        public AuthService(
            UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            RoleManager<IdentityRole> roleManager, 
            IConfiguration configuration, 
            GymEatsDbContext dataContext, 
            IMapper mapper, 
            ICommonService commonService, GymEatsDbContext gymEatsDbContext, IGenericRepository<UserDetails> userDetailsRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = configuration;
            _context = dataContext;
            _mapper = mapper;
            _commonService = commonService;
            _dbContext = gymEatsDbContext;
            _userDetailsRepository = userDetailsRepository;
        }

        public async Task<AuthResult<JwtResult>> LogIn(LoginViewModel loginModel)
        {
            if (loginModel == null || string.IsNullOrEmpty(loginModel.Email) || string.IsNullOrEmpty(loginModel.Password))
                return AuthResult<JwtResult>.UnvalidatedResult;
            var token = await ValidUser(loginModel);

            if (token.Access_token == null)
                return AuthResult<JwtResult>.UnauthorizedResult;

            return AuthResult<JwtResult>.TokenResult(token);
        }

        public async Task<JwtResult> ValidUser(LoginViewModel model)
        {
            JwtResult token = new JwtResult();
            try
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                    if(result.Succeeded)
                    {
                        token = await CreateToken(user);
                        return token;
                    }
                }
                return token;
            }
            catch(Exception ex)
            {
                throw new Exception(ErrorMessage.InvalidUser);
            }
        }

        public async Task<JwtResult> CreateToken(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            foreach(var role in roles)
            {
                claims.Add(new Claim("role", role));
            }
            var identity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                identity.Claims,
                expires: DateTime.UtcNow.AddHours(10),
                signingCredentials: creds
               );
            JwtResult retsults = new JwtResult()
            {
                Access_token = new JwtSecurityTokenHandler().WriteToken(token),
                Expires_in = token.ValidTo
            };

            return retsults;
        }

        public async Task<AuthResult<JwtResult>> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                if(model == null ||
                    string.IsNullOrEmpty(model.Email) ||
                    string.IsNullOrEmpty(model.ConfirmPassword) ||
                    string.IsNullOrEmpty(model.NewPassword) ||
                    string.IsNullOrEmpty(model.CurrentPassword) ||
                    model.NewPassword != model.ConfirmPassword)
                {
                    return AuthResult<JwtResult>.UnvalidatedResult;
                }
                LoginViewModel loginDto = new LoginViewModel()
                {
                    Email = model.Email,
                    Password = model.CurrentPassword
                };
                var token = await ValidUser(loginDto);

                if (token.Access_token == null)
                {
                    return AuthResult<JwtResult>.UnauthorizedResult;
                }

                var user = await _userManager.FindByEmailAsync(model.Email);


                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (result.Succeeded)
                    return AuthResult<JwtResult>.SucceededResult;


                return AuthResult<JwtResult>.UnauthorizedResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<AuthResult<Guid>> RequestPassword(RequestPasswordViewModel requestPasswordModel)
        {
            try
            {

                if (requestPasswordModel == null ||
                    string.IsNullOrEmpty(requestPasswordModel.Email))
                    return AuthResult<Guid>.UnvalidatedResult;

                var user = await _userManager.FindByEmailAsync(requestPasswordModel.Email);

                if (user != null)
                {
                    Guid token = Guid.NewGuid();
                    var passwordResetToken = token;
                    user.PasswordResetToken = passwordResetToken;
                    user.ResetTokenExirationTime = DateTime.UtcNow.AddHours(3);
                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                        return AuthResult<Guid>.TokenResult(passwordResetToken);
                }

                return AuthResult<Guid>.UnvalidatedResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<AuthResult<JwtResult>> RestorePassword(RestorePasswordViewModel restorePasswordDto)
        {
            if (restorePasswordDto == null ||
                string.IsNullOrEmpty(restorePasswordDto.PasswordResetToken) ||
                string.IsNullOrEmpty(restorePasswordDto.Password) ||
                string.IsNullOrEmpty(restorePasswordDto.ConfirmPassword) ||
                restorePasswordDto.ConfirmPassword != restorePasswordDto.Password
            )
                return AuthResult<JwtResult>.UnvalidatedResult;

            var user = await GetUserByToken(restorePasswordDto.PasswordResetToken);

            if (user.ResetTokenExirationTime < DateTime.UtcNow)
            {
                return AuthResult<JwtResult>.UnvalidatedResult;
            }

            if (user != null)
            {
                try
                {
                    var result = await _userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddPasswordAsync(user, restorePasswordDto.Password);

                    }

                    var token = await CreateToken(user);
                    return AuthResult<JwtResult>.TokenResult(token);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

            return AuthResult<JwtResult>.UnvalidatedResult;
        }

        public async Task<User> GetUserByToken(string token)
        {
            try
            {
                var user = _context.Users
               .FirstOrDefault(x => x.PasswordResetToken.ToString().ToLower() == token.ToLower());

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<User> GetUserById(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<UserViewModel> Register(RegisterViewModel model)
        {
            User user = _mapper.Map<RegisterViewModel, User>(model);
            user.Id = user.Id = Guid.NewGuid().ToString();
            user.CreatedOn = DateTime.UtcNow;

            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                if (result.Succeeded)
                {
                    //var role = _commonService.GetEnumDescription((UserType)model.UserType);
                    //await _userManager.AddToRoleAsync(user, role);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
                else
                {
                    throw new Exception(ErrorMessage.UsernameTaken);
                }

                var userData = _mapper.Map<UserViewModel>(user);
                userData.EmailConfirmatiomToken = token;

                return userData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<AuthResult<JwtResult>> CreateToken(RegisterViewModel model)
        {
            User user = _mapper.Map<RegisterViewModel, User>(model);
            var token = await CreateToken(user);

            return AuthResult<JwtResult>.TokenResult(token);
        }

        public async Task<bool> UpdateUserData(User user)
        {
            var updated = await _userManager.UpdateAsync(user);
            if (updated.Succeeded)
            {
                _dbContext.Entry(user).State = EntityState.Modified;
                var res =  await _dbContext.SaveChangesAsync();
                return res > 0;
            }
            return false;
        }

        public async Task<IEnumerable<UserDetailViewModel>> GetUserList(int pageSize = 10, int pageNo = 1, string sortOrder = "", string search = "")
        {
            try
            {
                List<UserDetails> userList;

                //searching
                if (!string.IsNullOrEmpty(search))
                {
                    userList = await _dbContext.UserDetails.Include(x => x.User).Where(x => x.User.FirstName.ToLower().Contains(search.ToLower()) || x.User.LastName.ToLower().Contains(search.ToLower()) || x.Gender.ToLower().Contains(search.ToLower()) || x.Age.Equals(search.ToLower()) || x.User.Email.ToLower().Contains(search.ToLower())).ToListAsync();
                }
                else
                {
                    userList = await _dbContext.UserDetails.Include(x => x.User).ToListAsync();
                }

                //sorting
                switch (sortOrder?.ToLower())
                {
                    case "fname_desc":
                        userList = userList.OrderByDescending(x => x.User.FirstName).ToList();
                        break;
                    case "lname":
                        userList = userList.OrderByDescending(x => x.User.LastName).ToList();
                        break;
                    case "lname_desc":
                        userList = userList.OrderByDescending(x => x.User.LastName).ToList();
                        break;
                    case "createdon":
                        userList = userList.OrderBy(x => x.CreatedOn).ToList();
                        break;
                    case "createdon_desc":
                        userList = userList.OrderByDescending(x => x.CreatedOn).ToList();
                        break;
                    default:
                        userList = userList.OrderBy(x => x.User.FirstName).ToList();
                        break;
                }

                //pagination
                userList = userList.Skip((pageNo-1)*pageSize).Take(pageSize).ToList();


                List<UserDetailViewModel> list = _mapper.Map<List<UserDetails>, List<UserDetailViewModel>>(userList);

                return list;
            }catch (Exception ex) { throw ex; }
        }

    }
}
