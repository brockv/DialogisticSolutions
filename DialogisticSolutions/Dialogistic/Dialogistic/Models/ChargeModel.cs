using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogistic.Models
{
    public class ChargeModel
    {
        public string StripeToken { get; set; }
        public decimal PledgeAmount { get; set; }
        public string Name { get; set; }

        public void PassedName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
