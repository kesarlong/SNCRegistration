using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SNCRegistration.ViewModels
    {
    public class ParticipantsCoverSheetModel
        {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Relationship")]
        public string Relationship { get; set; }

        [Display(Name = "Age")]
        public string Age { get; set; }

        [Display(Name = "Check In")]
        public string CheckedIn { get; set; }

        [Display(Name = "Cell Phone")]
        public string CellPhone { get; set; }

        [Display(Name = "Health Form")]
        public string HealthForm { get; set; }

        [Display(Name = "Photo Ack")]
        public string PhotoAck { get; set; }

        [Display(Name = "Attending")]
        public string Attending { get; set; }

        [Display(Name = "Comments")]
        public string Comments { get; set; }

        [Display(Name = "Event Year")]
        public int EventYear { get; set; }

        }
    }
