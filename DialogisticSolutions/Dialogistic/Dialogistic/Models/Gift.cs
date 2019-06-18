namespace Dialogistic.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Gift
    {
        public int GiftID { get; set; }

        public int ConstituentID { get; set; }

        public int CallID { get; set; }

        public bool Printed { get; set; }

        public decimal? GiftAmount { get; set; }

        [StringLength(128)]
        public string GiftType { get; set; }

        [StringLength(128)]
        public string GiftRecipient { get; set; }

        public virtual CallLog CallLog { get; set; }

        public virtual Constituent Constituent { get; set; }
    }
}
