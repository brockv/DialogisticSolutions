using Dialogistic.Abstract;
using System.Configuration;

namespace Dialogistic.Concrete
{
    public class NotificationConfiguration : INotificationConfiguration
    {
        public NotificationConfiguration()
        {
            AccountSid = ConfigurationManager.AppSettings["TwilioAccountSid"];
            AuthToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
            DefaultFromPhoneNumber = ConfigurationManager.AppSettings["TwilioFromNumber"];
        }

        public string AccountSid { get; }
        public string AuthToken { get; }
        public string DefaultFromPhoneNumber { get; }
    }
}