using SNCRegistration.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace SNCRegistration.Controllers
{
    public class VolunteersController : Controller
    {
        private SNCRegistrationEntities db = new SNCRegistrationEntities();

        // GET: Volunteers
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

            var volunteers = from s in db.Volunteers
                               select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                volunteers = volunteers.Where(s => s.VolunteerLastName.Contains(searchString) || s.VolunteerFirstName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    volunteers = volunteers.OrderByDescending(s => s.VolunteerLastName);
                    break;
                default:
                    volunteers = volunteers.OrderBy(s => s.VolunteerLastName);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(volunteers.ToPagedList(pageNumber, pageSize));




            //Original Class before changes. Delete if no problems. -Einar
            //public ActionResult Index()
            //{

            //    return View(db.Volunteers.ToList());
            //}
        }

        // GET: Volunteers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            return View(volunteer);
        }

        public ActionResult Registered() {
            return View();
        }

        // GET: Volunteers/Create
        public ActionResult Create()
        {
            ViewBag.ShirtSizes = new SelectList(db.ShirtSizes, "ShirtSizeCode", "ShirtSizeDescription");
            ViewBag.Attendance = new SelectList(db.Attendances, "AttendanceID", "Description");
            ViewBag.Age = new SelectList(db.Ages, "AgeID", "AgeDescription");
            return View();
        }

        // POST: Volunteers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VolunteerID,VolunteerFirstName,VolunteerLastName,VolunteerAge,LeadContactID,VolunteerShirtOrder,VolunteerShirtSize,VolunteerAttendingCode,SaturdayDinner,UnitChapterNumber,Comments, LeaderGuid")] Volunteer volunteer)
        {

            if (ModelState.IsValid)
            {

                db.Volunteers.Add(volunteer);
                var thisYear = DateTime.Now.Year.ToString();
                volunteer.EventYear = int.Parse(thisYear);

                try
                {
                    db.SaveChanges();

                    this.Session["lSession"] = volunteer.LeadContactID;
                        if (Request["submit"].Equals("Add an additional volunteer"))
                    { return RedirectToAction("Create", "Volunteers", new { LeadContactId = this.Session["lSession"] }); }

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
                //Session["lSession"] = volunteer.LeadContactID;

                //return RedirectToAction("Registered");
            }
            return View(volunteer);
        }

        // GET: Volunteers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            ViewBag.VolunteerID = new SelectList(db.Volunteers, "VolunteerID", "VolunteerFirstName", volunteer.VolunteerID);
            ViewBag.VolunteerID = new SelectList(db.Volunteers, "VolunteerID", "VolunteerFirstName", volunteer.VolunteerID);
            return View(volunteer);
        }


        // Original Edit Code, Delete this if nothing broken
        // POST: Volunteers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "VolunteerID,VolunteerFirstName,VolunteerLastName,VolunteerAge,LeadContactID,VolunteerShirtOrder,VolunteerShirtSize,VolunteerAttendingCode,SaturdayDinner,UnitChapterNumber,Comments")] Volunteer volunteer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(volunteer).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.VolunteerID = new SelectList(db.Volunteers, "VolunteerID", "VolunteerFirstName", volunteer.VolunteerID);
        //    ViewBag.VolunteerID = new SelectList(db.Volunteers, "VolunteerID", "VolunteerFirstName", volunteer.VolunteerID);
        //    return View(volunteer);
        //}

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var volunteer = db.Volunteers.Find(id);

            if (TryUpdateModel(volunteer, "",
               new string[] { "VolunteerID", "VolunteerFirstName", "VolunteerLastName", "VolunteerAge", "LeadContactID", "VolunteerShirtOrder", "VolunteerShirtSize", "VolunteerAttendingCode", "SaturdayDinner", "UnitChapterNumber", "CheckedIn", "Comments" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Details", "LeadContacts", new { id = volunteer.LeadContactID });
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(volunteer);


        }


        // GET: Volunteer/Checkin/5
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
        public ActionResult CheckIn(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = db.Volunteers.Find(id);
            
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            return View(volunteer);
        }

        // POST: VolunteerCheckIn/5
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
            var volunteer = db.Volunteers.Find(id);


            if (TryUpdateModel(volunteer, "",
               new string[] { "CheckedIn" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Details", "LeadContacts", new { id = volunteer.LeadContactID });
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(volunteer);


        }


        // GET: Volunteers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            return View(volunteer);
        }

        // POST: Volunteers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Volunteer volunteer = db.Volunteers.Find(id);
            db.Volunteers.Remove(volunteer);
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
