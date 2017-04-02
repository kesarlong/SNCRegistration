using System.Linq;
using System.Net;
using System.Web.Mvc;
using SNCRegistration.ViewModels;
using System.Data.Entity.Validation;
using System;
using System.Data;

namespace SNCRegistration.Controllers
{
    [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
    public class FamilyMembersController : Controller
    {
        private SNCRegistrationEntities db = new SNCRegistrationEntities();

        // GET: FamilyMembers
        public ActionResult Index()
        {
            return View(db.FamilyMembers.ToList());
        }

        // GET: FamilyMembers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FamilyMember familyMember = db.FamilyMembers.Find(id);
            if (familyMember == null)
            {
                return HttpNotFound();
            }
            return View(familyMember);
        }

        // GET: FamilyMembers/Create
        [OverrideAuthorization]
        public ActionResult Create()
        {

            ViewBag.FamilyMemberAge = new SelectList(db.Ages, "AgeID", "AgeDescription");
            ViewBag.AttendingCode = new SelectList(db.Attendances.Where(i => i.Participant == true), "AttendanceID", "Description");
            return View();
        }

        // POST: FamilyMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [OverrideAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FamilyMemberID,FamilyMemberFirstName,FamilyMemberLastName,FamilyMemberAge,GuardianID,HealthForm,PhotoAck,AttendingCode,Comments,GuardianGuid")] FamilyMember familyMember)
        {
            if (ModelState.IsValid)
            {

                if (TempData["myPK"] != null)
                {
                    familyMember.GuardianID = (int)TempData["myPK"];
                }

                //store year of event
                var thisYear = DateTime.Now.Year.ToString();
                familyMember.EventYear = int.Parse(thisYear);

                db.FamilyMembers.Add(familyMember);

                try
                {




                    db.SaveChanges();

                    //add another participant for guardian                   
                    if (Request["submit"].Equals("Add another participant"))
                    { return RedirectToAction("Create", "Participants", new { GuardianGuid = familyMember.GuardianGuid }); }

                    //add a family member
                    if (Request["submit"].Equals("Add a family member"))
                    { return RedirectToAction("Create", "FamilyMembers", new { GuardianGuid = familyMember.GuardianGuid }); }

                    if (Request["submit"].Equals("Cancel"))
                    { return RedirectToAction("Redirect", "Participants", new { GuardianGuid = familyMember.GuardianGuid }); }

                    //registration complete, no more people to add
                    if (Request["submit"].Equals("Complete registration"))
                    {
                        var email = Session["pEmail"] as string;
                        //to do: remove password
                        Helpers.EmailHelpers.SendEmail("sncracc@gmail.com", email, "Registration Confirmation", "You have successfully registered for the Special Needs Camporee. Please complete and return the required forms.  We look forward to seeing you!!");
                        return Redirect("Registered");
                    }

                }
                catch (DbEntityValidationException ex)
                //{
                //    //retrieve the error message as a list of strings
                //    var errorMessages = ex.EntityValidationErrors
                //        .SelectMany(x => x.ValidationErrors)
                //        .Select(x => x.ErrorMessage);

                //    //Join the list to a single string
                //    var fullErrorMessage = string.Join(" ,", errorMessages);

                //    //Combine the original exception message wtih the new one
                //    var exceptionMessage = string.Concat(ex.Message, "The validation errors are: ", fullErrorMessage);

                //    // Throw a new DbEntityValidationException with the improved exception message.
                //    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                //}
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

            return View(familyMember);
        }

        // GET: FamilyMembers/Edit/5
        [OverrideAuthorization]
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FamilyMember familyMember = db.FamilyMembers.Find(id);
            if (familyMember == null)
            {
                return HttpNotFound();
            }
            return View(familyMember);
        }


        //Original POST Edit, Delete if nothing broken
        // POST: FamilyMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "FamilyMemberID,FamilyMemberFirstName,FamilyMemberLastName,GuardianID,HealthForm,PhotoAck,AttendingCode,CheckedIn,Comments")] FamilyMember familyMember)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(familyMember).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(familyMember);
        //}

        // POST: FamilyMembers/Edit/5
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
            var familymember = db.FamilyMembers.Find(id);

            if (TryUpdateModel(familymember, "",
               new string[] { "FamilyMemberID","FamilyMemberFirstName","FamilyMemberLastName","GuardianID","HealthForm","PhotoAck","AttendingCode","CheckedIn","Comments" }))
            {
                try
                {
                    db.SaveChanges();

                    TempData["notice"] = "Edits Saved.";

                    //return RedirectToAction("Details", "Guardians", new { id = guardian.GuardianID });
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(familymember);


        }


        // GET: FamilyMembers/Delete/5
        [OverrideAuthorization]
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FamilyMember familyMember = db.FamilyMembers.Find(id);
            if (familyMember == null)
            {
                return HttpNotFound();
            }
            return View(familyMember);
        }

        // POST: FamilyMembers/Delete/5
        [OverrideAuthorization]
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FamilyMember familyMember = db.FamilyMembers.Find(id);
            db.FamilyMembers.Remove(familyMember);
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

        [OverrideAuthorization]
        public ActionResult Registered()
        {
            return View();
        }

        public ActionResult Redirect([Bind(Include = "GuardianID,GuardianGuid"),
            ] FamilyMember familyMember, string submit)
        {
            if (ModelState.IsValid)
            {
                if (TempData["myPK"] != null)
                {
                    familyMember.GuardianID = (int)TempData["myPK"];

                }

                //pass the guardianID to child form as FK                    
                TempData["myPK"] = familyMember.GuardianID;
                TempData.Keep();


                //store year of event
                var thisYear = DateTime.Now.Year.ToString();
                familyMember.EventYear = int.Parse(thisYear);

            }
            return View();

        }

        // GET: FamilyMembers/CheckIn/5
        public ActionResult CheckIn(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FamilyMember familymember = db.FamilyMembers.Find(id);
            if (familymember == null)
            {
                return HttpNotFound();
            }
            return View(familymember);

        }

        // POST: FamilyMembers/CheckIn/5
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
            var familymember = db.FamilyMembers.Find(id);


            if (TryUpdateModel(familymember, "",
               new string[] { "CheckedIn" }))
            {
                try
                {
                    db.SaveChanges();

                    TempData["notice"] = "Check In Status Saved!";
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(familymember);


        }






    }
}
