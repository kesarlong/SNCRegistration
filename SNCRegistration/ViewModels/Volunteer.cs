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
    
    public partial class Volunteer
    {
        public int VolunteerID { get; set; }
        public string VolunteerFirstName { get; set; }
        public string VolunteerLastName { get; set; }
        public int VolunteerAge { get; set; }
        public int LeadContactID { get; set; }
        public bool VolunteerShirtOrder { get; set; }
        public string VolunteerShirtSize { get; set; }
        public int VolunteerAttendingCode { get; set; }
        public Nullable<bool> SaturdayDinner { get; set; }
        public string UnitChapterNumber { get; set; }
        public string Comments { get; set; }
        public string LeaderGuid { get; set; }
        public bool CheckedIn { get; set; }
        public int EventYear { get; set; }
        public int VolunteerFee { get; set; }
        public int BSType { get; set; }
    
        public virtual BSType BSType1 { get; set; }
    }
}
