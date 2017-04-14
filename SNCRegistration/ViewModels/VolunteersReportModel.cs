using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SNCRegistration.ViewModels
    {
    public class VolunteersReportModel
        {
        [Display(Name = "UnitChapterNumber")]
        public string UnitChapterNumber{ get; set; }

        [Display(Name = "First Name")]
        public string VolunteerFirstName { get; set; }

        [Display(Name = "Last Name")]
        public string VolunteerLastName { get; set; }

        [Display(Name = "Lead Contact First Name")]
        public string LeadContactFirstName { get; set; }

        [Display(Name = "Lead Contact First Name")]
        public string LeadContactLastName { get; set; }

        [Display(Name = "Shirt Size")]
        public string VolunteerShirtSize { get; set; }

        [Display(Name = "Attending")]
        public string Description { get; set; }

        [Display(Name = "Checked In")]
        public string CheckedIn{ get; set; }
        }
    }