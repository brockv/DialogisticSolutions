using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialogistic.Models
{
    public class PledgeVM
    {
        public string Name { get; set; }
        public string CallerName { get; set; }
        public string PAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public decimal? PledgeAmmount { get; set; }
    }
}