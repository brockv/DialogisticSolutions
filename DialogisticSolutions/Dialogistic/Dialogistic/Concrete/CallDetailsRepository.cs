using Dialogistic.Abstract;
using Dialogistic.DAL;
using Dialogistic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialogistic.Concrete
{
    public class CallLogsRepository : DialogisticRepository<CallLog>, ICallLogsRepository
    {
        public CallLogsRepository(DialogisticContext context) : base(context)
        {
        }

        public IEnumerable<CallLog> GetCallLogs()
        {
            return DialogisticContext.CallLogs.ToList();
        }

        public DialogisticContext DialogisticContext
        {
            get { return Context as DialogisticContext; }
        }
    }
}