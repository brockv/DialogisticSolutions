namespace Dialogistic.Models
{
    using Dialogistic.DAL;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CallAssignment
    {
        [Key]
        [Column(Order = 0)]
        public string CallerID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ConstituentID { get; set; }

        public int? CallLogID { get; set; }

        [NotMapped]
        public string CallerName { get; set; }

        public virtual CallLog CallLog { get; set; }

        public virtual Constituent Constituent { get; set; }
    }
}
