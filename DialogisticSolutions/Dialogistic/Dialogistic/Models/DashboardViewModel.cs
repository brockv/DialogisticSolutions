using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialogistic.Models
{
    public class DashboardViewModel
    {
        public int TotalRemainingCalls { get; set; }
        public double TotalDonations { get; set; }

        public virtual UserProfile User { get; set; }
    }
}