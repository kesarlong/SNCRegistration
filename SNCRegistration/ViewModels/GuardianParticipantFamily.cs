using SNCRegistration.ViewModels.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace SNCRegistration.ViewModels
{
    public class GuardianParticipantFamily
    {
        public Guardian Guardians { get; set; }
        public Participant Participants { get; set; }
        public FamilyMember FamilyMembers { get; set; }

        //public IEnumerable<Guardian> Guardians { get; set; }
        //public IEnumerable<Participant> Participant { get; set; }

        //public IEnumerable<FamilyMember> FamilyMember { get; set; }
    }
}