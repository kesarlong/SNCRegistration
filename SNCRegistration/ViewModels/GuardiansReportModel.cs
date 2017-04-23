using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SNCRegistration.ViewModels
    {
    public class GuardiansReportModel
        {

        public int GuardianID { get; set; }
        [Display(Name = "First Name")]
        public string GuardianFirstName { get; set; }
        [Display(Name = "Last Name")]
        public string GuardianLastName { get; set; }

        [Display(Name = "Event Year")]
        public int EventYear { get; set; }

        }
    }
