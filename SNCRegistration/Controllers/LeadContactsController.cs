﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SNCRegistration.ViewModels;
using System.Data.Entity.Validation;
using PagedList;
using System.Net.Mail;
using System.Web.Hosting;
using SNCRegistration.Helpers;

namespace SNCRegistration.Controllers
{
    [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
    public class LeadContactsController : Controller
    {
        private SNCRegistrationEntities db = new SNCRegistrationEntities();

        // GET: LeadContacts
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? searchYear, int? page)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentYearSort = searchYear;
            ViewBag.currentFilter = currentFilter;
            ViewBag.page = page;
            ViewBag.searchString = searchString;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.TcuTypeSortParam = sortOrder == "tcutype_asc" ? "tcutype_desc" : "tcutype_asc";
            ViewBag.TcuNumSortParam = sortOrder == "tcunum_asc" ? "tcunum_desc" : "tcunum_asc";

            Session["SessionSortOrder"] = ViewBag.CurrentSort;
            Session["SessionCurrentFilter"] = ViewBag.currentFilter;
            Session["SessionSearchYear"] = ViewBag.CurrentYearSort;
            Session["SessionPage"] = ViewBag.page;
            Session["SessionSearchString"] = ViewBag.searchString;


            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            ViewBag.CurrentYear = DateTime.Now.Year;
            ViewBag.AllYears = (from y in db.LeadContacts select y.EventYear).Distinct();

            var leadContacts = from s in db.LeadContacts
                               where s.EventYear == searchYear
                               select s;


            if (!String.IsNullOrEmpty(searchString))
            {
                leadContacts = leadContacts.Where(s => s.LeadContactLastName.Contains(searchString) || s.LeadContactFirstName.Contains(searchString) || s.UnitChapterNumber.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    leadContacts = leadContacts.OrderByDescending(s => s.LeadContactLastName);
                    break;
                case "tcutype_desc":
                    leadContacts = leadContacts.OrderByDescending(s => s.BSType);
                    break;
                case "tcunum_desc":
                    leadContacts = leadContacts.OrderByDescending(s => s.UnitChapterNumber);
                    break;
                case "name_asc":
                    leadContacts = leadContacts.OrderBy(s => s.LeadContactLastName);
                    break;
                case "tcutype_asc":
                    leadContacts = leadContacts.OrderBy(s => s.BSType);
                    break;
                case "tcunum_asc":
                    leadContacts = leadContacts.OrderBy(s => s.UnitChapterNumber);
                    break;
                default:
                    leadContacts = leadContacts.OrderBy(s => s.LeadContactLastName);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(leadContacts.ToPagedList(pageNumber, pageSize));

        }



        // GET: LeadContacts/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = new LeadContactVolunteer();

            model.leadContact = db.LeadContacts.Find(id);
            model.volunteers = db.Volunteers.Where(i => i.LeadContactID == id);

            this.Session["lGuidSession"] = model.leadContact.LeaderGuid;
            this.Session["lIDSession"] = model.leadContact.LeadContactID;

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        // GET: LeadContacts/Create
        [OverrideAuthorization]
        public ActionResult Create()
        {
            ViewBag.ShirtSizes = new SelectList (db.ShirtSizes.Where(s=>s.ShirtSizeCode!="00"), "ShirtSizeCode", "ShirtSizeDescription");
            ViewBag.Attendance = new SelectList(db.Attendances.Where(i => i.Volunteer == true), "AttendanceID", "Description");
            ViewBag.BSType = new SelectList(db.BSTypes, "BSTypeID", "BSTypeDescription");

            return View();
        }

        // POST: LeadContacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [OverrideAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LeadContactID,BSType,UnitChapterNumber,LeadContactFirstName,LeadContactLastName,LeadContactAddress,LeadContactCity,LeadContactState,LeadContactZip,LeadContactCellPhone,LeadContactEmail,VolunteerAttendingCode,SaturdayDinner,TotalFee,Booth,Comments,LeadContactShirtOrder,LeadContactShirtSize, LeaderGuid")] LeadContact leadContact)
        {
            if (ModelState.IsValid)
            {
                db.LeadContacts.Add(leadContact);

                var thisYear = DateTime.Now.Year.ToString();
                leadContact.EventYear = int.Parse(thisYear);

                try
                {
                    db.SaveChanges();
                    this.Session["lSession"] = leadContact.LeadContactID;
                    this.Session["leaderEmail"] = leadContact.LeadContactEmail;
                    TempData["myPK"] = leadContact.LeadContactID;
                    TempData.Keep();

                    var volFee = Session["fSession"]  as string;

                    if (Request["submit"].Equals("Add a volunteer"))
                    {
                        return RedirectToAction("Create", "Volunteers", new { LeadContactId = this.Session["lSession"] });
                    }
                    if (Request["submit"].Equals("Complete registration"))
                    //registration complete, no more people to add
                    {
                        var leadFee = db.BSTypes.Single(x => x.BSTypeID == leadContact.BSType).BSFee;
                        var total = db.ComputeTotal(leadContact.LeadContactID);
                        var email = Session["leaderEmail"] as string;
                        var body = "You have successfully registered for the Special Needs Camporee.The total fee due is $" + leadFee + "<br />" + db.GetVolunteerList(leadContact.LeadContactID);
                        Helpers.EmailHelpers.SendVolEmail("sncracc@gmail.com", email, "Registration Confirmation", body, Server.MapPath("~/App_Data/PDF/"));
                        return Redirect("Registered");
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    //retrieve the error message as a list of strings
                    var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                    //Join the list to a single string
                    var fullErrorMessage = string.Join(" ,", errorMessages);

                    //Combine the original exception message wtih the new one
                    var exceptionMessage = string.Concat(ex.Message, "The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
            }

            Session["lSession"] = leadContact.LeadContactID;

            return RedirectToAction("Create", "Volunteers");

        }

        // GET: LeadContacts/Edit/5
        [OverrideAuthorization]
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeadContact leadContact = db.LeadContacts.Find(id);

            SetGroupAttendingViewBag(leadContact.BSType, leadContact.VolunteerAttendingCode, leadContact.LeadContactShirtSize);

            if (leadContact == null)
            {
                return HttpNotFound();
            }
            return View(leadContact);
        }

        // POST: LeadContacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [OverrideAuthorization]
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var leadContact = db.LeadContacts.Find(id);

            if (TryUpdateModel(leadContact, "",
               new string[] { "LeadContactID", "BSType", "UnitChapterNumber", "LeadContactFirstName", "LeadContactLastName", "LeadContactAddress", "LeadContactCity", "LeadContactState", "LeadContactZip", "LeadContactPhone", "LeadContactEmail", "VolunteerAttendingCode", "SaturdayDinner", "TotalFee", "Booth", "Comments", "LeadContactShirtOrder", "LeadContactShirtSize", "CheckedIn" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Details", "LeadContacts", new { id = leadContact.LeadContactID });
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            SetGroupAttendingViewBag(leadContact.BSType, leadContact.VolunteerAttendingCode, leadContact.LeadContactShirtSize);
            return View(leadContact);
        }



        // GET: Participants/CheckIn/5
        public ActionResult CheckIn(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeadContact leadContact = db.LeadContacts.Find(id);
            if (leadContact == null)
            {
                return HttpNotFound();
            }
            return View(leadContact);
        }

        // POST: Participants/CheckIn/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost, ActionName("CheckIn")]
        [ValidateAntiForgeryToken]
        public ActionResult CheckInPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var leadcontact = db.LeadContacts.Find(id);


            if (TryUpdateModel(leadcontact, "",
               new string[] { "CheckedIn" }))
            {
                try
                {
                    db.SaveChanges();
                    TempData["notice"] = "Volunteer Checked In Status Saved!";
                    return RedirectToAction("CheckIn", "LeadContacts", new { id = leadcontact.LeadContactID });
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(leadcontact);


        }

        [OverrideAuthorization]
        public ActionResult Registered()
        {
            return View();
        }

        // GET: LeadContacts/Delete/5
        [OverrideAuthorization]
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeadContact leadContact = db.LeadContacts.Find(id);
            if (leadContact == null)
            {
                return HttpNotFound();
            }
            return View(leadContact);
        }

        // POST: LeadContacts/Delete/5
        [OverrideAuthorization]
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            LeadContact leadContact = db.LeadContacts.Find(id);



            try
            {
                db.LeadContacts.Remove(leadContact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception err)
            {

                ModelState.AddModelError("DBerror", "Unable to Delete Lead Contact. Please delete associated volunteer records first before deleting Lead Contact.");
            }

            return RedirectToAction("Index");

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult GetYear()
        {
            return View("ActiveRegistrationYear");
        }

        private void SetGroupAttendingViewBag(int? bstgroup = null, int? attending = null, string shirtSize = null)
        {

            if (bstgroup == null)
            {
                ViewBag.bstID = new SelectList(db.BSTypes, "BSTypeID", "BSTypeDescription");
            }
            else
                ViewBag.bstID = new SelectList(db.BSTypes.ToArray(), "BSTypeID", "BSTypeDescription", bstgroup);

            if (attending == null)
            {
                ViewBag.AttendanceID = new SelectList(db.Attendances, "AttendanceID", "Description");
            }
            else
                ViewBag.AttendanceID = new SelectList(db.Attendances.Where(i => i.Volunteer == true), "AttendanceID", "Description", attending);

            if (shirtSize == null)
            {
                ViewBag.shirtSizeName = new SelectList(db.ShirtSizes, "ShirtSizeCode", "ShirtSizeDescription");
            }
            else
                ViewBag.shirtSizeName = new SelectList(db.ShirtSizes.ToArray(), "ShirtSizeCode", "ShirtSizeDescription", shirtSize);
        }
    }
}
