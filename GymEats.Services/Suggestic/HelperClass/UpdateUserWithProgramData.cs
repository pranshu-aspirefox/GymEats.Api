using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Suggestic.HelperClass
{
    public class UpdateUserProgram
    {
        public bool success { get; set; }
        public string message { get; set; }
    }

    public class UpdateUserWithProgramData
    {
        public UpdateUserProgram updateUserProgram { get; set; }
    }

    public class UpdateUserWithProgramRoot
    {
        public UpdateUserWithProgramData data { get; set; }
    }
}
