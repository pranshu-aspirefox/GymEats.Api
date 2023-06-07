using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Common.Model
{
    public class JwtResult
    {
        public string Access_token { get; set; }
        public DateTime Expires_in { get; set; }
    }
}
