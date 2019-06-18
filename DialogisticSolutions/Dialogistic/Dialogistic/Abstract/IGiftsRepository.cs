using Dialogistic.Models;
using System.Collections.Generic;

namespace Dialogistic.Abstract
{
    /// <summary>
    /// The interface for interacting with the Gifts table/model.
    /// </summary>
    public interface IGiftsRepository : IDialogisticRepository<Gift>
    {
        IEnumerable<Gift> GetGifts();
    }
}
