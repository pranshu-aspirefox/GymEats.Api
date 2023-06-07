using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.ComponentModel;

namespace GymEats.Services.Common
{
    public class CommonService : ICommonService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CommonService> _logger;

        public CommonService(IConfiguration configuration, ILogger<CommonService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public bool SendEmail(List<string> to, List<string> cc, List<string> bcc, string subject, string body)
        {
            try
            {
                var apiKey = _configuration["Email:ApiKey"];
                var client = new SendGridClient(apiKey);
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(_configuration["Email:Username"], "test"),
                    Subject = subject,
                    HtmlContent = body
                };

                foreach (var i in to)
                {
                    msg.AddTo(new EmailAddress(i, i));
                }
                var response = client.SendEmailAsync(msg).GetAwaiter().GetResult();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public string GetEnumDescription(Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : enumValue.ToString();
        }

    }
}
