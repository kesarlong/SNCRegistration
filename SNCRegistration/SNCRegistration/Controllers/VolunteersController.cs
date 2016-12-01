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

namespace SNCRegistration.Controllers
{
    public class VolunteersController : Controller
    {
        private SNCRegistrationEntities db = new SNCRegistrationEntities();

        // GET: Volunteers
        public ActionResult Index()
        {
            
            return View(db.Volunteers.ToList());
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

        // GET: Volunteers/Create
        public ActionResult Create()
        {
            //ViewBag.VolunteerID = new SelectList(db.Volunteers, "VolunteerID", "VolunteerFirstName");
            //ViewBag.VolunteerID = new SelectList(db.Volunteers, "VolunteerID", "VolunteerFirstName");
            return View();
        }

        // POST: Volunteers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VolunteerID,VolunteerFirstName,VolunteerLastName,VolunteerAge,LeadContactID,VolunteerShirtOrder,VolunteerShirtSize,VolunteerAttendingCode,SaturdayDinner,UnitChapterNumber,Comments")] Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                db.Volunteers.Add(volunteer);
                try
                {
                    db.SaveChanges();
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
                
                return RedirectToAction("Index");
            }

            //ViewBag.VolunteerID = new SelectList(db.Volunteers, "VolunteerID", "VolunteerFirstName", volunteer.VolunteerID);
            //ViewBag.VolunteerID = new SelectList(db.Volunteers, "VolunteerID", "VolunteerFirstName", volunteer.VolunteerID);
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

        // POST: Volunteers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VolunteerID,VolunteerFirstName,VolunteerLastName,VolunteerAge,LeadContactID,VolunteerShirtOrder,VolunteerShirtSize,VolunteerAttendingCode,SaturdayDinner,UnitChapterNumber,Comments")] Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(volunteer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VolunteerID = new SelectList(db.Volunteers, "VolunteerID", "VolunteerFirstName", volunteer.VolunteerID);
            ViewBag.VolunteerID = new SelectList(db.Volunteers, "VolunteerID", "VolunteerFirstName", volunteer.VolunteerID);
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
