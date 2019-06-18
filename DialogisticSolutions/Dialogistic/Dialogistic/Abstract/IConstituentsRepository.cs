using Dialogistic.Models;
using System.Collections.Generic;

namespace Dialogistic.Abstract
{
    /// <summary>
    /// The interface for interacting with the Constituents table/model.
    /// </summary>
    public interface IConstituentsRepository : IDialogisticRepository<Constituent>
    {
        IEnumerable<Constituent> GetConstituents();
    }
}
