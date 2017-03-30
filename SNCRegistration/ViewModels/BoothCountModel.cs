using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SNCRegistration.ViewModels
    {
    public class BoothCountModel
        {
       
        public int LeadContactID { get; set; }
        [Display(Name = "Booth")]
        public string Booth { get; set; }
        [Display(Name = "First Name")]
        public string LeadContactFirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LeadContactLastName{ get; set; }
        [Display(Name = "Unit/Chapter #")]
        public string UnitChapterNumber { get; set; }
        }
    }