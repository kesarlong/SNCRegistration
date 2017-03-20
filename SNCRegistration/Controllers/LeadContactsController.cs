using System;
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

namespace SNCRegistration.Controllers
{
    public class LeadContactsController : Controller
    {
        private SNCRegistrationEntities db = new SNCRegistrationEntities();

        // GET: LeadContacts
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var leadContacts = from s in db.LeadContacts
                               select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                leadContacts = leadContacts.Where(s => s.LeadContactLastName.Contains(searchString) || s.LeadContactFirstName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    leadContacts = leadContacts.OrderByDescending(s => s.LeadContactLastName);
                    break;
                default:
                    leadContacts = leadContacts.OrderBy(s => s.LeadContactLastName);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(leadContacts.ToPagedList(pageNumber, pageSize));

        }

        // Original Index, delete if nothing broken.
        //public ActionResult Index()
        //{
        //    return View(db.LeadContacts.ToList());
        //}



        // GET: LeadContacts/Details/5

        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
        public ActionResult Details(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //LeadContact leadContact = db.LeadContacts.Find(id);
            //if (leadContact == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(leadContact);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = new LeadContactVolunteer();

            model.leadContact = db.LeadContacts.Find(id);
            model.volunteers = db.Volunteers.Where(i => i.LeadContactID == id);


            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        // GET: LeadContacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeadContacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LeadContactID,BSType,UnitChapterNumber,LeadContactFirstName,LeadContactLastName,LeadContactAddress,LeadContactCity,LeadContactState,LeadContactZip,LeadContactCellPhone,LeadContactEmail,VolunteerAttendingCode,SaturdayDinner,TotalFee,Booth,Comments,LeadContactShirtOrder,LeadContactShirtSize, LeaderGuid")] LeadContact leadContact)
        {
            if (ModelState.IsValid)
            {
                db.LeadContacts.Add(leadContact);

                try
                {
                    db.SaveChanges();
                    this.Session["lSession"] = leadContact.LeadContactID;
                    TempData["myPK"] = leadContact.LeadContactID;
                    TempData.Keep();
                    return RedirectToAction("Create", "Volunteers", new { LeadContactId = this.Session["lSession"] });
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
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin")]
        public ActionResult Edit(int? id)
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

        // POST: LeadContacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            return View(leadContact);
        }

        // ORIGINAL EDIT, DELETE IF NOTHING BROKEN.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "LeadContactID,BSType,UnitChapterNumber,LeadContactFirstName,LeadContactLastName,LeadContactAddress,LeadContactCity,LeadContactState,LeadContactZip,LeadContactPhone,LeadContactEmail,VolunteerAttendingCode,SaturdayDinner,TotalFee,Booth,Comments,LeadContactShirtOrder,LeadContactShirtSize")] LeadContact leadContact)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(leadContact).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(leadContact);
        //}


        // GET: Participants/CheckIn/5
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
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

        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
        [HttpPost, ActionName("CheckIn")]
        [ValidateAntiForgeryToken]
        public ActionResult CheckInPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var leadContact = db.LeadContacts.Find(id);

            if (TryUpdateModel(leadContact, "",
               new string[] { "CheckedIn" }))
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
            return View(leadContact);


        }


        // GET: LeadContacts/Delete/5
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LeadContact leadContact = db.LeadContacts.Find(id);
            db.LeadContacts.Remove(leadContact);
            db.SaveChanges();
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
    }
}
