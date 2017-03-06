using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

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

        [Display(Name = "State")]
        [MaxLength(2)]
        public string GuardianState;

        [MaxLength(10)]
        [Display(Name = "Zip")]
        //TO DO: review field type (should be string as it is not used numerically) - Erika review (SP-245 created 11/21/16)
        public int GuardianZip;

        [MaxLength(10)]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage ="Please enter phone number as numbers only.")]
        [Display(Name = "Cell Phone")]
        //[DisplayFormat(DataFormatString ="{0;##########}")]
        //TO DO: FORMAT ON SCREEN FOR DATA INPUT -- MENTOR INPUT BEING REQUESTED 
            [DisplayFormat(DataFormatString = "{0:(###) ###-####}", ApplyFormatInEditMode = true)]
        public string GuardianCellPhone;



        //TO DO: is 50 characters sufficient for length - Erika review
        [MaxLength(50)]
        [Display(Name="Email Address")]
        [DataType(DataType.EmailAddress)]
        public string GuardianEmail;

        [DataType(DataType.Date)]
        [Display(Name = "Date Packet Sent")]
        public Nullable<System.DateTime> PacketSentDate;

        [DataType(DataType.Date)]
        [Display(Name = "Paperwork Received")]
        public Nullable<System.DateTime> ReceiptDate;


        [Display(Name="Health Form Received")]
        public Nullable<bool> HealthForm;


        [Display(Name = "Photo Ack Received")]
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
        [Display(Name = "Relationship to Participant")]
        public string Relationship;

        [Required]
        public string EventYear;
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

        [Required]
        [Display(Name = "Age")]
        public int ParticipantAge;

        //School is optional input field
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
        [Display(Name="First Name")]
        public string FamilyMemberFirstName;

        [Required]
        [Display(Name="Last Name")]
        public string FamilyMemberLastName;


        [Display(Name = "Health Form")]
        public Nullable<bool> HealthForm;

        [Display(Name="Photo Ack")]
        public Nullable<bool> PhotoAck;

        [Required]
        [Display(Name="Attendance")]
        public string AttendingCode;

        [MaxLength(50)]
        //to do: this needs to be a larger field
        [Display(Name="Comments")]
        public string Comments;

        [Required]
        [Display(Name="Age")]
        public int FamilyMemberAge;
        
        [Required]
        public string EventYear;

    }

    public class LeadContact_Metadata
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeadContactID;

        [Required]
        [Display(Name = "First Name")]
        public string LeadContactFirstName;

        [Required]
        [Display(Name = "Last Name")]
        public string LeadContactLastName;


        [Display(Name = "Street Address")]
        public string LeadContactAddress;

        [Display(Name = "City")]
        public string LeadContactCity;


        [Display(Name = "State")]
        public string LeadContactState;


        [Display(Name = "Zip Code")]
        public string LeadContactZip ;


        [Display(Name = "Cell Phone")]
        [DataType(DataType.PhoneNumber)]
        public string LeadContactCellPhone ;


        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string LeadContactEmail ;

        [Required]
        [Display(Name = "T-Shirt Order")]
        public bool LeadContactShirtOrder ;


        [Display(Name = "T-Shirt Size")]
        public string LeadContactShirtSize ;

        [Required]
        [Display(Name = "Troop/Chapter/Unit")]
        public string BSType ;

        [Required]
        [Display(Name = "Troop/Chapter/Unit Number")]
        public string UnitChapterNumber ;
        
        [Required]
        [Display(Name = "Attending")]
        public string VolunteerAttendingCode ;

        [Required]
        [Display(Name = "Saturday Dinner?")]
        public bool SaturdayDinner ;


        [Display(Name = "Booth Name")]
        public string Booth ;

        [MaxLength(50)]
        [Display(Name = "Comments")]
        public string Comments ;


        public Decimal TotalFee ;


        [Required]
        public string EventYear;

    }
        public class Volunteer_Metadata
        {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VolunteerID ;

        [Required]
        [Display(Name = "First Name")]
        public string VolunteerFirstName ;

        [Required]
        [Display(Name = "Last Name")]
        public string VolunteerLastName ;

        [Required]
        [Display(Name = "Age")]
        public string VolunteerAge ;

        [Required]
        [Display(Name = "T-Shirt Order")]
        public Nullable<bool> VolunteerShirtOrder ;

        [Display(Name = "T-Shirt Size")]
        public string VolunteerShirtSize ;

        [Required]
        [Display(Name = "Attending")]
        public string VolunteerAttendingCode ;

        [Required]
        [Display(Name = "Saturday Dinner?")]
        public Nullable<bool> SaturdayDinner ;

        [Required]
        [Display(Name = "Troop/Chapter/Unit Number")]
        public string UnitChapterNumber ;

        [MaxLength(50)]
        [Display(Name = "Comments")]
        public string Comments ;

        public Nullable<int> LeadContactID ;


        [Required]
        public string EventYear;
    }

    }