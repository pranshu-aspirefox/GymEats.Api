using GymEats.Common.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymEats.Api.Controllers
{
    [Route("api")]
    public class PasswordAuthController : Controller
    {
        public PasswordAuthController()
        {

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPasswordByMail(string resetToken)
        {
            ResetPasswordModel resetPasswordModel = new ResetPasswordModel();
            resetPasswordModel.ResetKey = resetToken;
            return View(resetPasswordModel);
        }
    }
}
