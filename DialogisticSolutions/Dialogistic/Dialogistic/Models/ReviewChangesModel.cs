using System.Collections.Generic;

namespace Dialogistic.Models
{
    public partial class ReviewChangesModel
    {
        public List<Constituent> Constituents { get; set; }
        public List<ProposedConstituentsChanges> ConstituentChanges { get; set; }        
    }
}