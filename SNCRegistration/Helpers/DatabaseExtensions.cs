using SNCRegistration.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SNCRegistration.Helpers
{
    public static class DatbaseExtensions
    {
        public static decimal ComputeTotal(this SNCRegistrationEntities db, LeadContact leadContact)
        {
            var volunteercount = db.Volunteers.Count(x => x.LeadContactID == leadContact.LeadContactID);
            var rate = new decimal(10.00); //compute based on group
            leadContact.TotalFee = (volunteercount + 1) * rate;
            db.SaveChanges();
            return leadContact.TotalFee ?? 0;
        }

        public static decimal ComputeTotal(this SNCRegistrationEntities db, int LeaderID)
        {
            var leadContact = db.LeadContacts.Single(x => x.LeadContactID == LeaderID);
            return db.ComputeTotal(leadContact);
        }
    }
}