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

    public partial class Guardian
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Guardian()
        {
            this.FamilyMembers = new HashSet<FamilyMember>();
            this.Participants = new HashSet<Participant>();
        }


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GuardianID { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string GuardianFirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string GuardianLastName { get; set; }

        [DisplayName("Street Address")]
        public string GuardianAddress { get; set; }

        [DisplayName("City")]
        public string GuardianCity { get; set; }


        [DisplayName("Zip")]
        //TO DO: review field type (should be string as it is not used numerically) - Erika review (SP-245 created 11/21/16)
        public int GuardianZip { get; set; }


        [DisplayName("Cell Phone")]
        [DataType(DataType.PhoneNumber)]
        public string GuardianCellPhone { get; set; }


        //TO DO: is 50 characters sufficient for length - Erika review
        [DisplayName("Email Address")]
        [DataType(DataType.EmailAddress)]
        public string GuardianEmail { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date Packet Sent")]
        public Nullable<System.DateTime> PacketSentDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Paperwork Received")]
        public Nullable<System.DateTime> ReceiptDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date Confirmed")]
        //TO DO: review if this field was kept - JS
        public Nullable<System.DateTime> ConfirmationSentDate { get; set; }

        [DisplayName("Health Form Received")]
        public Nullable<bool> HealthForm { get; set; }


        [DisplayName("Photo Ack Received")]
        public Nullable<bool> PhotoAck { get; set; }

        [DisplayName("Tent Required")]
        public bool Tent { get; set; }

        [DisplayName("Attending")]
        public string AttendingCode { get; set; }

        [DisplayName("Comments")]
        public string Comments { get; set; }

        [DisplayName("Relationship")]
        public string Relationship { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FamilyMember> FamilyMembers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Participant> Participants { get; set; }
    }
}
