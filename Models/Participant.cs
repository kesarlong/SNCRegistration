//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SNCRegistration.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Participant
    {
        public int ParticipantID { get; set; }
        public string ParticipantFirstName { get; set; }
        public string ParticipantLastName { get; set; }
        public int ParticipantAge { get; set; }
        public string ParticipantSchool { get; set; }
        public string ParticipantTeacher { get; set; }
        public bool ClassroomScouting { get; set; }
        public Nullable<bool> HealthForm { get; set; }
        public Nullable<bool> PhotoAck { get; set; }
        public string AttendingCode { get; set; }
        public Nullable<int> GuardianID { get; set; }
        public string Comments { get; set; }
    }
}