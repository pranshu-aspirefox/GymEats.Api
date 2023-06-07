using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Suggestic.HelperClass
{
    public class SuggesticUserLogin
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
    }

    public class LoginData
    {
        public SuggesticUserLogin login { get; set; }
    }
}
