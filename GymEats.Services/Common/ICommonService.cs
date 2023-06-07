using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Common
{
    public interface ICommonService
    {
        bool SendEmail(List<string> to, List<string> cc, List<string> bcc, string subject, string body);
        public string GetEnumDescription(Enum enumValue);
    }
}
