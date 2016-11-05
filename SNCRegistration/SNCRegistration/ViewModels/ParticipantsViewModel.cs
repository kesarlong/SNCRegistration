using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SNCRegistration.ViewModels
{
    public class ParticipantsViewModel
    {
        //private readonly EntityFrameworkContext _dbContext = new EntityFrameworkContext();

        public int ParticipantID { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("First Name")]    
        public string ParticipantFirstName { get; set; }


        [Required]
        [MaxLength(50)]
        [DisplayName("Last Name")]
        public string ParticipantLastName { get; set; }

        [MaxLength(4)]
        [DisplayName("Age")]
        public int ParticipantAge { get; set; }

        [MaxLength(50)]
        [DisplayName("School")]
        public string ParticipantSchool { get; set; }

        [MaxLength(10)]
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