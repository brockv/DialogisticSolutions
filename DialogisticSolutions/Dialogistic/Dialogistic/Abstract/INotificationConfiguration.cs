using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogistic.Abstract
{
    public interface INotificationConfiguration
    {
        string AccountSid { get; }
        string AuthToken { get; }
        string DefaultFromPhoneNumber { get; }
    }
}
