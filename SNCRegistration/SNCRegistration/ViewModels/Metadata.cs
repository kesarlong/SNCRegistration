using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SNCRegistration.ViewModels.Metadata
{
    public class Guardian_Metadata
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
          public int GuardianID;

        [Required]
        [Display(Name = "First Name")]
        public string GuardianFirstName;

        [Required]
        [Display(Name = "Last Name")]
        public string GuardianLastName;

        [Display(Name = "Street Address")]
        public string GuardianAddress;

        [Display(Name ="City")]
        public string GuardianCity;

        [MaxLength(10)]
        [Display(Name = "Zip")]
        //TO DO: review field type (should be string as it is not used numerically) - Erika review (SP-245 created 11/21/16)
        public int GuardianZip;


        [Display(Name = "Cell Phone")]
        [DataType(DataType.PhoneNumber)]
        public string GuardianCellPhone;


        //TO DO: is 50 characters sufficient for length - Erika review
        [Display(Name="Email Address")]
        [DataType(DataType.EmailAddress)]
        public string GuardianEmail;

        [DataType(DataType.Date)]
        [Display(Name = "Date Packet Sent")]
        public Nullable<System.DateTime> PacketSentDate;

        [DataType(DataType.Date)]
        [Display(Name = "Paperwork Received")]
        public Nullable<System.DateTime> ReceiptDate;

        [DataType(DataType.Date)]
        [Display(Name = "Date Confirmed")]
        //TO DO: review if this field was kept - JS
        public Nullable<System.DateTime> ConfirmationSentDate; 

        [Display(Name="Health Form Received")]
        public Nullable<bool> HealthForm;


        [Display(Name = "Photo Ack Received")]
        public Nullable<bool> PhotoAck;

        [Display(Name = "Tent Required")]
        public bool Tent;

        [Display(Name = "Attending")]
        public string AttendingCode;

        [Display(Name = "Comments")]
        public string Comments;

        [Display(Name = "Relationship")]
        public string Relationship;
    }

    public class Participant_Metadata
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParticipantID;

        [Required]
        [Display(Name = "First Name")]
        public string ParticipantFirstName;


        [Required]
        [Display(Name = "Last Name")]
        public string ParticipantLastName;


        [Display(Name="Age")]
        public string ParticipantAge ;


        [Display(Name="School")]
        public string ParticipantSchool ;


        //TO DO: database field size increased (Erika)
        [Display(Name="Teacher")]
        public string ParticipantTeacher ;


        [Display(Name="Classroom Scouting")]
        public bool ClassroomScouting ;

        [Display(Name="Health Form")]
        public bool? HealthForm ;

        [Display(Name="Photo Acknowledgment")]
        public bool? PhotoAck ;

        [Display(Name="Attendance")]
        public string AttendingCode ;

        [Display(Name = "Returning")]
        public bool? Returning;


    }

    public class FamilyMember_Metadata
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FamilyMemberID;

        [Required]
        [Display(Name="First Name")]
        public string FamilyMemberFirstName;

        [Required]
        [Display(Name="Last Name")]
        public string FamilyMemberLastName;


        [Display(Name = "Health Form")]
        public Nullable<bool> HealthForm;

        [Display(Name="Photo Ack")]
        public Nullable<bool> PhotoAck;

        [Display(Name="Attendance")]
        public string AttendingCode;

        [Display(Name="Comments")]
        public string Comments;

        [Display(Name="Age")]
        public int FamilyMemberAge;

        
    }
}