using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SNCRegistration.ViewModels
{
    public class GuardiansViewModel
    {
        
        public int GuardianID { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("First Name")]
        public string GuardianFirstName { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Last Name")]
        public string GuardianLastName { get; set; }

        [MaxLength(50)]
       //TO DO: increase field size -Erika review
        [DisplayName("Street Address")]
        public string GuardianAddress { get; set; }
 
        [MaxLength(50)]
        [DisplayName("City")]
        public string GuardianCity { get; set; }

        [MaxLength(50)]
        [DisplayName("Zip")]
        //TO DO: review field type (should be string as it is not used numerically) - Erika review
        public int GuardianZip { get; set; }

        [MaxLength(10)]
        [DisplayName("Phone")]
        [DataType(DataType.PhoneNumber)]
        public string GuardianPhone { get; set; }
        
        [MaxLength(50)]
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