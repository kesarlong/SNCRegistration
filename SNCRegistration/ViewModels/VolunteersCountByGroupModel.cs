using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SNCRegistration.ViewModels
    {
    public class VolunteersCountByGroupModel
        {

        [Display(Name = "Group Type")]
        public string GroupType{ get; set; }

        [Display(Name = "Group Number")]
        public string GroupNumber { get; set; }

        [Display(Name = "Total")]
        public int Total { get; set; }
        
        }
    }