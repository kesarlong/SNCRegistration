using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SNCRegistration.ViewModels
    {
    public class TeeShirtOrdersModel
        {
        [Display(Name = "First Name")]
        public string LeadContactFirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LeadContactLastName { get; set; }

        [Display(Name = "Shirt Order")]
        public string LeadContactShirtOrder { get; set; }

        [Display(Name = "Shirt Size")]
        public string LeadContactShirtSize { get; set; }

        }
    }