using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SNCRegistration.ViewModels
    {
    public class TeeShirtCountBySizeModel
        {

        [Display(Name = "Volunteer Shirt Sizes")]
        public string VolunteerShirtSize { get; set; }

        [Display(Name = "Total")]
        public int Total { get; set; }
        
        }
    }