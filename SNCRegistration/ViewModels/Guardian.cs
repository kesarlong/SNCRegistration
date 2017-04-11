
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
    
public partial class Guardian
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Guardian()
    {

        this.FamilyMembers = new HashSet<FamilyMember>();

        this.Participants = new HashSet<Participant>();

    }


    public int GuardianID { get; set; }

    public string GuardianFirstName { get; set; }

    public string GuardianLastName { get; set; }

    public int Relationship { get; set; }

    public string GuardianAddress { get; set; }

    public string GuardianCity { get; set; }

    public string GuardianState { get; set; }

    public string GuardianZip { get; set; }

    public string GuardianCellPhone { get; set; }

    public string GuardianEmail { get; set; }

    public Nullable<bool> HealthForm { get; set; }

    public Nullable<bool> PhotoAck { get; set; }

    public Nullable<bool> Tent { get; set; }

    public int AttendingCode { get; set; }

    public string Comments { get; set; }

    public string GuardianGuid { get; set; }

    public bool CheckedIn { get; set; }

    public int EventYear { get; set; }

    public Nullable<int> NumberInTent { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<FamilyMember> FamilyMembers { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Participant> Participants { get; set; }

}

}
