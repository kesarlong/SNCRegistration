
namespace SNCRegistration.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;

    public partial class Participant
    {
        public int ParticipantID { get; set; }

        [Required]
     
        [DisplayName("First Name")]
        public string ParticipantFirstName { get; set; }


        [Required]
     
        [DisplayName("Last Name")]
        public string ParticipantLastName { get; set; }

       
        [DisplayName("Age")]
        public int ParticipantAge { get; set; }

     
        [DisplayName("School")]
        public string ParticipantSchool { get; set; }

        
        //TO DO: database field size increased (Erika)
        [DisplayName("Teacher")]
        public string ParticipantTeacher { get; set; }


        [DisplayName("Classroom Scouting")]
        public bool ClassroomScouting { get; set; }

        [DisplayName("Health Form")]
        public Nullable<bool> HealthForm { get; set; }

        [DisplayName("Photo Acknowledgment")]
        public Nullable<bool> PhotoAck { get; set; }

        [DisplayName("Attendance")]
        public string AttendingCode { get; set; }


        public Nullable<int> GuardianID { get; set; }
        public string Comments { get; set; }
    }

}