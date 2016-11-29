//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SNCRegistration.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class LeadContact
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LeadContact()
        {
            this.Volunteers = new HashSet<Volunteer>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeadContactID { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string LeadContactFirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LeadContactLastName { get; set; }

        [DisplayName("Street Address")]
        public string LeadContactAddress { get; set; }

        [DisplayName("City")]
        public string LeadContactCity { get; set; }

        public string LeadContactState { get; set; }

        [DisplayName("Zip Code")]
        public string LeadContactZip { get; set; }

        [DisplayName("Cell Phone")]
        [DataType(DataType.PhoneNumber)]
        public string LeadContactCellPhone { get; set; }

        [DisplayName("Email Address")]
        [DataType(DataType.EmailAddress)]
        public string LeadContactEmail { get; set; }

        [DisplayName("T-Shirt Order")]
        public Nullable<bool> LeadContactShirtOrder { get; set; }

        [DisplayName("T-Shirt Size")]
        public string LeadContactShirtSize { get; set; }

        [DisplayName("Troop/Chapter/Unit")]
        public string BSType { get; set; }

        [DisplayName("Troop/Chapter/Unit Number")]
        public string UnitChapterNumber { get; set; }

        [DisplayName("Attending")]
        public string VolunteerAttendingCode { get; set; }

        [DisplayName("Saturday Dinner?")]
        public Nullable<bool> SaturdayDinner { get; set; }

        [DisplayName("Booth Name")]
        public string Booth { get; set; }

        [DisplayName("Comments")]
        public string Comments { get; set; }

        public Decimal  TotalFee{ get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Volunteer> Volunteers { get; set; }
    }
}
