using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Common.Model
{
    public class UserAddressRequest
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Street_Num { get; set; }
        public string Street_Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
        public bool IsPrimary { get; set; }

        public string UserId { get; set; }
    }
}
