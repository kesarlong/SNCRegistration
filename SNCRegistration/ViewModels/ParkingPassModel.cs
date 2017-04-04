using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SNCRegistration.ViewModels
{
    public class ParkingPassModel
    {
        public int GuardianID { get; set; }
        [Display(Name = "First Name")]
        public string GuardianFirstName { get; set; }
        [Display(Name = "Last Name")]
        public string GuardianLastName { get; set; }
        [Display(Name = "Cell Phone")]
        public string GuardianCellPhone { get; set; }
        [Display(Name = "Event Year")]
        public int EventYear { get; set; }
        }
    }