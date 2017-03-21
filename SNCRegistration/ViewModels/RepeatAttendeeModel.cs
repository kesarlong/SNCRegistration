using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SNCRegistration.ViewModels
    {
    public class RepeatAttendeeReport
        {
        public int ParticipantID { get; set; }
        [Display(Name = "First Name")]
        public string ParticipantFirstName { get; set; }
        [Display(Name = "Last Name")]
        public string ParticipantLastName { get; set; }
        [Display(Name = "Repeat Attendee")]
        public string Returning { get; set; }
        [Display(Name = "Attending")]
        public string Description{ get; set; }
        }
    }