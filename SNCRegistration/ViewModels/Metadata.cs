using System;
using System.Collections.Generic;
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
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string GuardianLastName;

        [MaxLength(50)]
        [Display(Name = "Street Address (Optional)")]
        public string GuardianAddress;

        [MaxLength(50)]
        [Display(Name = "City (Optional)")]
        public string GuardianCity;

        [Display(Name = "State")]
        [MaxLength(2)]
        public string GuardianState;

        [MaxLength(10)]
        [MinLength(5)]
        [Display(Name = "Zip (Optional)")]
        public int GuardianZip;

        [Phone]
        [MaxLength(10)]
        [MinLength(10)]
        [Required]
        [Display(Name = "Cell Phone")]
        public string GuardianCellPhone;


        [MaxLength(50)]
        [Required]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string GuardianEmail;
        

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

        [Required]
        [Display(Name = "Relationship")]
        public int Relationship;

        [Required]
        public string EventYear;

        [Display(Name = "Checked In?")]
        public bool? CheckedIn;

        [MaxLength(200)]
        [Display(Name = "Comments (Optional)")]
        public string Comments;
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
        [Display(Name = "School (Optional)")]
        public string ParticipantSchool;

        [MaxLength(50)]
        [Display(Name = "Teacher (Optional)")]
        public string ParticipantTeacher;

        [Required]
        [Display(Name = "Classroom Scouting")]
        public bool ClassroomScouting;

        [Display(Name = "Health Form")]
        public bool HealthForm;

        [Display(Name = "Checked In?")]
        public bool CheckedIn;

        [Display(Name = "Photo Acknowledgment")]
        public bool? PhotoAck;

        [Required]
        [Display(Name = "Attendance")]
        public int AttendingCode;

        [Required]
        [Display(Name = "Returning")]
        public bool? Returning;

        [Display(Name = "Comments (Optional)")]
        [MaxLength(200)]
        public string Comments;

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

        [MaxLength(200)]
        [Display(Name = "Comments (Optional)")]
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
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string LeadContactFirstName;

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LeadContactLastName;

        //[Required]
        [MinLength(2)]
        [MaxLength(50)]
        [Display(Name = "Street Address (Optional)")]
        public string LeadContactAddress;

        //[Required]
        [MinLength(2)]
        [MaxLength(50)]
        [Display(Name = "City (Optional)")]
        public string LeadContactCity;


        [Display(Name = "State")]
        [MaxLength(2)]
        public string LeadContactState;

        //[Required]
        [RegularExpression(@"^(?!00000)[0-9]{5,5}$", ErrorMessage = "Zip Code should be five numbers long")]
        [Display(Name = "Zip Code (Optional)")]
        public string LeadContactZip;

        [Required]
        [RegularExpression(@"^(?!0000000000)[0-9]{10,10}$", ErrorMessage = "Phone number should be ten numbers long")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string LeadContactCellPhone;

        [Required]
        [MinLength(7)]
        [MaxLength(100)]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string LeadContactEmail;

        [Required]
        [Display(Name = "Do you want to order an event t-shirt?")]
        public bool LeadContactShirtOrder;

        [Required]
        [Display(Name = "T-Shirt Size")]
        public string LeadContactShirtSize;

        [Required]
        [Display(Name = "Group Type")]
        public string BSType;

        [MinLength(1)]
        [MaxLength(10)]
        [Display(Name = "Troop/Chapter/Unit #")]
        public string UnitChapterNumber;

        [Required]
        [Display(Name = " Days Attending")]
        public string VolunteerAttendingCode;

        [Display(Name = "Are you joining us for Saturday dinner?")]
        public bool SaturdayDinner;

        [MaxLength(50)]
        [Display(Name = "Booth Name (Optional)")]
        public string Booth;

        [MaxLength(200)]
        [Display(Name = "Comments (Optional)")]
        public string Comments;


        public Decimal TotalFee;


        [Required]
        public string EventYear;


        [Display(Name = "Checked In?")]
        public bool? CheckedIn;

        [Required]
        [Display(Name = "Marketing option")]
        public bool? Marketing;

    }
    public class Volunteer_Metadata
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VolunteerID;

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string VolunteerFirstName;

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string VolunteerLastName;

        [Required]
        [Display(Name = "Age")]
        public string VolunteerAge;

        [Required]
        [Display(Name = "T-Shirt Order")]
        public Nullable<bool> VolunteerShirtOrder;

        [Required]
        [Display(Name = "T-Shirt Size")]
        public string VolunteerShirtSize;

        [Required]
        [Display(Name = "Days Attending")]
        public string VolunteerAttendingCode;


        [Display(Name = "Are you joining us for Saturday dinner?")]
        public Nullable<bool> SaturdayDinner;

        [MinLength(1)]
        [MaxLength(10)]
        [Display(Name = "Troop/Chapter/Unit Number")]
        public string UnitChapterNumber;

        [MaxLength(200)]
        [Display(Name = "Comments (Optional)")]
        public string Comments;

        public int LeadContactID;


        [Required]
        public string EventYear;

        [Display(Name = "Checked In?")]
        public bool? CheckedIn;

        //Messing things up with LeadContact/Details
        //[Display(Name = "Number of people in tent")]
        //public int NumberInTent;
    }

}