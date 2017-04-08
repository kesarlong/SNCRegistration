using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SNCRegistration.ViewModels
    {
    public class PeopleCheckedInCountModel
        {
        [Display(Name = "Unit Number")]
        public string UnitChapterNumber { get; set; }
        [Display(Name = "Registrant")]
        public string Registrant { get; set; }
        [Display(Name = "First Name")]
        public string ParticipantFirstName { get; set; }
        [Display(Name = "Last Name")]
        public string ParticipantLastName { get; set; }
        [Display(Name = "Checked In")]
        public string CheckedIn { get; set; }
        }
    }