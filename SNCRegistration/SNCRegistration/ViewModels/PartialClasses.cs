using SNCRegistration.ViewModels.Metadata;
using System.ComponentModel.DataAnnotations;

namespace SNCRegistration.ViewModels
{

    [MetadataType(typeof(Guardian_Metadata))]
    public partial class Guardian
    {

    }

    [MetadataType(typeof(Participant_Metadata))]
    public partial class Participant
    {

    }

    [MetadataType(typeof(FamilyMember_Metadata))]
    public partial class FamilyMember
    {

    }

}