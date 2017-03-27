using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SNCRegistration.ViewModels
{
    public class GuardianParticipantFamily
    {

        public Guardian guardian { get; set; }
        public Participant participant { get; set; }
        public FamilyMember familymember { get; set; }

        public IEnumerable<Guardian> guardians { get; set; }
        public IEnumerable<Participant> participants { get; set; }
        public IEnumerable<FamilyMember> familymembers { get; set; }

        public IEnumerable<Participant> relatedparticipants { get; set; }


    }
}