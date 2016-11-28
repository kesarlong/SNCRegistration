using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using SNCRegistration.ViewModels;
using System.Data;

namespace SNCRegistration.Controllers {

    public class AdminController : Controller {
        private ApplicationUserManager userManager;

        private ApplicationRoleManager roleManager;

        SNCRegistrationEntities db = new SNCRegistrationEntities();


        // GET: Admin
        public ActionResult Index() {
            return View();
        }

        //[Authorize(Roles = "SystemAdmin")]
        public ActionResult ManageUsers() {
            return View(db.AspNetUsers.ToList());
        }


        public ActionResult Create() {
            return View();
        }

        //Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email, UserName, Password")]AspNetUser admin) {
            try {
                if (ModelState.IsValid) {
                    db.AspNetUsers.Add(admin);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */) {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(admin);
        }




        //Details

        public ActionResult Details(string id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser admin = db.AspNetUsers.Find(id);
            if (admin == null) {
                return HttpNotFound();
            }
            return View(admin);
        }


        //Get Admin/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id) {

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var adminToUpdate = db.AspNetUsers.Find(id);
            if (TryUpdateModel(adminToUpdate, "",
               new string[] { "UserName", "Email" })) {
                try {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (DataException /* dex */) {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(adminToUpdate);
        }


        //Delete Get
        public ActionResult Delete(string id, bool? saveChangesError = false) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault()) {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            AspNetUser admin = db.AspNetUsers.Find(id);
            if (admin == null) {
                return HttpNotFound();
            }
            return View(admin);
        }


        //Delete Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id) {
            try {
                AspNetUser admin = db.AspNetUsers.Find(id);
                db.AspNetUsers.Remove(admin);
                db.SaveChanges();
            }
            catch (DataException/* dex */) {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }




        public ApplicationRoleManager RoleManager {

            get {

                return roleManager;

            }

            private set {

                roleManager = value;

            }

        }


    }



}