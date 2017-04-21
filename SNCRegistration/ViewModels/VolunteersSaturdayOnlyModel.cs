using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SNCRegistration.ViewModels
    {
    public class VolunteersSaturdayOnlyModel
        {
        public int VolunteerID { get; set; }

        [Display(Name = "Group Number")]
        public string GroupNumber { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Attending")]
        public string Attending { get; set; }
        }
    }