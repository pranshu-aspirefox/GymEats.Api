using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Suggestic.HelperClass
{
    public class DeleteMyProfile
    {
        public bool success { get; set; }
    }

    public class RemoveUserData
    {
        public DeleteMyProfile deleteMyProfile { get; set; }
    }
}
