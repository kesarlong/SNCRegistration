using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SNCRegistration.ViewModels.Metadata
{
    public class Guardian_Metadata
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GuardianID;

        [Required]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string GuardianFirstName;

        [Required]
        [Display(Name = "Last Name")]
        public string GuardianLastName;

        [MaxLength(50)]
        [Display(Name = "Street Address")]
        public string GuardianAddress;

        [MaxLength(50)]
        [Display(Name = "City")]
        public string GuardianCity;

        [Display(Name = "State")]
        [MaxLength(2)]
        public string GuardianState;

        [MaxLength(10)]
        [Display(Name = "Zip")]
        public int GuardianZip;

        [Phone]
        [MaxLength(10)]
        [MinLength(10)]
        [Display(Name = "Cell Phone")]
        public string GuardianCellPhone;



        //TO DO: is 50 characters sufficient for length - Erika review
        [MaxLength(50)]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string GuardianEmail;

        [DataType(DataType.Date)]
        [Display(Name = "Date Packet Sent")]
        public Nullable<System.DateTime> PacketSentDate;

        [DataType(DataType.Date)]
        [Display(Name = "Paperwork Received")]
        public Nullable<System.DateTime> ReceiptDate;


        [Display(Name = "Health Form")]
        public Nullable<bool> HealthForm;


        [Display(Name = "Photo Acknowledgment")]
        public Nullable<bool> PhotoAck;

        [Required]
        [Display(Name = "Tent Required")]
        public bool Tent;

        [Required]
        [Display(Name = "Attending")]
        public int AttendingCode;

        [MaxLength(50)]

        //TO DO: this needs to be a larger field
        [Display(Name = "Comments")]
        public string Comments;

        [Required]
        [Display(Name = "Relationship")]
        public string Relationship;

        [Required]
        public string EventYear;

        [Display(Name = "Checked In?")]
        public bool? CheckedIn;

    }

    public class Participant_Metadata
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParticipantID;

        [Required]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string ParticipantFirstName;


        [Required]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string ParticipantLastName;

        [Required]
        [Display(Name = "Age")]
        public int ParticipantAge;

        //School is optional input field
        [MaxLength(50)]
        [Display(Name = "School")]
        public string ParticipantSchool;

        [MaxLength(50)]
        [Display(Name = "Teacher")]
        public string ParticipantTeacher;

        [Required]
        [Display(Name = "Classroom Scouting")]
        public bool ClassroomScouting;

        [Display(Name = "Health Form")]
        public bool? HealthForm;

        [Display(Name = "Checked In?")]
        public bool? CheckedIn;

        [Display(Name = "Photo Acknowledgment")]
        public bool? PhotoAck;

        [Required]
        [Display(Name = "Attendance")]
        public int AttendingCode;

        [Required]
        [Display(Name = "Returning")]
        public bool? Returning;


        [Required]
        public string EventYear;


    }

    public class FamilyMember_Metadata
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FamilyMemberID;

        [Required]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FamilyMemberFirstName;

        [Required]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string FamilyMemberLastName;


        [Display(Name = "Health Form")]
        public Nullable<bool> HealthForm;

        [Display(Name = "Photo Acknowledgment")]
        public Nullable<bool> PhotoAck;

        [Required]
        [Display(Name = "Attendance")]
        public string AttendingCode;

        [MaxLength(50)]
        //to do: this needs to be a larger field
        [Display(Name = "Comments")]
        public string Comments;

        [Required]
        [Display(Name = "Age")]
        public int FamilyMemberAge;

        [Required]
        public string EventYear;

        [Display(Name = "Checked In?")]
        public bool? CheckedIn;

    }

    public class LeadContact_Metadata
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeadContactID;

        [Required]
        [MinLength(2)]
        [Display(Name = "First Name")]
        public string LeadContactFirstName;

        [Required]
        [MinLength(2)]
        [Display(Name = "Last Name")]
        public string LeadContactLastName;

        [Required]
        [MinLength(2)]
        [Display(Name = "Street Address")]
        public string LeadContactAddress;

        [Required]
        [MinLength(2)]
        [Display(Name = "City")]
        public string LeadContactCity;


        [Display(Name = "State")]
        public string LeadContactState;

        [Required]
        [RegularExpression(@"^(?!00000)[0-9]{5,5}$", ErrorMessage = "Zip Code should be five numbers long")]
        [Display(Name = "Zip Code")]
        public string LeadContactZip;

        [Required]
        [RegularExpression(@"^(?!0000000000)[0-9]{10,10}$", ErrorMessage = "Phone number should be ten numbers long")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string LeadContactCellPhone;

        [Required]
        [MinLength(7)]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string LeadContactEmail;

        [Required]
        [Display(Name = "T-Shirt Order")]
        public bool LeadContactShirtOrder;


        [Display(Name = "T-Shirt Size")]
        public string LeadContactShirtSize;

        [Required]
        [Display(Name = "Troop/Chapter/Unit")]
        public string BSType;

        [Required]
        [MinLength(1)]
        [MaxLength(10)]
        [Display(Name = "Troop/Chapter/Unit #")]
        public string UnitChapterNumber;

        [Required]
        [Display(Name = "Attending")]
        public string VolunteerAttendingCode;

        [Required]
        [Display(Name = "Saturday Dinner?")]
        public bool SaturdayDinner;


        [Display(Name = "Booth Name")]
        public string Booth;

        [MaxLength(50)]
        [Display(Name = "Comments")]
        public string Comments;


        public Decimal TotalFee;


        [Required]
        public string EventYear;

        [Display(Name = "Checked In?")]
        public bool? CheckedIn;

    }
    public class Volunteer_Metadata
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VolunteerID;

        [Required]
        [MinLength(2)]
        [Display(Name = "First Name")]
        public string VolunteerFirstName;

        [Required]
        [MinLength(2)]
        [Display(Name = "Last Name")]
        public string VolunteerLastName;

        [Required]
        [Display(Name = "Age")]
        public string VolunteerAge;

        [Required]
        [Display(Name = "T-Shirt Order")]
        public Nullable<bool> VolunteerShirtOrder;

        [Display(Name = "T-Shirt Size")]
        public string VolunteerShirtSize;

        [Required]
        [Display(Name = "Attending")]
        public string VolunteerAttendingCode;

        [Required]
        [Display(Name = "Saturday Dinner?")]
        public Nullable<bool> SaturdayDinner;

        [Required]
        [MinLength(1)]
        [MaxLength(10)]
        [Display(Name = "Troop/Chapter/Unit Number")]
        public string UnitChapterNumber;

        [MaxLength(50)]
        [Display(Name = "Comments")]
        public string Comments;

        public Nullable<int> LeadContactID;


        [Required]
        public string EventYear;

        [Display(Name = "Checked In?")]
        public bool? CheckedIn;

        //Messing things up with LeadContact/Details
        //[Display(Name = "Number of people in tent")]
        //public int NumberInTent;
    }

}