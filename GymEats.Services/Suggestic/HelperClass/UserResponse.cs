using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Suggestic.HelperClass
{
    public class Users
    {
        public string id { get; set; }
        public string databaseId { get; set; }
        public string email { get; set; }
        public string name { get; set; }
    }

    public class CreateUser
    {
        public bool success { get; set; }
        public string message { get; set; }
        public Users user { get; set; }
    }

    public class SuggesticUserData
    {
        public CreateUser createUser { get; set; }
    }
}
