using Dialogistic.Models;
using System.Collections.Generic;

namespace Dialogistic.Abstract
{
    /// <summary>
    /// The interface for interacting with the ProposedConstituentsChanges table/model.
    /// </summary>
    public interface IChangesRepository : IDialogisticRepository<ProposedConstituentsChanges>
    {
        IEnumerable<ProposedConstituentsChanges> GetProposedChanges();
    }
}
