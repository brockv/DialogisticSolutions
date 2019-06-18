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
    public class ChangesRepository : DialogisticRepository<ProposedConstituentsChanges>, IChangesRepository
    {
        public ChangesRepository(DialogisticContext context) : base(context)
        {
        }

        public IEnumerable<ProposedConstituentsChanges> GetProposedChanges()
        {
            return (IEnumerable<ProposedConstituentsChanges>) DialogisticContext.ProposedConstituentsChanges.ToList();
        }

        public DialogisticContext DialogisticContext
        {
            get { return Context as DialogisticContext; }
        }
    }
}