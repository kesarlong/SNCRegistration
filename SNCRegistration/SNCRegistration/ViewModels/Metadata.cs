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


        [Display(Name = "Classroom Scouting")]
        public bool ClassroomScouting;

        [Display(Name = "Health Form")]
        public bool? HealthForm;

        [Display(Name = "Photo Acknowledgment")]
        public bool? PhotoAck;

        [Required]
        [Display(Name = "Attendance")]
        public string AttendingCode;

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

        [Display(Name="Age")]
        public int FamilyMemberAge;

        
    }

    public class LeadContact_Metadata
    {
  
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeadContactID { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string LeadContactFirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LeadContactLastName { get; set; }

        [Required]
        [DisplayName("Street Address")]
        public string LeadContactAddress { get; set; }

        [Required]
        [DisplayName("City")]
        public string LeadContactCity { get; set; }

        [Required]
        public string LeadContactState { get; set; }

        [Required]
        [DisplayName("Zip Code")]
        public string LeadContactZip { get; set; }

        [Required]
        [DisplayName("Cell Phone")]
        [DataType(DataType.PhoneNumber)]
        public string LeadContactCellPhone { get; set; }

        [Required]
        [DisplayName("Email Address")]
        [DataType(DataType.EmailAddress)]
        public string LeadContactEmail { get; set; }

        [Required]
        [DisplayName("T-Shirt Order")]
        public bool LeadContactShirtOrder { get; set; }

        [Required]
        [DisplayName("T-Shirt Size")]
        public string LeadContactShirtSize { get; set; }

        [Required]
        [DisplayName("Troop/Chapter/Unit")]
        public string BSType { get; set; }

        [Required]
        [DisplayName("Troop/Chapter/Unit Number")]
        public string UnitChapterNumber { get; set; }

        [Required]
        [DisplayName("Attending")]
        public string VolunteerAttendingCode { get; set; }

        [Required]
        [DisplayName("Saturday Dinner?")]
        public bool SaturdayDinner { get; set; }

        [Required]
        [DisplayName("Booth Name")]
        public string Booth { get; set; }

        [Required]
        [DisplayName("Comments")]
        public string Comments { get; set; }


        public Decimal TotalFee { get; set; }

    }
        public class Volunteer_Metadata
        {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VolunteerID { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string VolunteerFirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string VolunteerLastName { get; set; }

        [DisplayName("Age")]
        public string VolunteerAge { get; set; }

        [DisplayName("T-Shirt Order")]
        public Nullable<bool> VolunteerShirtOrder { get; set; }

        [DisplayName("T-Shirt Size")]
        public string VolunteerShirtSize { get; set; }

        [DisplayName("Attending")]
        public string VolunteerAttendingCode { get; set; }

        [DisplayName("Saturday Dinner?")]
        public Nullable<bool> SaturdayDinner { get; set; }

        [DisplayName("Troop/Chapter/Unit Number")]
        public string UnitChapterNumber { get; set; }

        [DisplayName("Comments")]
        public string Comments { get; set; }

        public Nullable<int> LeadContactID { get; set; }

        public virtual Volunteer Volunteers1 { get; set; }

        public virtual Volunteer Volunteer1 { get; set; }

    }
    }