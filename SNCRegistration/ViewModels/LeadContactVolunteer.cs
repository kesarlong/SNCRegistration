using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SNCRegistration.ViewModels
{
    public class LeadContactVolunteer {

        public LeadContact leadContact { get; set; }
        public Volunteer volunteer { get; set; }


        public IEnumerable<Volunteer> volunteers { get; set; }
        public IEnumerable<LeadContact> leadcontacts { get; set; }

    }
}