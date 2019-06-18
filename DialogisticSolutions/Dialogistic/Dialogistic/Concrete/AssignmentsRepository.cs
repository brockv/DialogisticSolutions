using Dialogistic.Abstract;
using Dialogistic.DAL;
using Dialogistic.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Dialogistic.Concrete
{
    /// <summary>
    /// The repository for interacting with the ProposedConstituentsChanges table/model, which implements its respective interface.
    /// </summary>
    public class AssignmentsRepository : DialogisticRepository<CallAssignment>, IAssignmentsRepository
    {
        public AssignmentsRepository(DialogisticContext context) : base(context)
        {
        }

        public IEnumerable<CallAssignment> GetCallAssignments()
        {
            return DialogisticContext.CallAssignments.ToList();
        }

        public DialogisticContext DialogisticContext
        {
            get { return Context as DialogisticContext; }
        }
    }
}