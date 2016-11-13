
namespace SNCRegistration.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Guardian
    {
        public int GuardianID { get; set; }

        [Required]
     
        [DisplayName("First Name")]
        public string GuardianFirstName { get; set; }

        [Required]
     
        [DisplayName("Last Name")]
        public string GuardianLastName { get; set; }

     
        //TO DO: increase field size -Erika review
        [DisplayName("Street Address")]
        public string GuardianAddress { get; set; }

     
        [DisplayName("City")]
        public string GuardianCity { get; set; }

     
        [DisplayName("Zip")]
        //TO DO: review field type (should be string as it is not used numerically) - Erika review
        public int GuardianZip { get; set; }

        
        [DisplayName("Cell Phone")]
        [DataType(DataType.PhoneNumber)]
        public string GuardianPhone { get; set; }

     
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
    }
}