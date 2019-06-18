namespace Dialogistic.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Constituent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Constituent()
        {
            CallAssignments = new HashSet<CallAssignment>();
            CallLogs = new HashSet<CallLog>();
            Gifts = new HashSet<Gift>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Constituent ID")]
        public int ConstituentID { get; set; }

        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z. ]+$", ErrorMessage = "Special characters are not allowed.")]
        [Display(Name="Name")]
        public string PrimaryAddressee { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[0-9a-zA-Z.# ]+$", ErrorMessage = "Invalid format.")]
        [Display(Name = "Address 1")]
        public string PreferredAddressLine1 { get; set; }

        [StringLength(100)]
        [RegularExpression(@"^[0-9a-zA-Z.# ]+$", ErrorMessage = "Invalid format.")]
        [Display(Name = "Address 2")]
        public string PreferredAddressLine2 { get; set; }

        [StringLength(100)]
        [RegularExpression(@"^[0-9a-zA-Z.# ]+$", ErrorMessage = "Invalid format.")]
        [Display(Name = "Address 3")]
        public string PreferredAddressLine3 { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z. ]+$", ErrorMessage = "Only letters A - Z are allowed.")]
        [Display(Name = "City")]
        public string PreferredCity { get; set; }

        [Required]
        [Display(Name = "State")]
        public string PreferredState { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^\d{5}(?:[-\s]\d{4})?$", ErrorMessage = "Please use either of the following formats: XXXXX or XXXXX-XXXX")]
        [Display(Name = "ZIP")]
        public string PreferredZIP { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression("\\d{3}-\\d{3}-\\d{4}", ErrorMessage = "Please use the format: XXX-XXX-XXXX")]
        [Display(Name = "Primary Number")]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        [RegularExpression("\\d{3}-\\d{3}-\\d{4}", ErrorMessage = "Please use the format: XXX-XXX-XXXX")]
        [Display(Name = "Mobile Number")]
        public string MobilePhoneNumber { get; set; }

        [StringLength(50)]
        [RegularExpression("\\d{3}-\\d{3}-\\d{4}", ErrorMessage = "Please use the format: XXX-XXX-XXXX")]
        [Display(Name = "Alt. Number")]
        public string AlternatePhoneNumber { get; set; }

        public bool Deceased { get; set; }

        [StringLength(20)]
        //[RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Only letters A-Z and spaces are allowed.")]
        [Display(Name = "Donation Status")]
        public string DonationStatus { get; set; }

        [StringLength(20)]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Only letters A-Z and spacesare allowed.")]
        [Display(Name = "University Relationship")]
        public string UniversityRelationship { get; set; }

        //[RegularExpression(@"[^\d+$]", ErrorMessage = "Only numbers are allowed.")]
        [Display(Name = "Call Priority")]
        public int? CallPriority { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CallAssignment> CallAssignments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CallLog> CallLogs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Gift> Gifts { get; set; }

        public string[] getLocation()
        {
            string[] geolocation = new string[2];
            geolocation[0] = Latitude.ToString();
            geolocation[1] = Longitude.ToString();
            return geolocation;
        }

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
