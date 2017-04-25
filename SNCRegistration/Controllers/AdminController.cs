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
using System.Web.Security;
using System.IO;
using System.Net.Mime;

namespace SNCRegistration.Controllers
    {

    [CustomAuthorize(Roles = "SystemAdmin")]
    public class AdminController : Controller
        {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        [OverrideAuthorization]
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin, UnassignedUser")]
        //Default page after log in. Welcome message with indication to select menu options on the left bar.
        public ActionResult Index()
            {
            return View();
            }

        //Manage users list. Only SystemAdmins can use.

        public ActionResult ManageUsers(string searchStringUserNameOrEmail, string currentFilter, int? page)
            {
            try
                {
                int intPage = 1;
                int intPageSize = 5;
                int intTotalPageCount = 0;

                if (searchStringUserNameOrEmail != null)
                    {
                    intPage = 1;
                    }
                else
                    {
                    if (currentFilter != null)
                        {
                        searchStringUserNameOrEmail = currentFilter;
                        intPage = page ?? 1;
                        }
                    else
                        {
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

                foreach (var item in result)
                    {
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
            catch (Exception ex)
                {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                List<ExpandedUserDTO> col_UserDTO = new List<ExpandedUserDTO>();

                return View(col_UserDTO.ToPagedList(1, 25));
                }
            }


        // User Creation. Only System Admins can use
        // GET: /Admin/ManageUsers/Edit/Create 

        public ActionResult Create()
            {
            ExpandedUserDTO objExpandedUserDTO = new ExpandedUserDTO();

            ViewBag.Roles = GetAllRolesAsSelectList();

            return View(objExpandedUserDTO);
            }

        // User Creation. Only System Admins can use
        // GET: /Admin/ManageUsers/Edit/Create 
        // PUT: /Admin/ManageUsers/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExpandedUserDTO paramExpandedUserDTO)
            {

            try
                {
                if (paramExpandedUserDTO == null)
                    {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                var Email = paramExpandedUserDTO.Email.Trim();
                var UserName = paramExpandedUserDTO.UserName.Trim();
                var Password = paramExpandedUserDTO.Password.Trim();

                if (Email == "")
                {
                    throw new Exception("No Email");
                }
                if (UserName == "")
                    {
                    throw new Exception("No User Name");
                    }

                if (Password == "")
                    {
                    throw new Exception("No Password");
                    }


                // Create user

                var objNewAdminUser = new ApplicationUser { UserName = UserName, Email = Email };
                var AdminUserCreateResult = UserManager.Create(objNewAdminUser, Password);


                if (AdminUserCreateResult.Succeeded == true)
                    {
                    string strNewRole = Convert.ToString(Request.Form["Roles"]);

                    if (strNewRole != "0")
                        {
                        // Put user in role
                        UserManager.AddToRole(objNewAdminUser.Id, strNewRole);
                        }

                    return Redirect("~/Admin/ManageUsers");
                    }
                else
                    {
                    ViewBag.Roles = GetAllRolesAsSelectList();
                    ModelState.AddModelError(string.Empty,
                        "Error: Failed to create the user. Ensure that user or email does not already exist. Check that password has at least 6 characters, contain one upper case, contain one lower case, and one numerical digit.");
                    return View(paramExpandedUserDTO);
                    }
                }
            catch
                {
                ViewBag.Roles = GetAllRolesAsSelectList();
                ModelState.AddModelError(string.Empty, "Error: See below for errors.");
                return View("Create");
                }
            }

        public JsonResult doesUserNameExist(string UserName)
        {

            var user = Membership.GetUser(UserName);

            return Json(user == null);
        }

        public JsonResult doesEmailExist(string Email)
        {

            var email = Membership.GetUser(Email);

            return Json(email == null);
        }

        //User Edit. Only System Admins can edit other users.
        // GET: /Admin/ManageUsers/Edit/User 

        public ActionResult EditUser(string UserName)
            {
            if (UserName == null)
                {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            ExpandedUserDTO objExpandedUserDTO = GetUser(UserName);
            if (objExpandedUserDTO == null)
                {
                return HttpNotFound();
                }
            return View(objExpandedUserDTO);
            }


        //User Edit. Only System Admins can edit other users.
        // PUT: /Admin/ManageUsers/EditUser

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(ExpandedUserDTO paramExpandedUserDTO)
            {
            try
                {
                if (paramExpandedUserDTO == null)
                    {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                ExpandedUserDTO objExpandedUserDTO = UpdateDTOUser(paramExpandedUserDTO);

                if (objExpandedUserDTO == null)
                    {
                    return HttpNotFound();
                    }

                return Redirect("~/Admin/ManageUsers");
                }
            catch (Exception ex)
                {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("EditUser", GetUser(paramExpandedUserDTO.UserName));
                }
            }


        //User Delete. Only System Admins can delete users.
        // DELETE: /Admin/ManageUsers/DeleteUser

        public ActionResult DeleteUser(string UserName)
            {
            try
                {
                if (UserName == null)
                    {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                if (UserName.ToLower() == this.User.Identity.Name.ToLower())
                    {
                    ModelState.AddModelError(
                        string.Empty, "Error: Cannot delete the current user");

                    return View("EditUser");
                    }

                ExpandedUserDTO objExpandedUserDTO = GetUser(UserName);

                if (objExpandedUserDTO == null)
                    {
                    return HttpNotFound();
                    }
                else
                    {
                    DeleteUser(objExpandedUserDTO);
                    }

                return Redirect("~/Admin/ManageUsers");
                }
            catch (Exception ex)
                {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("EditUser", GetUser(UserName));
                }
            }

        private void DeleteUser(ExpandedUserDTO paramExpandedUserDTO)
        {
            ApplicationUser user =
                UserManager.FindByName(paramExpandedUserDTO.UserName);

            // If we could not find the user, throw an exception
            if (user == null)
            {
                throw new Exception("Could not find the User");
            }

            UserManager.RemoveFromRoles(user.Id, UserManager.GetRoles(user.Id).ToArray());
            UserManager.Update(user);
            UserManager.Delete(user);
        }
        //User edit roles. Only System Admins can edit user roles.
        // GET: /Admin/ManageUsers/EditRoles/TestUser 

        public ActionResult EditRoles(string UserName)
            {
            if (UserName == null)
                {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

            UserName = UserName.ToLower();

            // Checks if user exists
            ExpandedUserDTO objExpandedUserDTO = GetUser(UserName);

            if (objExpandedUserDTO == null)
                {
                return HttpNotFound();
                }

            UserAndRolesDTO objUserAndRolesDTO =
                GetUserAndRoles(UserName);

            return View(objUserAndRolesDTO);
            }


        // PUT: /Admin/ManageUsers/EditRoles/TestUser 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRoles(UserAndRolesDTO paramUserAndRolesDTO)
            {
            try
                {
                if (paramUserAndRolesDTO == null)
                    {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                string UserName = paramUserAndRolesDTO.UserName;
                string strNewRole = Convert.ToString(Request.Form["AddRole"]);

                if (strNewRole != "No Roles Found")
                    {
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
            catch (Exception ex)
                {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("EditRoles");
                }
            }

        // DELETE: /Admin/ManageUsers/DeleteRole?UserName="TestUser&RoleName=SystemAdmin
        public ActionResult DeleteRole(string UserName, string RoleName)
            {
            try
                {
                if ((UserName == null) || (RoleName == null))
                    {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                UserName = UserName.ToLower();

                // Checks that we have a user
                ExpandedUserDTO objExpandedUserDTO = GetUser(UserName);

                if (objExpandedUserDTO == null)
                    {
                    return HttpNotFound();
                    }

                if (UserName.ToLower() ==
                    this.User.Identity.Name.ToLower() && RoleName == "SystemAdmin")
                    {
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
            catch (Exception ex)
                {
                ModelState.AddModelError(string.Empty, "Error: " + ex);

                ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));

                UserAndRolesDTO objUserAndRolesDTO =
                    GetUserAndRoles(UserName);

                return View("EditRoles", objUserAndRolesDTO);
                }
            }

        // Roles 

        //// GET: /Admin/ViewAllRoles
        //public ActionResult ViewAllRoles()
        //    {
        //    var roleManager =
        //        new RoleManager<IdentityRole>
        //        (
        //            new RoleStore<IdentityRole>(new ApplicationDbContext())
        //            );

        //    List<RoleDTO> colRoleDTO = (from objRole in roleManager.Roles
        //                                select new RoleDTO
        //                                    {
        //                                    Id = objRole.Id,
        //                                    RoleName = objRole.Name
        //                                    }).ToList();

        //    return View(colRoleDTO);
        //    }

        // GET: /Admin/ManageUsers/AddRole
        //public ActionResult AddRole()
        //    {
        //    RoleDTO objRoleDTO = new RoleDTO();

        //    return View(objRoleDTO);
        //    }

        // PUT: /Admin/AddRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRole(RoleDTO paramRoleDTO)
            {
            try
                {
                if (paramRoleDTO == null)
                    {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                var RoleName = paramRoleDTO.RoleName.Trim();

                if (RoleName == "")
                    {
                    throw new Exception("No RoleName");
                    }

                // Create Role
                var roleManager =
                    new RoleManager<IdentityRole>(
                        new RoleStore<IdentityRole>(new ApplicationDbContext())
                        );

                if (!roleManager.RoleExists(RoleName))
                    {
                    roleManager.Create(new IdentityRole(RoleName));
                    }

                return Redirect("~/Admin/ViewAllRoles");
                }
            catch (Exception ex)
                {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("AddRole");
                }
            }


        // DELETE: /Admin/ManageUsers/DeleteUserRole?RoleName=TestRole
        public ActionResult DeleteUserRole(string RoleName)
            {
            try
                {
                if (RoleName == null)
                    {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                if (RoleName.ToLower() == "systemadmin")
                    {
                    throw new Exception(String.Format("Cannot delete {0} Role.", RoleName));
                    }

                var roleManager =
                    new RoleManager<IdentityRole>(
                        new RoleStore<IdentityRole>(new ApplicationDbContext()));

                var UsersInRole = roleManager.FindByName(RoleName).Users.Count();
                if (UsersInRole > 0)
                    {
                    throw new Exception(
                        String.Format("Canot delete {0} Role because it still has users.", RoleName));
                    }

                var objRoleToDelete = (from objRole in roleManager.Roles
                                       where objRole.Name == RoleName
                                       select objRole).FirstOrDefault();
                if (objRoleToDelete != null)
                    {
                    roleManager.Delete(objRoleToDelete);
                    }
                else
                    {
                    throw new Exception(
                        String.Format(
                            "Canot delete {0} Role does not exist.",
                            RoleName)
                            );
                    }

                List<RoleDTO> colRoleDTO = (from objRole in roleManager.Roles
                                            select new RoleDTO
                                                {
                                                Id = objRole.Id,
                                                RoleName = objRole.Name
                                                }).ToList();

                return View("ViewAllRoles", colRoleDTO);
                }
            catch (Exception ex)
                {
                ModelState.AddModelError(string.Empty, "Error: " + ex);

                var roleManager =
                    new RoleManager<IdentityRole>(
                        new RoleStore<IdentityRole>(new ApplicationDbContext()));

                List<RoleDTO> colRoleDTO = (from objRole in roleManager.Roles
                                            select new RoleDTO
                                                {
                                                Id = objRole.Id,
                                                RoleName = objRole.Name
                                                }).ToList();

                return View("ViewAllRoles", colRoleDTO);
                }
            }



        //
        public ApplicationUserManager UserManager
            {
            get
                {
                return _userManager ??
                    HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                }
            private set
                {
                _userManager = value;
                }
            }



        public ApplicationRoleManager RoleManager
            {
            get
                {
                return _roleManager ??
                    HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationRoleManager>();
                }
            private set
                {
                _roleManager = value;
                }
            }


        private List<SelectListItem> GetAllRolesAsSelectList()
            {
            List<SelectListItem> SelectRoleListItems =
                new List<SelectListItem>();

            var roleManager =
                new RoleManager<IdentityRole>(
                    new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var colRoleSelectList = roleManager.Roles.OrderBy(x => x.Name).ToList();

            SelectRoleListItems.Add(
                new SelectListItem
                    {
                    Text = "Select",
                    Value = "0"
                    });

            foreach (var item in colRoleSelectList)
                {
                SelectRoleListItems.Add(
                    new SelectListItem
                        {
                        Text = item.Name.ToString(),
                        Value = item.Name.ToString()
                        });
                }

            return SelectRoleListItems;
            }

        private ExpandedUserDTO GetUser(string paramUserName)
            {
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

        private ExpandedUserDTO UpdateDTOUser(ExpandedUserDTO paramExpandedUserDTO)
            {
            ApplicationUser result =
                UserManager.FindByName(paramExpandedUserDTO.UserName);

            // If we could not find the user, throw an exception
            if (result == null)
                {
                throw new Exception("Could not find the User");
                }

            result.Email = paramExpandedUserDTO.Email;

            // Lets check if the account needs to be unlocked
            if (UserManager.IsLockedOut(result.Id))
                {
                // Unlock user
                UserManager.ResetAccessFailedCountAsync(result.Id);
                }

            UserManager.Update(result);

            // Was a password sent across?
            if (!string.IsNullOrEmpty(paramExpandedUserDTO.Password))
                {
                // Remove current password
                var removePassword = UserManager.RemovePassword(result.Id);
                if (removePassword.Succeeded)
                    {
                    // Add new password
                    var AddPassword =
                        UserManager.AddPassword(
                            result.Id,
                            paramExpandedUserDTO.Password
                            );

                    if (AddPassword.Errors.Count() > 0)
                        {
                        throw new Exception(AddPassword.Errors.FirstOrDefault());
                        }
                    }
                }

            return paramExpandedUserDTO;
            }



        private UserAndRolesDTO GetUserAndRoles(string UserName)
            {
            // Go get the User
            ApplicationUser user = UserManager.FindByName(UserName);

            List<UserRoleDTO> colUserRoleDTO =
                (from objRole in UserManager.GetRoles(user.Id)
                 select new UserRoleDTO
                     {
                     RoleName = objRole,
                     UserName = UserName
                     }).ToList();

            if (colUserRoleDTO.Count() == 0)
                {
                colUserRoleDTO.Add(new UserRoleDTO { RoleName = "No Roles Found" });
                }

            ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));

            // Create UserRolesAndPermissionsDTO
            UserAndRolesDTO objUserAndRolesDTO = new UserAndRolesDTO();
            objUserAndRolesDTO.UserName = UserName;
            objUserAndRolesDTO.colUserRoleDTO = colUserRoleDTO;
            return objUserAndRolesDTO;
            }

        private List<string> RolesUserIsNotIn(string UserName)
            {
            // Get roles the user is not in
            var colAllRoles = RoleManager.Roles.Select(x => x.Name).ToList();

            // Go get the roles for an individual
            ApplicationUser user = UserManager.FindByName(UserName);

            // If we could not find the user, throw an exception
            if (user == null)
                {
                throw new Exception("Could not find the User");
                }

            var colRolesForUser = UserManager.GetRoles(user.Id).ToList();
            var colRolesUserInNotIn = (from objRole in colAllRoles
                                       where !colRolesForUser.Contains(objRole)
                                       select objRole).ToList();

            if (colRolesUserInNotIn.Count() == 0)
                {
                colRolesUserInNotIn.Add("No Roles Found");
                }

            return colRolesUserInNotIn;
            }


        public ActionResult PDFFileManagement()
        {

            var dir = new System.IO.DirectoryInfo(Server.MapPath("~/App_Data/PDF/"));
            System.IO.FileInfo[] fileNames = dir.GetFiles("*.*");
            List<string> items = new List<string>();

            foreach (var file in fileNames)
            {
                items.Add(file.Name);

            }

            return View(items);
           // return View();
        }

        public ActionResult SponsorImageFileManagement()
        {

            var dir = new System.IO.DirectoryInfo(Server.MapPath("~/Resources/SponsorImages/"));
            System.IO.FileInfo[] fileNames = dir.GetFiles("*.*");
            List<string> items = new List<string>();

            foreach (var file in fileNames)
            {
                items.Add(file.Name);

            }

            return View(items);
            // return View();
        }

        [HttpPost]
        public ActionResult UploadPDF(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/PDF"), fileName);
                    file.SaveAs(path);
                    
                }
                ViewBag.PDFMessage = "PDF Upload successful";
                return RedirectToAction("PDFFileManagement");
            }
            catch
            {
                ViewBag.PDFMessage = "PDF Upload failed";
                return RedirectToAction("PDFFileManagement");
            }
        }

        [HttpPost]
        public ActionResult UploadSponsorImage(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Resources/SponsorImages/"), fileName);
                    file.SaveAs(path);

                }
                ViewBag.IMGMessage = "Sponsor Image Upload successful";
                return RedirectToAction("SponsorImageFileManagement");
            }
            catch
            {
                ViewBag.IMGMessage = "Sponsor Image Upload failed";
                return RedirectToAction("SponsorImageFileManagement");
            }
        }

        [OverrideAuthorization]
        public ActionResult GetSponsorImage(string file)
        {
            var appData = Server.MapPath("~/Resources/SponsorImages/");
            var path = Path.Combine(appData, file);
            path = Path.GetFullPath(path);
            if (!path.StartsWith(appData))
            {
                // Ensure that we are serving file only inside the App_Data folder
                // and block requests outside like "../web.config"
                throw new HttpException(403, "Forbidden");
            }

            if (!System.IO.File.Exists(path))
            {
                return HttpNotFound();
            }

            return File(path, MediaTypeNames.Image.Jpeg);
        }

        [OverrideAuthorization]
        public ActionResult GetAsset(string file)
        {
            var appData = Server.MapPath("~/Resources/WebsiteImages/");
            var path = Path.Combine(appData, file);
            path = Path.GetFullPath(path);
            if (!path.StartsWith(appData))
            {
                // Ensure that we are serving file only inside the App_Data folder
                // and block requests outside like "../web.config"
                throw new HttpException(403, "Forbidden");
            }

            if (!System.IO.File.Exists(path))
            {
                return HttpNotFound();
            }

            return File(path, MediaTypeNames.Image.Jpeg);
        }


        public ActionResult DeletePDFFile(string FileName)
        {

            string fullPath = Request.MapPath("~/App_Data/PDF/" + FileName);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
                ViewBag.Message = "File successfully deleted.";
            }
            else
            {
                ViewBag.Message = "File cannot be deleted.";
            }

            return RedirectToAction("PDFFileManagement");
        }


        public ActionResult DeleteImgFile(string FileName)
        {

            string fullPath = Request.MapPath("~/Resources/SponsorImages/" + FileName);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
                ViewBag.Message = "File successfully deleted.";
            }
            else
            {
                ViewBag.Message = "File cannot be deleted.";
            }

            return RedirectToAction("SponsorImageFileManagement");
        }

        public FileResult DownloadPDF(string FileName)
        {
            return File("~/App_Data/PDF/" + FileName, System.Net.Mime.MediaTypeNames.Application.Octet, FileName);
        }
        public FileResult DownloadIMG(string FileName)
        {
            return File("~/Resources/SponsorImages/" + FileName, System.Net.Mime.MediaTypeNames.Application.Octet, FileName);
        }



    
}
    }