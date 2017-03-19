using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SNCRegistration.ViewModels
    {
    public class VolunteersReportModel
        {
        public int VolunteerID { get; set; }
        [Display(Name = "First Name")]
        public string VolunteerFirstName { get; set; }
        [Display(Name = "Last Name")]
        public string VolunteerLastName { get; set; }
        }
    }