using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SNCRegistration.ViewModels
    {
    public class PendingRegistrationsReport
        {
        [Display(Name = "First Name")]
        public string ParticipantFirstName { get; set; }
        [Display(Name = "Last Name")]
        public string ParticipantLastName { get; set; }
        [Display(Name = "Health Form")]
        public string HealthForm { get; set; }
        [Display(Name = "Photo Acknowledgement")]
        public string PhotoAck { get; set; }
        }
    }