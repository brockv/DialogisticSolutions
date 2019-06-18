namespace Dialogistic.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CallLog
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CallLog()
        {
            CallAssignments = new HashSet<CallAssignment>();
            Gifts = new HashSet<Gift>();
        }

        [Key]
        public int CallID { get; set; }

        [Required]
        [StringLength(128)]
        public string CallerID { get; set; }

        public int ConstituentID { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateOfCall { get; set; }

        public bool CallAnswered { get; set; }

        public bool LineAvailable { get; set; }

        [StringLength(128)]
        public string CallOutcome { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CallAssignment> CallAssignments { get; set; }

        public virtual Constituent Constituent { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Gift> Gifts { get; set; }
    }
}
