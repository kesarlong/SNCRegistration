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
    [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
    public class LeadContactsController : Controller
    {
        private SNCRegistrationEntities db = new SNCRegistrationEntities();

        // GET: LeadContacts
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? searchYear, int? page)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentYearSort = searchYear;
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

            ViewBag.CurrentYear = DateTime.Now.Year;
            ViewBag.AllYears = (from y in db.LeadContacts select y.EventYear).Distinct();

            var leadContacts = from s in db.LeadContacts
                               where s.EventYear == searchYear
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
            ViewBag.ShirtSizes = new SelectList (db.ShirtSizes, "ShirtSizeCode", "ShirtSizeDescription");
            ViewBag.Attendance = new SelectList(db.Attendances.Where(i => i.Volunteer == true), "AttendanceID", "Description");
            ViewBag.BSType = new SelectList(db.BSTypes, "BSTypeID", "BSTypeDescription");

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

                var thisYear = DateTime.Now.Year.ToString();
                leadContact.EventYear = int.Parse(thisYear);

                try
                {
                    db.SaveChanges();
                    this.Session["lSession"] = leadContact.LeadContactID;
                    this.Session["leaderEmail"] = leadContact.LeadContactEmail;
                    TempData["myPK"] = leadContact.LeadContactID;
                    TempData.Keep();
                    if (Request["submit"].Equals("Add a volunteer"))
                    {
                        return RedirectToAction("Create", "Volunteers", new { LeadContactId = this.Session["lSession"] });
                    }
                    if (Request["submit"].Equals("Complete registration"))
                    //registration complete, no more people to add
                    {
                        var email = Session["leaderEmail"] as string;
                        Helpers.EmailHelpers.SendEmail("sncracc@gmail.com", email, "Registration Confirmation", "You have successfully registered for the Special Needs Camporee. The total fee due is $10.00");
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

                    return RedirectToAction("Details", "LeadContacts", new { id = leadcontact.LeadContactID });
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(leadcontact);


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

        public ActionResult GetYear()
        {
            return View("ActiveRegistrationYear");
        }
    }
}
