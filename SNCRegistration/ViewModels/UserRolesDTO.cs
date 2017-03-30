using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SNCRegistration.ViewModels {
    public class ExpandedUserDTO {
        [Key]
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long and no longer than {1} characters maximum.", MinimumLength = 4)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at maximum {2} characters long.")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required (ErrorMessage = "Password must be at least 6 characters, contain one upper case, one lower case, and one numerical digit.")]
        public string Password { get; set; }
        [Display(Name = "Lockout End Date Utc")]
        public DateTime? LockoutEndDateUtc { get; set; }
        public int AccessFailedCount { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<UserRolesDTO> Roles { get; set; }
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
        public List<UserRoleDTO> colUserRoleDTO { get; set; }
    }
}