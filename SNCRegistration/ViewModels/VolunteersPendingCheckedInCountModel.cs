﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SNCRegistration.ViewModels
    {
    public class VolunteersPendingCheckedInCountModel
        {
        [Display(Name = "UnitChapterNumber")]
        public string UnitChapterNumber { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Checked In")]
        public string CheckedIn { get; set; }
        }
    }