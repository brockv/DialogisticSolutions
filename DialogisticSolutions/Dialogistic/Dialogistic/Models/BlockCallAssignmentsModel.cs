using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dialogistic.Models
{
    public class BlockCallAssignmentsModel
    {
        public string CallerID { get; set; }
        public int[] Constituents { get; set; }
    }
}