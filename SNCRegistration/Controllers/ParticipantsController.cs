﻿using System.Data;
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
    [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
    public class ParticipantsController : Controller
    {
        private SNCRegistrationEntities db = new SNCRegistrationEntities();

        // GET: Participants. For the Index
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? searchYear, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.currentFilter = currentFilter;
            ViewBag.CurrentYearSort = searchYear;
            ViewBag.searchString = searchString;
            ViewBag.page = page;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

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
            ViewBag.AllYears = (from y in db.Participants select y.EventYear).Distinct();

            var participants = from s in db.Participants
                               where s.EventYear == searchYear
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

        }



        // GET: Participants/Details/5
        public ActionResult Details(int? id)
        {

         
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = new GuardianParticipantFamily();

            model.participant = db.Participants.Find(id);
            model.relatedparticipants = db.Participants.Where(i => i.GuardianID == model.participant.GuardianID && i.ParticipantID != id);
            model.guardians = db.Guardians.Where(i => i.GuardianID == model.participant.GuardianID);
            model.familymembers = db.FamilyMembers.Where(i => i.GuardianID == model.participant.GuardianID);
            model.age = db.Ages.Find(model.participant.ParticipantAge);
            model.attendance = db.Attendances.Find(model.participant.AttendingCode);


            ViewBag.agedesc = model.age.AgeDescription;
            ViewBag.attend = model.attendance.Description;


   //             string.Equals(db.Ages, "AgeID", "AgeDescription");

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);

        }

        // GET: Participants/Create
        [OverrideAuthorization]
        public ActionResult Create() 
        {
            ViewBag.ParticipantAge = new SelectList(db.Ages, "AgeID", "AgeDescription");
            ViewBag.Attendance = new SelectList(db.Attendances.Where(i => i.Participant == true), "AttendanceID", "Description",TempData["gAttend"]);

            return View();
        }


        // POST: Participants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [OverrideAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ParticipantID,ParticipantFirstName,ParticipantLastName,ParticipantAge,ParticipantSchool,ParticipantTeacher,ClassroomScouting,HealthForm,PhotoAck,AttendingCode,Returning,GuardianID,GuardianGuid,Comments,GuardianGuid,CheckedIn,EventYear"),
            ] Participant participant, string submit)
        {


             if (ModelState.IsValid)
            {
                if (TempData["myPK"] != null)
                {
                    participant.GuardianID = (int)TempData["myPK"];
                    TempData["gAttend"] = participant.AttendingCode;
                    TempData.Keep();
                }


                //store year of event
                var thisYear = DateTime.Now.Year.ToString();
                participant.EventYear = int.Parse(thisYear);


                db.Participants.Add(participant);


                try
                {
                    db.SaveChanges();

                    TempData["gAttend"] = participant.AttendingCode;

                    this.Session["gSession"] = participant.GuardianGuid;


                    if (Request["submit"].Equals("Add another participant"))
                    //add another participant for guardian
                    { return RedirectToAction("Create", "Participants", new { GuardianGuid = participant.GuardianGuid}); }


                    if (Request["submit"].Equals("Add a family member"))
                    //add a family member
                    { return RedirectToAction("Create", "FamilyMembers", new { GuardianGuid = participant.GuardianGuid }); }

                    if(Request["submit"].Equals("Cancel"))
                    { return RedirectToAction("Redirect", new { GuardianGuid = participant.GuardianGuid }); }

                    if (Request["submit"].Equals("Complete registration"))
                    //registration complete, no more people to add
                    {
                        var email = Session["pEmail"] as string;
                        //to do: remove password
                        Helpers.EmailHelpers.SendEmail("sncracc@gmail.com", email, "Registration Confirmation", "You have successfully registered for the Special Needs Camporee. Please complete and return the required forms.  We look forward to seeing you!!", Server.MapPath("~/App_Data/PDF/"));
                        return Redirect("Registered");
                    }
        
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }

            }
                return View(participant);
                    }


        // GET: Participants/Edit/5
        [OverrideAuthorization]
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin")]
        public ActionResult Edit(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Participant participant = db.Participants.Find(id);

            SetAgeAttendanceViewBag(participant.ParticipantAge, participant.AttendingCode);

            if (participant == null)
            {
                return HttpNotFound();
            }
            return View(participant);


        }

        // POST: Participants/Edit/5
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
            var participant = db.Participants.Find(id);

            if (TryUpdateModel(participant, "",
               new string[] { "ParticipantFirstName","ParticipantLastName","ParticipantAge","ParticipantSchool","ParticipantTeacher","ClassroomScouting","HealthForm","PhotoAck","AttendingCode","Returning","Comments","CheckedIn","EventYear"}))
            {
                try
                {
                    db.SaveChanges();
                    TempData["notice"] = "Edits Saved!";
                    return RedirectToAction("Edit", "Participants", new { id = participant.ParticipantID });
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            SetAgeAttendanceViewBag(participant.ParticipantAge, participant.AttendingCode);

            return View(participant);


        }

        // GET: Participants/CheckIn/5
        public ActionResult CheckIn(int? id, string returnUrl)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Participant participant = db.Participants.Find(id);

            var age = db.Ages.Find(participant.ParticipantAge);

            ViewBag.agedesc = age.AgeDescription;

            var attend = db.Attendances.Find(participant.AttendingCode);

            ViewBag.attenda = attend.Description;



            if (participant == null)
            {
                return HttpNotFound();
            }
            if (String.IsNullOrEmpty(returnUrl)
               && Request.UrlReferrer != null
               && Request.UrlReferrer.ToString().Length > 0)
            {
                return RedirectToAction("CheckIn",
                    new { returnUrl = Request.UrlReferrer.ToString() });
            }
            return View(participant);
        }

        // POST: Participants/CheckIn/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("CheckIn")]
        [ValidateAntiForgeryToken]
        public ActionResult CheckInPost(int? id, string returnUrl)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var participant = db.Participants.Find(id);



                if (TryUpdateModel(participant, "",
                    new string[] { "HealthForm", "PhotoAck", "CheckedIn" }))
                {


                if (participant.HealthForm.Value == false && participant.CheckedIn == true)
                {
                    ModelState.AddModelError("", "Health Form must be received before check in.");
                }
                else
                {
                    try
                    {
                        db.SaveChanges();
                        if (!String.IsNullOrEmpty(returnUrl))
                            return Redirect(returnUrl);
                        else
                            return RedirectToAction("Index");

                    }
                    catch (DataException /* dex */)
                    {
                        //Log the error (uncomment dex variable name and add a line here to write a log.
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                    }
                }
            }

            

            return View(participant);


        }

        // GET: Participants/Delete/5
        [OverrideAuthorization]
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin")]
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
        [OverrideAuthorization]
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Participant participant = db.Participants.Find(id);
            int? prevID = participant.GuardianID;

            db.Participants.Remove(participant);
            db.SaveChanges();
            return RedirectToAction("Details", "Guardians", new { id = prevID });
        }

        [OverrideAuthorization]
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

        [OverrideAuthorization]
        //send registration confirmation.  Use in cancel procedure when not on form to publish from there.
        public ActionResult SendConfirm()
        {
            var email = Session["pEmail"] as string;
            //to do: remove password
            Helpers.EmailHelpers.SendEmail("sncracc@gmail.com", email, "Registration Confirmation", "You have successfully registered for the Special Needs Camporee. Please complete and return the required forms.  We look forward to seeing you!!", Server.MapPath("~/App_Data/PDF/"));
            return Redirect("Registered");
        }

        [OverrideAuthorization]
        public ActionResult Registered()
        {

            return View();
        }

        //public ActionResult Redirect()
        [OverrideAuthorization]
        public ActionResult Redirect([Bind(Include = "GuardianID,GuardianGuid"),
            ] Participant participant, string submit)
        {
            if (ModelState.IsValid)
            {
                if (TempData["myPK"] != null)
                {
                    participant.GuardianID = (int)TempData["myPK"];
                    TempData["gAttend"] = participant.AttendingCode;
                    TempData.Keep();
                }

                //pass the guardianID to child form as FK                    
                TempData["myPK"] = participant.GuardianID;
                TempData.Keep();

                


                //store year of event
                var thisYear = DateTime.Now.Year.ToString();
                participant.EventYear = int.Parse(thisYear);

            }


                return View();

        }


        private void SetAgeAttendanceViewBag(int? age = null, int? attendance = null)
        {

            if (age == null)
            {
                ViewBag.AgeID = new SelectList(db.Ages, "AgeID", "AgeDescription");
            }
            else
                ViewBag.AgeID = new SelectList(db.Ages.ToArray(), "AgeID", "AgeDescription", age);

            if (attendance == null)
            {
                ViewBag.AttendanceID = new SelectList(db.Attendances, "AttendanceID", "Description");
            }
            else
                ViewBag.AttendanceID = new SelectList(db.Attendances.Where( i => i.Participant == true), "AttendanceID", "Description", attendance);



        }


        // GET: Participants/AddAdditionalParticipant
        public ActionResult AddAdditionalParticipant(string guardGuid, int guardID)
        {
            ViewBag.ParticipantAge = new SelectList(db.Ages, "AgeID", "AgeDescription");
            ViewBag.Attendance = new SelectList(db.Attendances.Where(i => i.Participant == true), "AttendanceID", "Description");
            ViewBag.guardGuid = Session["gUIDSession"];
            ViewBag.guardID = Session["gIDSession"];
            return View();
        }


        // POST: Participants/AddAdditionalParticipant
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAdditionalParticipant([Bind(Include = "ParticipantID,ParticipantFirstName,ParticipantLastName,ParticipantAge,ParticipantSchool,ParticipantTeacher,ClassroomScouting,HealthForm,PhotoAck,AttendingCode,Returning,GuardianID,GuardianGuid,Comments,GuardianGuid,CheckedIn,EventYear"),
            ] Participant participant, string submit)
        {


            if (ModelState.IsValid)
            {
                //store year of event
                var thisYear = DateTime.Now.Year.ToString();
                participant.EventYear = int.Parse(thisYear);


                db.Participants.Add(participant);


                try
                {

                    this.Session["gUIDSession"] = participant.GuardianGuid;
                    this.Session["gIDSession"] = participant.GuardianID;
                    
                    db.SaveChanges();


                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }



            }
            ViewBag.ParticipantAge = new SelectList(db.Ages, "AgeID", "AgeDescription");
            ViewBag.Attendance = new SelectList(db.Attendances.Where(i => i.Participant == true), "AttendanceID", "Description");
            return RedirectToAction("Details", "Guardians", new { id = participant.GuardianID });
        }


    }
}
