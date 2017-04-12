using SNCRegistration.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SNCRegistration.Helpers
{
    public static class DatbaseExtensions
    {
        public static decimal ComputeTotal(this SNCRegistrationEntities db, LeadContact leadContact)
        {
           

            var volSum = db.Volunteers.Where(x => x.LeadContactID == leadContact.LeadContactID).Sum(x=>x.BSType1.BSFee);
            var leadFee = db.BSTypes.Single(x => x.BSTypeID == leadContact.BSType).BSFee;
            leadContact.TotalFee = volSum + leadFee;
            db.SaveChanges();
            return leadContact.TotalFee ?? 0;
        }

        public static decimal ComputeTotal(this SNCRegistrationEntities db, int LeaderID)
        {
            var leadContact = db.LeadContacts.Single(x => x.LeadContactID == LeaderID);
            return db.ComputeTotal(leadContact);
        }

        public static string GetVolunteerList(this SNCRegistrationEntities db, int LeaderID)
        {
            var volList = db.Volunteers.Where(x => x.LeadContactID == LeaderID).Select(x => new { x.VolunteerFirstName , x.VolunteerLastName});
            var sb = new StringBuilder();
            foreach (var vol in volList)
            {
                sb.Append(vol.VolunteerFirstName + " " + vol.VolunteerLastName + "<br />");
                
            }

            return sb.ToString(); 
        }
    }
}