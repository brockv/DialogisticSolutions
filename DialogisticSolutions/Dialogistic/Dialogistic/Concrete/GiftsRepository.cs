using Dialogistic.Abstract;
using Dialogistic.DAL;
using Dialogistic.Models;
using System.Collections.Generic;
using System.Linq;

namespace Dialogistic.Concrete
{
    /// <summary>
    /// The repository for interacting with the Constituents table/model, which implements its respective interface.
    /// </summary>
    public class GiftsRepository : DialogisticRepository<Gift>, IGiftsRepository
    {
        public GiftsRepository(DialogisticContext context) : base(context)
        {
        }

        public IEnumerable<Gift> GetGifts()
        {
            return DialogisticContext.Gifts.ToList();
        }

        public DialogisticContext DialogisticContext
        {
            get { return Context as DialogisticContext; }
        }
    }
}
