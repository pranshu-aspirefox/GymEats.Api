using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymEats.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public string GetUserId()
        {
            var userClaim = User.FindFirst("jti");
            return userClaim.Value;

        }
    }
}
