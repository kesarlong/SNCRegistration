using System;
using System.Collections;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

namespace SNCRegistration.ViewModels {

    /// <summary>
    /// Might have to delete this, from tutorial. http://openlightgroup.com/Blog/TabId/58/PostId/189/UserRolesAdministration.aspx
    /// </summary>

    public class ExpandedUserDTO {

        [Key]

        [Display(Name = "User Name")]

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        [Display(Name = "Lockout End Date Utc")]

        public DateTime? LockoutEndDateUtc { get; set; }

        public int AccessFailedCount { get; set; }

        public string PhoneNumber { get; set; }

        public IEnumerable Roles { get; set; }

    }

    public class UserRolesDTO {

        [Key]

        [Display(Name = "Role Name")]

        public string RoleName { get; set; }

    }

    public class UserRoleDTO {

        [Key]

        [Display(Name = "User Name")]

        public string UserName { get; set; }

        [Display(Name = "Role Name")]

        public string RoleName { get; set; }

    }

    public class RoleDTO {

        [Key]

        public string Id { get; set; }

        [Display(Name = "Role Name")]

        public string RoleName { get; set; }

    }

    public class UserAndRolesDTO {

        [Key]

        [Display(Name = "User Name")]

        public string UserName { get; set; }

        public List<UserAndRolesDTO> colUserRoleDTO { get; set; }

    }

}