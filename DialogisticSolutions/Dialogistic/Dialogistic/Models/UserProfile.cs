namespace Dialogistic.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserProfile
    {
        [Key]
        public int ProfileID { get; set; }

        [Required]
        [StringLength(128)]
        public string UserID { get; set; }

        [Required]
        [StringLength(256)]
        public string UserName { get; set; }

        [Required]
        [StringLength(256)]
        public string FullName { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        public string Avatar { get; set; }

        [StringLength(100)]
        public string SiteTheme { get; set; }

        public bool IsCaller { get; set; }

        public int? TotalCallsMade { get; set; }

        public int? CallsRemaining { get; set; }

        public decimal? DonationsRaised { get; set; }
    }
}
