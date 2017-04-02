using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SNCRegistration.ViewModels
    {
    public class TeeShirtOrdersModel
        {

        [Display(Name = "Troop/Unit Number")]
        public string UnitChapterNumber { get; set; }

        [Display(Name = "Volunteer First Name")]
        public string VolunteerFirstName { get; set; }

        [Display(Name = "Volunteer Last Name")]
        public string VolunteerLastName { get; set; }

        [Display(Name = "Lead First Name")]
        public string LeadContactFirstName { get; set; }
        [Display(Name = "Lead Last Name")]
        public string LeadContactLastName { get; set; }      

        [Display(Name = "Shirt Order")]
        public string VolunteerShirtOrder { get; set; }

        [Display(Name = "Shirt Size")]
        public string VolunteerShirtSize { get; set; }

        }
    }