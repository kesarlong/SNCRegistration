//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SNCRegistration.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class LeadContact
    {
        public int LeadContactID { get; set; }

        public int BSType { get; set; }
        [Required]
        [MinLength(1)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Must be numeric")]
        public string UnitChapterNumber { get; set; }

        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string LeadContactFirstName { get; set; }

        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string LeadContactLastName { get; set; }

        [Required]
        [MinLength(2)]
        public string LeadContactAddress { get; set; }

        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string LeadContactCity { get; set; }

        public string LeadContactState { get; set; }

        [Required]
        [MaxLength(5)]
        [MinLength(5)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Zip must be numeric")]
        public string LeadContactZip { get; set; }

        [Required]
        [MaxLength(10)]
        [MinLength(10)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone must be numeric")]
        public string LeadContactCellPhone { get; set; }

        [Required]
        [MinLength(7)]
        public string LeadContactEmail { get; set; }

        public int VolunteerAttendingCode { get; set; }
        public Nullable<bool> SaturdayDinner { get; set; }
        public Nullable<decimal> TotalFee { get; set; }
        public string Booth { get; set; }
        public string Comments { get; set; }
        public bool LeadContactShirtOrder { get; set; }
        public string LeadContactShirtSize { get; set; }
        public string LeaderGuid { get; set; }
        public bool CheckedIn { get; set; }
        public int EventYear { get; set; }
        public bool Marketing { get; set; }    
        public virtual Event Event { get; set; }
    }
}
