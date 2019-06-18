using Dialogistic.Models;
using System.Collections.Generic;

namespace Dialogistic.Abstract
{
    public interface ICallLogsRepository : IDialogisticRepository<CallLog>
    {
        IEnumerable<CallLog> GetCallLogs();
    }
}
