using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Common.Constants
{
    public static class ErrorMessage
    {
        public const string InvalidUser = "Username or Password is Incorrect";
        public const string AddToDb = "Unable to add record some error occurred";
        public const string UpdateToDb = "Unable to update record ! some error occurred";
        public const string DeleteToDb = "Unable to delete record ! some error occurred";
        public const string UsernameTaken = "User name not avilable";
        public const string EmailTaken = "Email Already taken";
        public const string GenrateError = "Unable to generate meal plan with this settings";
        public const string Error = "Some error occurred.";
        public const string InvalidToken = "Invalid Token";
        public const string ExistingUser = "Thank you for your interest, looks like we already have your email in our pre signup list. We will email you when we launch";
        public const string InvalidUserType = "Please select 1 for Admin and 2 for User.";
    }
}
