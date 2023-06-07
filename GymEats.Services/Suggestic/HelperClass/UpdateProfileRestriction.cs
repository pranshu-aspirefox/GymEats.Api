using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Suggestic.HelperClass
{
    public class ProfileRestrictionsUpdate
    {
        public bool success { get; set; }
    }

    public class ProfileRestrictionsUpdateData
    {
        public ProfileRestrictionsUpdate profileRestrictionsUpdate { get; set; }
    }
}
