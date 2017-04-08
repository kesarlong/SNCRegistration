using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SNCRegistration.ViewModels
    {
    public class FamilyFriendsReportModel
        {
        public int FamilyMemberID { get; set; }
        [Display(Name = "First Name")]
        public string FamilyMemberFirstName { get; set; }
        [Display(Name = "Last Name")]
        public string FamilyMemberLastName { get; set; }
        }
    }