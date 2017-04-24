using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SNCRegistration.ViewModels
{
    public class FutureEventsModel
    {
        public int LeadContactID { get; set; }
        [Display(Name = "First Name")]
        public string LeadContactFirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LeadContactLastName { get; set; }
        [Display(Name = "Email")]
        public string LeadContactEmail { get; set; }
        [Display(Name = "Group")]
        public string BSDescription { get; set; }
        [Display(Name = "Group Number")]
        public string UnitChapterNumber { get; set; }
    }
}