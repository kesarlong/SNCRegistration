using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using SNCRegistration.ViewModels;
using PagedList;



namespace SNCRegistration.Controllers {
    public class AdminController : Controller {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;


        public ActionResult Index() {
            return View();
        }


        // GET: /ManageUsers/
        [Authorize(Roles = "SystemAdmin, FullAdmin")]
        public ActionResult ManageUsers(string searchStringUserNameOrEmail, string currentFilter, int? page) {
            try {
                int intPage = 1;
                int intPageSize = 5;
                int intTotalPageCount = 0;

                if (searchStringUserNameOrEmail != null) {
                    intPage = 1;
                }
                else {
                    if (currentFilter != null) {
                        searchStringUserNameOrEmail = currentFilter;
                        intPage = page ?? 1;
                    }
                    else {
                        searchStringUserNameOrEmail = "";
                        intPage = page ?? 1;
                    }
                }

                ViewBag.CurrentFilter = searchStringUserNameOrEmail;

                List<ExpandedUserDTO> col_UserDTO = new List<ExpandedUserDTO>();
                int intSkip = (intPage - 1) * intPageSize;

                intTotalPageCount = UserManager.Users
                    .Where(x => x.UserName.Contains(searchStringUserNameOrEmail))
                    .Count();

                var result = UserManager.Users
                    .Where(x => x.UserName.Contains(searchStringUserNameOrEmail))
                    .OrderBy(x => x.UserName)
                    .Skip(intSkip)
                    .Take(intPageSize)
                    .ToList();

                foreach (var item in result) {
                    ExpandedUserDTO objUserDTO = new ExpandedUserDTO();

                    objUserDTO.UserName = item.UserName;
                    objUserDTO.Email = item.Email;
                    objUserDTO.LockoutEndDateUtc = item.LockoutEndDateUtc;

                    col_UserDTO.Add(objUserDTO);
                }

                // Set the number of pages
                var _UserDTOAsIPagedList =
                    new StaticPagedList<ExpandedUserDTO>
                    (
                        col_UserDTO, intPage, intPageSize, intTotalPageCount
                        );

                return View(_UserDTOAsIPagedList);
            }
            catch (Exception ex) {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                List<ExpandedUserDTO> col_UserDTO = new List<ExpandedUserDTO>();

                return View(col_UserDTO.ToPagedList(1, 25));
            }
        }

        // Users Part

        // GET: /Admin/ManageUsers/Edit/Create 
        [Authorize(Roles = "SystemAdmin, FullAdmin")]
        public ActionResult Create() {
            ExpandedUserDTO objExpandedUserDTO = new ExpandedUserDTO();

            ViewBag.Roles = GetAllRolesAsSelectList();

            return View(objExpandedUserDTO);
        }

        // PUT: /Admin/ManageUsers/Create
        [Authorize(Roles = "SystemAdmin, FullAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExpandedUserDTO paramExpandedUserDTO) {
            try {
                if (paramExpandedUserDTO == null) {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var Email = paramExpandedUserDTO.Email.Trim();
                var UserName = paramExpandedUserDTO.UserName.Trim();
                var Password = paramExpandedUserDTO.Password.Trim();

                if (Email == "") {
                    throw new Exception("No Email");
                }

                if (UserName == "") {
                    throw new Exception("No User Name");
                }

                if (Password == "") {
                    throw new Exception("No Password");
                }


                // Create user

                var objNewAdminUser = new ApplicationUser { UserName = UserName, Email = Email };
                var AdminUserCreateResult = UserManager.Create(objNewAdminUser, Password);

                if (AdminUserCreateResult.Succeeded == true) {
                    string strNewRole = Convert.ToString(Request.Form["Roles"]);

                    if (strNewRole != "0") {
                        // Put user in role
                        UserManager.AddToRole(objNewAdminUser.Id, strNewRole);
                    }

                    return Redirect("~/Admin/ManageUsers");
                }
                else {
                    ViewBag.Roles = GetAllRolesAsSelectList();
                    ModelState.AddModelError(string.Empty,
                        "Error: Failed to create the user. Password must be at least 6 characters, contain one upper case, one lower case, and one numerical digit.");
                    return View(paramExpandedUserDTO);
                }
            }
            catch (Exception ex) {
                ViewBag.Roles = GetAllRolesAsSelectList();
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("Create");
            }
        }


        // GET: /Admin/ManageUsers/Edit/User 
        [Authorize(Roles = "SystemAdmin, FullAdmin")]
        public ActionResult EditUser(string UserName) {
            if (UserName == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpandedUserDTO objExpandedUserDTO = GetUser(UserName);
            if (objExpandedUserDTO == null) {
                return HttpNotFound();
            }
            return View(objExpandedUserDTO);
        }

        // PUT: /Admin/ManageUsers/EditUser
        [Authorize(Roles = "SystemAdmin, FullAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        #region public ActionResult EditUser(ExpandedUserDTO paramExpandedUserDTO)
        public ActionResult EditUser(ExpandedUserDTO paramExpandedUserDTO) {
            try {
                if (paramExpandedUserDTO == null) {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ExpandedUserDTO objExpandedUserDTO = UpdateDTOUser(paramExpandedUserDTO);

                if (objExpandedUserDTO == null) {
                    return HttpNotFound();
                }

                return Redirect("~/Admin/ManageUsers");
            }
            catch (Exception ex) {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("EditUser", GetUser(paramExpandedUserDTO.UserName));
            }
        }
        #endregion

        // DELETE: /Admin/ManageUsers/DeleteUser
        [Authorize(Roles = "SystemAdmin, FullAdmin")]
        #region public ActionResult DeleteUser(string UserName)
        public ActionResult DeleteUser(string UserName) {
            try {
                if (UserName == null) {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (UserName.ToLower() == this.User.Identity.Name.ToLower()) {
                    ModelState.AddModelError(
                        string.Empty, "Error: Cannot delete the current user");

                    return View("EditUser");
                }

                ExpandedUserDTO objExpandedUserDTO = GetUser(UserName);

                if (objExpandedUserDTO == null) {
                    return HttpNotFound();
                }
                else {
                    DeleteUser(objExpandedUserDTO);
                }

                return Redirect("~/Admin/ManageUsers");
            }
            catch (Exception ex) {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("EditUser", GetUser(UserName));
            }
        }
        #endregion

        // GET: /Admin/ManageUsers/EditRoles/TestUser 
        [Authorize(Roles = "SystemAdmin, FullAdmin")]
        public ActionResult EditRoles(string UserName) {
            if (UserName == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserName = UserName.ToLower();

            // Checks if user exists
            ExpandedUserDTO objExpandedUserDTO = GetUser(UserName);

            if (objExpandedUserDTO == null) {
                return HttpNotFound();
            }

            UserAndRolesDTO objUserAndRolesDTO =
                GetUserAndRoles(UserName);

            return View(objUserAndRolesDTO);
        }


        // PUT: /Admin/ManageUsers/EditRoles/TestUser 
        [Authorize(Roles = "SystemAdmin, FullAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRoles(UserAndRolesDTO paramUserAndRolesDTO) {
            try {
                if (paramUserAndRolesDTO == null) {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                string UserName = paramUserAndRolesDTO.UserName;
                string strNewRole = Convert.ToString(Request.Form["AddRole"]);

                if (strNewRole != "No Roles Found") {
                    // Gets the User
                    ApplicationUser user = UserManager.FindByName(UserName);

                    // Put user in role
                    UserManager.AddToRole(user.Id, strNewRole);
                }

                ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));

                UserAndRolesDTO objUserAndRolesDTO =
                    GetUserAndRoles(UserName);

                return View(objUserAndRolesDTO);
            }
            catch (Exception ex) {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("EditRoles");
            }
        }

        // DELETE: /Admin/ManageUsers/DeleteRole?UserName="TestUser&RoleName=SystemAdmin
        [Authorize(Roles = "SystemAdmin, FullAdmin")]
        public ActionResult DeleteRole(string UserName, string RoleName) {
            try {
                if ((UserName == null) || (RoleName == null)) {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                UserName = UserName.ToLower();

                // Checks that we have a user
                ExpandedUserDTO objExpandedUserDTO = GetUser(UserName);

                if (objExpandedUserDTO == null) {
                    return HttpNotFound();
                }

                if (UserName.ToLower() ==
                    this.User.Identity.Name.ToLower() && RoleName == "SystemAdmin") {
                    ModelState.AddModelError(string.Empty,
                        "Error: Cannot delete SystemAdmin Role for the current user");
                }

                // Get the User
                ApplicationUser user = UserManager.FindByName(UserName);
                // Remove User from role
                UserManager.RemoveFromRoles(user.Id, RoleName);
                UserManager.Update(user);

                ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));

                return RedirectToAction("EditRoles", new { UserName = UserName });
            }
            catch (Exception ex) {
                ModelState.AddModelError(string.Empty, "Error: " + ex);

                ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));

                UserAndRolesDTO objUserAndRolesDTO =
                    GetUserAndRoles(UserName);

                return View("EditRoles", objUserAndRolesDTO);
            }
        }

        // Roles 

        // GET: /Admin/ViewAllRoles
        [Authorize(Roles = "SystemAdmin")]
        #region public ActionResult ViewAllRoles()
        public ActionResult ViewAllRoles() {
            var roleManager =
                new RoleManager<IdentityRole>
                (
                    new RoleStore<IdentityRole>(new ApplicationDbContext())
                    );

            List<RoleDTO> colRoleDTO = (from objRole in roleManager.Roles
                                        select new RoleDTO {
                                            Id = objRole.Id,
                                            RoleName = objRole.Name
                                        }).ToList();

            return View(colRoleDTO);
        }
        #endregion

        // GET: /Admin/ManageUsers/AddRole
        [Authorize(Roles = "SystemAdmin")]
        #region public ActionResult AddRole()
        public ActionResult AddRole() {
            RoleDTO objRoleDTO = new RoleDTO();

            return View(objRoleDTO);
        }
        #endregion

        // PUT: /Admin/AddRole
        [Authorize(Roles = "SystemAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRole(RoleDTO paramRoleDTO) {
            try {
                if (paramRoleDTO == null) {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var RoleName = paramRoleDTO.RoleName.Trim();

                if (RoleName == "") {
                    throw new Exception("No RoleName");
                }

                // Create Role
                var roleManager =
                    new RoleManager<IdentityRole>(
                        new RoleStore<IdentityRole>(new ApplicationDbContext())
                        );

                if (!roleManager.RoleExists(RoleName)) {
                    roleManager.Create(new IdentityRole(RoleName));
                }

                return Redirect("~/Admin/ViewAllRoles");
            }
            catch (Exception ex) {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("AddRole");
            }
        }


        // DELETE: /Admin/ManageUsers/DeleteUserRole?RoleName=TestRole
        [Authorize(Roles = "SystemAdmin")]
        public ActionResult DeleteUserRole(string RoleName) {
            try {
                if (RoleName == null) {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (RoleName.ToLower() == "systemadmin") {
                    throw new Exception(String.Format("Cannot delete {0} Role.", RoleName));
                }

                var roleManager =
                    new RoleManager<IdentityRole>(
                        new RoleStore<IdentityRole>(new ApplicationDbContext()));

                var UsersInRole = roleManager.FindByName(RoleName).Users.Count();
                if (UsersInRole > 0) {
                    throw new Exception(
                        String.Format("Canot delete {0} Role because it still has users.",RoleName));
                }

                var objRoleToDelete = (from objRole in roleManager.Roles
                                       where objRole.Name == RoleName
                                       select objRole).FirstOrDefault();
                if (objRoleToDelete != null) {
                    roleManager.Delete(objRoleToDelete);
                }
                else {
                    throw new Exception(
                        String.Format(
                            "Canot delete {0} Role does not exist.",
                            RoleName)
                            );
                }

                List<RoleDTO> colRoleDTO = (from objRole in roleManager.Roles
                                            select new RoleDTO {
                                                                Id = objRole.Id,
                                                                RoleName = objRole.Name
                                                                }).ToList();

                return View("ViewAllRoles", colRoleDTO);
            }
            catch (Exception ex) {
                ModelState.AddModelError(string.Empty, "Error: " + ex);

                var roleManager =
                    new RoleManager<IdentityRole>(
                        new RoleStore<IdentityRole>(new ApplicationDbContext()));

                List<RoleDTO> colRoleDTO = (from objRole in roleManager.Roles
                                            select new RoleDTO {
                                                                Id = objRole.Id,
                                                                RoleName = objRole.Name
                                                                }).ToList();

                return View("ViewAllRoles", colRoleDTO);
            }
        }



        //
        public ApplicationUserManager UserManager {
            get {
                return _userManager ??
                    HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set {
                _userManager = value;
            }
        }



        public ApplicationRoleManager RoleManager {
            get {
                return _roleManager ??
                    HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationRoleManager>();
            }
            private set {
                _roleManager = value;
            }
        }


        private List<SelectListItem> GetAllRolesAsSelectList() {
            List<SelectListItem> SelectRoleListItems =
                new List<SelectListItem>();

            var roleManager =
                new RoleManager<IdentityRole>(
                    new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var colRoleSelectList = roleManager.Roles.OrderBy(x => x.Name).ToList();

            SelectRoleListItems.Add(
                new SelectListItem {
                    Text = "Select",
                    Value = "0"
                });

            foreach (var item in colRoleSelectList) {
                SelectRoleListItems.Add(
                    new SelectListItem {
                        Text = item.Name.ToString(),
                        Value = item.Name.ToString()
                    });
            }

            return SelectRoleListItems;
        }

        private ExpandedUserDTO GetUser(string paramUserName) {
            ExpandedUserDTO objExpandedUserDTO = new ExpandedUserDTO();

            var result = UserManager.FindByName(paramUserName);

            // If we could not find the user, throw an exception
            if (result == null) throw new Exception("Could not find the User");

            objExpandedUserDTO.UserName = result.UserName;
            objExpandedUserDTO.Email = result.Email;
            objExpandedUserDTO.LockoutEndDateUtc = result.LockoutEndDateUtc;
            objExpandedUserDTO.AccessFailedCount = result.AccessFailedCount;
            objExpandedUserDTO.PhoneNumber = result.PhoneNumber;

            return objExpandedUserDTO;
        }

        private ExpandedUserDTO UpdateDTOUser(ExpandedUserDTO paramExpandedUserDTO) {
            ApplicationUser result =
                UserManager.FindByName(paramExpandedUserDTO.UserName);

            // If we could not find the user, throw an exception
            if (result == null) {
                throw new Exception("Could not find the User");
            }

            result.Email = paramExpandedUserDTO.Email;

            // Lets check if the account needs to be unlocked
            if (UserManager.IsLockedOut(result.Id)) {
                // Unlock user
                UserManager.ResetAccessFailedCountAsync(result.Id);
            }

            UserManager.Update(result);

            // Was a password sent across?
            if (!string.IsNullOrEmpty(paramExpandedUserDTO.Password)) {
                // Remove current password
                var removePassword = UserManager.RemovePassword(result.Id);
                if (removePassword.Succeeded) {
                    // Add new password
                    var AddPassword =
                        UserManager.AddPassword(
                            result.Id,
                            paramExpandedUserDTO.Password
                            );

                    if (AddPassword.Errors.Count() > 0) {
                        throw new Exception(AddPassword.Errors.FirstOrDefault());
                    }
                }
            }

            return paramExpandedUserDTO;
        }

        private void DeleteUser(ExpandedUserDTO paramExpandedUserDTO) {
            ApplicationUser user =
                UserManager.FindByName(paramExpandedUserDTO.UserName);

            // If we could not find the user, throw an exception
            if (user == null) {
                throw new Exception("Could not find the User");
            }

            UserManager.RemoveFromRoles(user.Id, UserManager.GetRoles(user.Id).ToArray());
            UserManager.Update(user);
            UserManager.Delete(user);
        }

        private UserAndRolesDTO GetUserAndRoles(string UserName) {
            // Go get the User
            ApplicationUser user = UserManager.FindByName(UserName);

            List<UserRoleDTO> colUserRoleDTO =
                (from objRole in UserManager.GetRoles(user.Id)
                 select new UserRoleDTO {
                     RoleName = objRole,
                     UserName = UserName
                 }).ToList();

            if (colUserRoleDTO.Count() == 0) {
                colUserRoleDTO.Add(new UserRoleDTO { RoleName = "No Roles Found" });
            }

            ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));

            // Create UserRolesAndPermissionsDTO
            UserAndRolesDTO objUserAndRolesDTO = new UserAndRolesDTO();
            objUserAndRolesDTO.UserName = UserName;
            objUserAndRolesDTO.colUserRoleDTO = colUserRoleDTO;
            return objUserAndRolesDTO;
        }

        private List<string> RolesUserIsNotIn(string UserName) {
            // Get roles the user is not in
            var colAllRoles = RoleManager.Roles.Select(x => x.Name).ToList();

            // Go get the roles for an individual
            ApplicationUser user = UserManager.FindByName(UserName);

            // If we could not find the user, throw an exception
            if (user == null) {
                throw new Exception("Could not find the User");
            }

            var colRolesForUser = UserManager.GetRoles(user.Id).ToList();
            var colRolesUserInNotIn = (from objRole in colAllRoles
                                       where !colRolesForUser.Contains(objRole)
                                       select objRole).ToList();

            if (colRolesUserInNotIn.Count() == 0) {
                colRolesUserInNotIn.Add("No Roles Found");
            }

            return colRolesUserInNotIn;
        }

    }
}