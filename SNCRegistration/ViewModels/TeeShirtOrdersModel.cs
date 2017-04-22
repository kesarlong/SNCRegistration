using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SNCRegistration.ViewModels
    {
    public class TeeShirtOrdersModel
        {

        [Display(Name = "Troop/Unit Number")]
        public string GroupNumber { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Shirt Size")]
        public string ShirtSize { get; set; }

        }
    }