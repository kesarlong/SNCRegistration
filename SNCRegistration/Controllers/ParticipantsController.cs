using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SNCRegistration.ViewModels;
using System.Data.Entity.Validation;
using System.Net.Mime;
using System.IO;
using System;
using PagedList;

namespace SNCRegistration.Controllers
{

    public class ParticipantsController : Controller
    {
        private SNCRegistrationEntities db = new SNCRegistrationEntities();

        // GET: Participants
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

            var participants = from s in db.Participants
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                participants = participants.Where(s => s.ParticipantLastName.Contains(searchString) || s.ParticipantFirstName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    participants = participants.OrderByDescending(s => s.ParticipantLastName);
                    break;
                default:
                    participants = participants.OrderBy(s => s.ParticipantLastName);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(participants.ToPagedList(pageNumber, pageSize));

            //Original. public ActionResult Index()
            //try
            //{
            //    return View(db.Participants.ToList());
            //}
            //catch (DbEntityValidationException ex)
            //{
            //    foreach (var entityValidationErrors in ex.EntityValidationErrors)
            //    {
            //        foreach (var validationError in entityValidationErrors.ValidationErrors)
            //        {
            //            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
            //        }
            //    }
            //}
            //return View(db.Participants.ToList());
        }



        // GET: Participants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Participant participant = db.Participants.Find(id);
            if (participant == null)
            {
                return HttpNotFound();
            }
            return View(participant);
        }

        // GET: Participants/Create
        public ActionResult Create() 
        {

            return View();
        }

        //public ActionResult Cancel([Bind(Include = "GuardianGuid"),]Participant participant)
        //{
                
        //        return RedirectToAction("Edit", "Guardians", new { GuardianGuid = participant.GuardianGuid });

        //}

        // POST: Participants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ParticipantID,ParticipantFirstName,ParticipantLastName,ParticipantAge,ParticipantSchool,ParticipantTeacher,ClassroomScouting,HealthForm,PhotoAck,AttendingCode,Returning,GuardianID,GuardianGuid,Comments,GuardianGuid,CheckedIn,EventYear"),
            ] Participant participant,string submit)
        {
            //clear form and return to Guardian form
            //if (Request["submit"].Equals("Cancel"))
            //{
            //    ModelState.Clear();
            //    return RedirectToAction("Edit", "Guardians", new { GuardianGuid = participant.GuardianGuid });
            //    //return Cancel(participant);

            //}

             if (ModelState.IsValid)
            {
                if (TempData["myPK"] != null)
                {
                    participant.GuardianID = (int)TempData["myPK"];
                }


                //store year of event
                var thisYear = DateTime.Now.Year.ToString();
                participant.EventYear = int.Parse(thisYear);


                db.Participants.Add(participant);


                try
                {
                    db.SaveChanges();

                    if (Request["submit"].Equals("Add another participant"))
                    //add another participant for guardian
                    { return RedirectToAction("Create", "Participants", new { GuardianGuid = participant.GuardianGuid }); }


                    if (Request["submit"].Equals("Add a family member"))
                    //add a family member
                    { return RedirectToAction("Create", "FamilyMembers", new { GuardianGuid = participant.GuardianGuid }); }

                    if (Request["submit"].Equals("Complete registration"))
                    //registration complete, no more people to add
                    { return RedirectToAction("Registered"); }




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
                return View(participant);
                    }


        // GET: Participants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Participant participant = db.Participants.Find(id);
            if (participant == null)
            {
                return HttpNotFound();
            }
            return View(participant);
        }

        // POST: Participants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var participant = db.Participants.Find(id);

            if (TryUpdateModel(participant, "",
               new string[] { "ParticipantFirstName","ParticipantLastName","ParticipantAge","ParticipantSchool","ParticipantTeacher","ClassroomScouting","HealthForm","PhotoAck","AttendingCode","Returning","Comments","CheckedIn","EventYear"}))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(participant);


        }

        // GET: Participants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Participant participant = db.Participants.Find(id);
            if (participant == null)
            {
                return HttpNotFound();
            }
            return View(participant);
        }

        // POST: Participants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Participant participant = db.Participants.Find(id);
            db.Participants.Remove(participant);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult GetFile(string file) {
            var appData = Server.MapPath("~/App_Data/PDF");
            var path = Path.Combine(appData, file);
            path = Path.GetFullPath(path);
            if (!path.StartsWith(appData)) {
                // Ensure that we are serving file only inside the App_Data folder
                // and block requests outside like "../web.config"
                throw new HttpException(403, "Forbidden");
            }

            if (!System.IO.File.Exists(path)) {
                return HttpNotFound();
            }

            return File(path, MediaTypeNames.Application.Pdf);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Registered()
        {
            return View();
        }

        //public ActionResult Redirect()
        public ActionResult Redirect([Bind(Include = "GuardianID,GuardianGuid"),
            ] Participant participant, string submit)
        { 
                if (TempData["myPK"] != null)
                {
                    participant.GuardianID = (int)TempData["myPK"];
                }
            return View();

        }

    }
}
