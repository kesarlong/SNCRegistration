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

            var attend = db.Attendances.Find(familyMember.AttendingCode);

            ViewBag.attende = attend.Description;


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
            ViewBag.AttendingCode = new SelectList(db.Attendances.Where(i => i.Participant == true), "AttendanceID", "Description", TempData["gAttend"]);
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
                    
                    //set attendance choice to pass to family member form
                    TempData["gAttend"] = familyMember.AttendingCode;
                    TempData.Keep();
                }

                
                //store year of event
                var thisYear = DateTime.Now.Year.ToString();
                familyMember.EventYear = int.Parse(thisYear);



                db.FamilyMembers.Add(familyMember);

                try
                {
                    db.SaveChanges();

                    TempData["gAttend"] = familyMember.AttendingCode;

                    this.Session["gSession"] = familyMember.GuardianGuid;

    
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
                catch (NullReferenceException ex)
                {
                    Response.Write("Processor Usage" + ex.Message);
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
            int? prevID = familyMember.GuardianID;
            db.FamilyMembers.Remove(familyMember);
            db.SaveChanges();
            return RedirectToAction("Details", "Guardians", new { id = prevID });
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

        [OverrideAuthorization]
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
        public ActionResult CheckIn(int? id, string returnUrl)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FamilyMember familymember = db.FamilyMembers.Find(id);
            var attend = db.Attendances.Find(familymember.AttendingCode);

            ViewBag.attende = attend.Description;
            if (familymember == null)
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

            return View(familymember);

        }

        // POST: FamilyMembers/CheckIn/5
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
            var familymember = db.FamilyMembers.Find(id);





                if (TryUpdateModel(familymember, "",
               new string[] { "HealthForm", "PhotoAck", "CheckedIn" }))
                {

                if (familymember.HealthForm.Value == false && familymember.CheckedIn == true)
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
            return View(familymember);


        }


        // GET: FamilyMembers/AddAdditionalFamily
        [OverrideAuthorization]
        public ActionResult AddAdditionalFamily()
        {

            ViewBag.FamilyMemberAge = new SelectList(db.Ages, "AgeID", "AgeDescription");
            ViewBag.AttendingCode = new SelectList(db.Attendances.Where(i => i.Participant == true), "AttendanceID", "Description");
            ViewBag.guardGuid = Session["gUIDSession"];
            ViewBag.guardID = Session["gIDSession"];
            return View();
        }

        // POST: FamilyMembers/AddAdditionalFamily
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [OverrideAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAdditionalFamily([Bind(Include = "FamilyMemberID,FamilyMemberFirstName,FamilyMemberLastName,FamilyMemberAge,GuardianID,HealthForm,PhotoAck,AttendingCode,Comments,GuardianGuid")] FamilyMember familyMember)
        {
            if (ModelState.IsValid)
            {

             
                //store year of event
                var thisYear = DateTime.Now.Year.ToString();
                familyMember.EventYear = int.Parse(thisYear);

                db.FamilyMembers.Add(familyMember);

                try
                {

                    ViewBag.guardGuid = Session["gUIDSession"];
                    ViewBag.guardID = Session["gIDSession"];
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
            ViewBag.FamilyMemberAge = new SelectList(db.Ages, "AgeID", "AgeDescription");
            ViewBag.AttendingCode = new SelectList(db.Attendances.Where(i => i.Participant == true), "AttendanceID", "Description");
            return RedirectToAction("Details", "Guardians", new { id = familyMember.GuardianID });
        }




    }
}
