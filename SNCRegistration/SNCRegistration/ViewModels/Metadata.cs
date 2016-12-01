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


        [Display(Name = "Cell Phone")]
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


        [Display(Name="Health Form Received")]
        public Nullable<bool> HealthForm;


        [Display(Name = "Photo Ack Received")]
        public Nullable<bool> PhotoAck;

        [Required]
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

        [Required]
        [Display(Name = "Age")]
        public string ParticipantAge;

        //TO DO: make field not required
        [Required]
        [Display(Name = "School")]
        public string ParticipantSchool;


        //TO DO: database field size increased (Erika)
        //TO DO: make field not required
        [Required]
        [Display(Name = "Teacher")]
        public string ParticipantTeacher;

        [Required]
        [Display(Name = "Classroom Scouting")]
        public bool ClassroomScouting;

        [Display(Name = "Health Form")]
        public bool? HealthForm;

        [Display(Name = "Photo Acknowledgment")]
        public bool? PhotoAck;

        [Required]
        [Display(Name = "Attendance")]
        public string AttendingCode;

        [Required]
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

        [Required]
        [Display(Name="Attendance")]
        public string AttendingCode;

        [Display(Name="Comments")]
        public string Comments;

        [Required]
        [Display(Name="Age")]
        public int FamilyMemberAge;

        
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


        [Display(Name = "T-Shirt Order")]
        public bool LeadContactShirtOrder ;


        [Display(Name = "T-Shirt Size")]
        public string LeadContactShirtSize ;


        [Display(Name = "Troop/Chapter/Unit")]
        public string BSType ;


        [Display(Name = "Troop/Chapter/Unit Number")]
        public string UnitChapterNumber ;


        [Display(Name = "Attending")]
        public string VolunteerAttendingCode ;


        [Display(Name = "Saturday Dinner?")]
        public bool SaturdayDinner ;


        [Display(Name = "Booth Name")]
        public string Booth ;

        [Display(Name = "Comments")]
        public string Comments ;


        public Decimal TotalFee ;

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

     
        [Display(Name = "Age")]
        public string VolunteerAge ;

        [Display(Name = "T-Shirt Order")]
        public Nullable<bool> VolunteerShirtOrder ;

        [Display(Name = "T-Shirt Size")]
        public string VolunteerShirtSize ;


        [Display(Name = "Attending")]
        public string VolunteerAttendingCode ;


        [Display(Name = "Saturday Dinner?")]
        public Nullable<bool> SaturdayDinner ;


        [Display(Name = "Troop/Chapter/Unit Number")]
        public string UnitChapterNumber ;


        [Display(Name = "Comments")]
        public string Comments ;

        public Nullable<int> LeadContactID ;


    }
    }