using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SNCRegistration.ViewModels
    {
    public class VolunteersCountByGroupModel
        {

        [Display(Name = "Troop/Unit Chapter Number")]
        public string UnitChapterNumber { get; set; }

        [Display(Name = "Total")]
        public int Total { get; set; }
        
        }
    }