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
       
        [Display(Name = "Booth")]
        public string Booth { get; set; }
        }
    }