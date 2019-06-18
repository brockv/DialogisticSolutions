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
    public class ConstituentsRepository : DialogisticRepository<Constituent>, IConstituentsRepository
    {
        public ConstituentsRepository(DialogisticContext context) : base(context)
        {
        }

        //public IEnumerable<Constituent> Constituents
        //{
        //    get { return DialogisticContext.Constituents; }
        //}

        public IEnumerable<Constituent> GetConstituents()
        {
            return DialogisticContext.Constituents.ToList();
        }

        public DialogisticContext DialogisticContext
        {
            get { return Context as DialogisticContext; }
        }
    }
}
