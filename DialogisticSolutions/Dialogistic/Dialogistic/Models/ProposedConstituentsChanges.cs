namespace Dialogistic.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProposedConstituentsChanges
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ConstituentID { get; set; }

        [StringLength(100)]
        public string PrimaryAddressee { get; set; }

        [Required]
        [StringLength(100)]
        public string PreferredAddressLine1 { get; set; }

        [StringLength(100)]
        public string PreferredAddressLine2 { get; set; }

        [StringLength(100)]
        public string PreferredAddressLine3 { get; set; }

        [Required]
        [StringLength(50)]
        public string PreferredCity { get; set; }

        [Required]
        [StringLength(50)]
        public string PreferredState { get; set; }

        [Required]
        [StringLength(20)]
        public string PreferredZIP { get; set; }

        [Required]
        [StringLength(50)]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string MobilePhoneNumber { get; set; }

        [StringLength(50)]
        public string AlternatePhoneNumber { get; set; }

        public bool Deceased { get; set; }

        [StringLength(20)]
        public string DonationStatus { get; set; }

        [StringLength(20)]
        public string UniversityRelationship { get; set; }

        public int? CallPriority { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public string getFullAddress()
        {
            // Build the address line as a string
            string lineOne = PreferredAddressLine1.ToString();
            string city = PreferredCity.ToString();
            string state = PreferredState.ToString();
            string address = lineOne + " " + city + " " + state;

            return address;
        }
    }
}
