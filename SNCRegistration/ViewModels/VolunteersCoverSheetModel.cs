using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SNCRegistration.ViewModels
    {
    public class VolunteersCoverSheetModel
        {

        [Display(Name = "Year")]
        public string EventYear { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Check In")]
        public string CheckedIn { get; set; }

        [Display(Name = "Cell Phone")]
        public string CellPhone { get; set; }

        [Display(Name = "Attending")]
        public string Attending { get; set; }

        [Display(Name = "Booth")]
        public string Booth { get; set; }

        [Display(Name = "ShirtOrder")]
        public int ShirtOrder { get; set; }

        [Display(Name = "ShirtSize")]
        public string ShirtSize { get; set; }


        }
    }
