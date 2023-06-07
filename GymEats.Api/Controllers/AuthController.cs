using AutoMapper;
using GymEats.Common.Constants;
using GymEats.Common.Enum;
using GymEats.Common.Model;
using GymEats.Data.Entity;
using GymEats.Services.Auth;
using GymEats.Services.Common;
using GymEats.Services.Service;
using GymEats.Services.Suggestic;
using GymEats.Services.Suggestic.HelperClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Dynamic;
using System.Text.RegularExpressions;

namespace GymEats.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICommonService _commonService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment,
            ICommonService commonService,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _authService = authService;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _commonService = commonService;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorList = ModelState.Select(o => new
                    {
                        Key = o.Key,
                        Error = o.Value.Errors.ToList().FirstOrDefault().ErrorMessage,
                    }).ToList();
                    return BadRequest(errorList);
                }

                var result = await _authService.LogIn(loginModel);

                if (result.Succeeded)
                {
                    return Ok(new { token = result.Data });
                }
                return Unauthorized(ErrorMessage.InvalidUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel changePasswordModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorList = ModelState.Select(o => new
                    {
                        Key = o.Key,
                        Error = o.Value.Errors.ToList().FirstOrDefault().ErrorMessage,
                    }).ToList();
                    return BadRequest(errorList);
                }
                changePasswordModel.Email = User.Identity.Name;

                var result = await _authService.ChangePassword(changePasswordModel);

                if (result.Succeeded)
                {
                    return Ok(SuccessMessages.UpdatePasswordMessage);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("request-pass")]
        public async Task<IActionResult> RequestPassword([FromBody] RequestPasswordViewModel requestPasswordModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorList = ModelState.Select(o => new
                    {
                        Key = o.Key,
                        Error = o.Value.Errors.ToList().FirstOrDefault().ErrorMessage,
                    }).ToList();
                    return BadRequest(errorList);
                }
                var result = await _authService.RequestPassword(requestPasswordModel);
                string localHost = _configuration["Host:Development"];

                if (_webHostEnvironment.IsProduction())
                {
                    localHost = _configuration["Host:Production"];
                }

                string link = string.Format("{0}/api/reset-password?resetToken={1}", localHost, result.Data);

                var path = _webHostEnvironment.ContentRootPath;
                var filePath = System.IO.Path.Combine(path, "Email/forgot-password.html");
                var imagePath = System.IO.Path.Combine(path, "Email/images/logo.png");

                var text = System.IO.File.ReadAllText(filePath);
                text = text.Replace("[ImageSrc]", imagePath);
                text = text.Replace("{{ACTIVATIONLINK}}", link);
                text = text.Replace("*#FirstName#*", requestPasswordModel.Email);

                _commonService.SendEmail(new List<string> { requestPasswordModel.Email }, null, null, "Forgot Password", text);

                if (result.Succeeded)
                    return Ok(new { result.Data, Description = SuccessMessages.ResetPasswordMessage });

                return BadRequest();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("reset-pass")]
        public async Task<IActionResult> RestorePassword([FromBody] RestorePasswordViewModel restorePasswordModel)
        {
            var result = await _authService.RestorePassword(restorePasswordModel);

            if (result.Succeeded)
                return Ok(new { token = result.Data });

            return BadRequest(ErrorMessage.Error);
        }

        [HttpPost]
        [Route("sign-out")]
        public async Task<IActionResult> SignOut()
        {
            await _authService.SignOut();
            dynamic response = new ExpandoObject();
            response.Success = true;
            response.Message = "Successfully logged out.";
            return Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorList = ModelState.Select(o => new
                    {
                        Key = o.Key,
                        Error = o.Value.Errors.ToList().FirstOrDefault().ErrorMessage,
                    }).ToList();
                    return BadRequest(errorList);
                }

                var existingUser = await _authService.GetUserByEmail(model.Email);
                if (existingUser != null)
                {
                    return Ok(new { message = ErrorMessage.ExistingUser });
                }
                else
                {

                    RegisterViewModel userModel = new RegisterViewModel();
                    userModel.FirstName = model.FirstName;
                    userModel.LastName = model.LastName;
                    userModel.Email = model.Email;
                    userModel.UserName = model.Email;
                    userModel.Password = model.Password;
                    userModel.UserType = (int)UserType.User;
                    var user = await _authService.Register(userModel);

                    if (user != null)
                    {
                        var path = _webHostEnvironment.ContentRootPath;
                        var filePath = Path.Combine(path, "Email/confirm-email.html");
                        var text = System.IO.File.ReadAllText(filePath);
                        text = text.Replace("[Recipient]", user.FirstName + " " + user.LastName);
                        string localHost = _configuration["Host:Development"];

                        if (_webHostEnvironment.IsProduction())
                        {
                            localHost = _configuration["Host:Production"];
                        }

                        string callBackUrl = $"{localHost}/api/auth/confirmEmail?token={user.EmailConfirmatiomToken}&email={user.Email}";
                        text = text.Replace("[URL]", callBackUrl);
                        _commonService.SendEmail(new List<string> { user.Email }, null, null, "Verify Your Email Address", body: text);
                    }

                    return Ok(new { message = SuccessMessages.SignUp });
                }
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpGet]
        [Route("confirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            string emailRegex = @"^[A-Za-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$";
            var match = Regex.Match(email, emailRegex);
            if (!match.Success)
            {
                return BadRequest(new {
                    Success = false,
                    ErrorMessage = "Please enter valid email."
                });
            }

            token = token.Replace(" ", "+");

            var user = await _userManager.FindByEmailAsync(email)
;
            if (user is null) return BadRequest($"User not found for email: {email}");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return (result.Succeeded ? Ok(new { result = SuccessMessages.Sucess }) : BadRequest(ErrorMessage.InvalidToken));
        }

        [HttpGet("GetUserList")]
        public async Task<IActionResult> GetUserList(int? pageSize, int? pageNo, string? sortOrder, string? search)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _authService.GetUserList(pageSize ?? 10, pageNo ?? 1, sortOrder, search);
                if (data.Any())
                {
                    response.Success = true;
                    response.Data = data;
                    return Ok(response);
                }
                response.Success = false;
                response.ErrorMessage = "No data found.";
                return BadRequest(response);
            }
            catch(Exception ex) 
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
        }
    }
}
