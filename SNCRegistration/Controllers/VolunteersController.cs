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
using SNCRegistration.Helpers;
using System.IO;
using System.Net.Mime;

namespace SNCRegistration.Controllers
{
    [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
    public class VolunteersController : Controller
    {
        private SNCRegistrationEntities db = new SNCRegistrationEntities();


        // GET: Volunteers
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? searchYear, int? page)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentYearSort = searchYear;
            ViewBag.currentFilter = currentFilter;
            ViewBag.searchString = searchString;
            ViewBag.page = page;
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
            ViewBag.AllYears = (from y in db.Volunteers select y.EventYear).Distinct();

            //var volunteers = from s in db.Volunteers
            //                 where s.EventYear == searchYear
            //                 select s;

            var volunteers = from s in db.Volunteers
                               join sa in db.BSTypes on s.BSType equals sa.BSTypeID
                               where s.EventYear == searchYear
                               select new LeadContactBST() { volunteer = s, bsttype = sa };


            if (!String.IsNullOrEmpty(searchString))
            {
                volunteers = volunteers.Where(s => s.volunteer.VolunteerLastName.Contains(searchString) || s.volunteer.VolunteerFirstName.Contains(searchString) || s.volunteer.UnitChapterNumber.Contains(searchString) || s.bsttype.BSTypeDescription.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    volunteers = volunteers.OrderByDescending(s => s.volunteer.VolunteerLastName);
                    break;
                case "tcunum_desc":
                    volunteers = volunteers.OrderByDescending(s => s.volunteer.UnitChapterNumber);
                    break;
                case "name_asc":
                    volunteers = volunteers.OrderBy(s => s.volunteer.VolunteerLastName);
                    break;
                case "tcunum_asc":
                    volunteers = volunteers.OrderBy(s => s.volunteer.UnitChapterNumber);
                    break;
                case "tcutype_desc":
                    volunteers = volunteers.OrderByDescending(s => s.volunteer.BSType);
                    break;
                case "tcutype_asc":
                    volunteers = volunteers.OrderBy(s => s.volunteer.BSType);
                    break;
                default:
                    volunteers = volunteers.OrderBy(s => s.volunteer.VolunteerLastName);
                    break;
            }

            int pageSize = 10;
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

            var shirtsize = db.ShirtSizes.Find(volunteer.VolunteerShirtSize);
            ViewBag.shirtsizedesc = shirtsize.ShirtSizeDescription;

            var attendance = db.Attendances.Find(volunteer.VolunteerAttendingCode);
            ViewBag.attendancedesc = attendance.Description;

            var bsttype = db.BSTypes.Find(volunteer.BSType);
            ViewBag.bsttypedec = bsttype.BSTypeDescription;

            var age = db.Ages.Find(volunteer.VolunteerAge);
            ViewBag.agereal = age.AgeDescription;

            if (volunteer == null)
            {
                return HttpNotFound();
            }
            return View(volunteer);
        }


        [OverrideAuthorization]
        public ActionResult Registered()
        {

            return View();
        }

        // GET: Volunteers/Create
        [OverrideAuthorization]
        public ActionResult Create(int LeadContactID)
        {
            //ViewBag.ShirtSizes = new SelectList(db.ShirtSizes.Where(s => s.ShirtSizeCode != "00"), "ShirtSizeCode", "ShirtSizeDescription");
            ViewBag.ShirtSizes = new SelectList(db.ShirtSizes, "ShirtSizeCode", "ShirtSizeDescription");
            ViewBag.Attendance = new SelectList(db.Attendances.Where(i => i.Volunteer == true), "AttendanceID", "Description");
            ViewBag.Age = new SelectList(db.Ages, "AgeID", "AgeDescription");
            ViewBag.BSType = new SelectList(db.BSTypes, "BSTypeID", "BSTypeDescription");
            ViewBag.LeadContactID = LeadContactID;
            return View();
        }

        // POST: Volunteers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [OverrideAuthorization]
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VolunteerID,VolunteerFirstName,VolunteerLastName,VolunteerAge,LeadContactID,VolunteerShirtOrder,VolunteerShirtSize,VolunteerAttendingCode,SaturdayDinner,UnitChapterNumber,Comments, LeaderGuid")] Volunteer volunteer)
        {

            if (ModelState.IsValid)
            {

                db.Volunteers.Add(volunteer);
                var thisYear = DateTime.Now.Year.ToString();
                volunteer.EventYear = int.Parse(thisYear);
                var fee = 0;
                volunteer.VolunteerFee = fee;

                try
                {
                    db.SaveChanges();

                    this.Session["lSession"] = volunteer.LeadContactID;

                    this.Session["gSession"] = volunteer.LeaderGuid;

                    if (Request["submit"].Equals("View volunteers I have registered"))
                    { return RedirectToAction("VolunteersRegisteredView", "Volunteers", new { LeadContactId = Session["lSession"] }); }

                    if (Request["submit"].Equals("Add an additional volunteer"))
                    { return RedirectToAction("Create", "Volunteers", new { LeadContactId = this.Session["lSession"] }); }

                    //if (Request["submit"].Equals("Cancel"))              
                    //{ return RedirectToAction("Redirect", new { LeaderGuid = volunteer.LeaderGuid }); }

                    if (Request["submit"].Equals("Cancel"))
                    { return RedirectToAction("Redirect", new { LeaderGuid = volunteer.LeaderGuid }); }

                    if (Request["submit"].Equals("Complete registration"))
                    //registration complete, no more people to add
                    {
                        var total = db.ComputeTotal(volunteer.LeadContactID);
                        var email = Session["leaderEmail"] as string;
                        var body = "You have successfully registered for the Special Needs Camporee.The total fee due is " + total.ToString("c") + "<br />" + "Your registered volunteers are:" + "<br />" + db.GetVolunteerList(volunteer.LeadContactID);
                        Helpers.EmailHelpers.SendVolEmail("sncracc@gmail.com", email, "Registration Confirmation", body, Server.MapPath("~/App_Data/PDF/"));
                        return RedirectToAction("Registered");
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
        [OverrideAuthorization]
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = db.Volunteers.Find(id);

            SetAgeAttendanceViewBag(volunteer.BSType, volunteer.VolunteerAge, volunteer.VolunteerAttendingCode, volunteer.VolunteerShirtSize);

            if (volunteer == null)
            {
                return HttpNotFound();
            }
            ViewBag.VolunteerID = new SelectList(db.Volunteers, "VolunteerID", "VolunteerFirstName", volunteer.VolunteerID);
            ViewBag.VolunteerID = new SelectList(db.Volunteers, "VolunteerID", "VolunteerFirstName", volunteer.VolunteerID);
            return View(volunteer);
        }

        [OverrideAuthorization]
        public ActionResult GetFile(string file)
        {
            var appData = Server.MapPath("~/App_Data/PDF");
            var path = Path.Combine(appData, file);
            path = Path.GetFullPath(path);
            if (!path.StartsWith(appData))
            {
                // Ensure that we are serving file only inside the App_Data folder
                // and block requests outside like "../web.config"
                throw new HttpException(403, "Forbidden");
            }

            if (!System.IO.File.Exists(path))
            {
                return HttpNotFound();
            }

            return File(path, MediaTypeNames.Application.Pdf);
        }

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
            var volunteer = db.Volunteers.Find(id);

            if (TryUpdateModel(volunteer, "",
               new string[] { "VolunteerID", "VolunteerFirstName", "VolunteerLastName", "VolunteerAge", "LeadContactID", "VolunteerShirtOrder", "VolunteerShirtSize", "VolunteerAttendingCode", "SaturdayDinner", "UnitChapterNumber", "CheckedIn", "Comments" }))
            {
                try
                {
                    db.SaveChanges();
                    TempData["notice"] = "Edits Saved!";
                    return RedirectToAction("Details", "Volunteers", new { id = volunteer.VolunteerID });
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            SetAgeAttendanceViewBag(volunteer.BSType, volunteer.VolunteerAge, volunteer.VolunteerAttendingCode, volunteer.VolunteerShirtSize);
            return View(volunteer);


        }


        // GET: Volunteer/Checkin/5
        public ActionResult CheckIn(int? id, string returnUrl)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = db.Volunteers.Find(id);

            var shirtsize = db.ShirtSizes.Find(volunteer.VolunteerShirtSize);
            ViewBag.shirtsizedesc = shirtsize.ShirtSizeDescription;

            var attendance = db.Attendances.Find(volunteer.VolunteerAttendingCode);
            ViewBag.attendancedesc = attendance.Description;

            var bsttype = db.BSTypes.Find(volunteer.BSType);
            ViewBag.bsttypedec = bsttype.BSTypeDescription;

            var age = db.Ages.Find(volunteer.VolunteerAge);
            ViewBag.agereal = age.AgeDescription;

            if (volunteer == null)
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

            return View(volunteer);
        }

        // POST: VolunteerCheckIn/5
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
            var volunteer = db.Volunteers.Find(id);


            if (TryUpdateModel(volunteer, "",
               new string[] { "CheckedIn"}))
            {
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index", new { SearchString = Session["SessionSearchString"], sortOrder = Session["SessionSortOrder"], currentFilter = Session["SessionCurrentFilter"], searchYear = Session["SessionSearchYear"], page = Session["SessionPage"] });

                    //if (!String.IsNullOrEmpty(returnUrl))
                    //    return Redirect(returnUrl);
                    //else
                    //    return RedirectToAction("Index");
                }
                catch (NullReferenceException ex)
                {
                    Response.Write("Processor Usage" + ex.Message);
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
        [OverrideAuthorization]
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin")]
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
        [OverrideAuthorization]
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Volunteer volunteer = db.Volunteers.Find(id);
            int? prevID = volunteer.LeadContactID;
            db.Volunteers.Remove(volunteer);
            db.SaveChanges();
            return RedirectToAction("Details", "LeadContacts", new { id = prevID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Volunteers/AddAdditionalVolunteer
        public ActionResult AddAdditionalVolunteer()
        {
            ViewBag.ShirtSizes = new SelectList(db.ShirtSizes, "ShirtSizeCode", "ShirtSizeDescription");
            ViewBag.Attendance = new SelectList(db.Attendances.Where(i => i.Volunteer == true), "AttendanceID", "Description");
            ViewBag.Age = new SelectList(db.Ages, "AgeID", "AgeDescription");

            ViewBag.leadGuid = Session["lGuidSession"];
            ViewBag.leadID = Session["lIDSession"];
            return View();
        }

        // POST: Volunteers/AddAdditionalVolunteer
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAdditionalVolunteer([Bind(Include = "VolunteerID,VolunteerFirstName,VolunteerLastName,VolunteerAge,LeadContactID,VolunteerShirtOrder,VolunteerShirtSize,VolunteerAttendingCode,SaturdayDinner,UnitChapterNumber,Comments, LeaderGuid")] Volunteer volunteer)
        {

            if (ModelState.IsValid)
            {


                var thisYear = DateTime.Now.Year.ToString();
                volunteer.EventYear = int.Parse(thisYear);
                db.Volunteers.Add(volunteer);
                var fee = 0;
                volunteer.VolunteerFee = fee;

                try
                {

                    this.Session["lIDSession"] = volunteer.LeadContactID;
                    this.Session["lGuidSession"] = volunteer.LeaderGuid;
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
            ViewBag.ShirtSizes = new SelectList(db.ShirtSizes, "ShirtSizeCode", "ShirtSizeDescription");
            ViewBag.Attendance = new SelectList(db.Attendances.Where(i => i.Volunteer == true), "AttendanceID", "Description");
            ViewBag.Age = new SelectList(db.Ages, "AgeID", "AgeDescription");

            return RedirectToAction("Details", "LeadContacts", new { id = volunteer.LeadContactID });
        }


        private void SetAgeAttendanceViewBag(int? bstgroup = null, int ? age = null, int? attendance = null, string shirtSize = null)
        {

            if (bstgroup == null)
            {
                ViewBag.bstID = new SelectList(db.BSTypes, "BSTypeID", "BSTypeDescription");
            }
            else
                ViewBag.bstID = new SelectList(db.BSTypes.ToArray(), "BSTypeID", "BSTypeDescription", bstgroup);

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
                ViewBag.AttendanceID = new SelectList(db.Attendances.Where(i => i.Participant == true), "AttendanceID", "Description", attendance);

            if (shirtSize == null)
            {
                ViewBag.shirtSizeName = new SelectList(db.ShirtSizes, "ShirtSizeCode", "ShirtSizeDescription");
            }
            else
                ViewBag.shirtSizeName = new SelectList(db.ShirtSizes.ToArray(), "ShirtSizeCode", "ShirtSizeDescription", shirtSize);


        }


        //[OverrideAuthorization]
        //public ActionResult Redirect([Bind(Include = "VolunteerFirstName, VolunteerLastName"),
        //    ] Volunteer volunteer, string submit)
        //{
        //    ModelState.Remove("VolunteerFirstName");
        //    ModelState.Remove("VolunteerLastName");
        //    if (ModelState.IsValid)
        //    {
        //        if (TempData["myPK"] != null)
        //        {
        //            volunteer.LeadContactID = (int)TempData["myPK"];

        //        }

        //        //pass the guardianID to child form as FK                    
        //        TempData["myPK"] = volunteer.LeadContactID;
        //        TempData.Keep();




        //        //store year of event
        //        var thisYear = DateTime.Now.Year.ToString();
        //        volunteer.EventYear = int.Parse(thisYear);

        //        if (Request["submit"].Equals("Add an additional volunteer"))
        //        { return RedirectToAction("Create", "Volunteers", new { LeadContactGuid = volunteer.LeaderGuid }); }

        //        if (Request["submit"].Equals("Complete registration"))
        //        //registration complete, no more people to add
        //        {
        //            var total = db.ComputeTotal(volunteer.LeadContactID);
        //            var email = Session["leaderEmail"] as string;
        //            var body = "You have successfully registered for the Special Needs Camporee.The total fee due is " + total.ToString("c") + "<br />" + "Your registered volunteers are:" + "<br />" + db.GetVolunteerList(volunteer.LeadContactID);
        //            Helpers.EmailHelpers.SendVolEmail("sncracc@gmail.com", email, "Registration Confirmation", body, Server.MapPath("~/App_Data/PDF/"));
        //            return Redirect("Registered");
        //        }

        //    }


        //    return View();

        //}

        [OverrideAuthorization]
        public new ActionResult Redirect(string submit)
        {
            int leadContactID = 0;
            if (TempData["myPK"] != null)
            {
                leadContactID = (int)TempData["myPK"];

            }

            ViewBag.LeadContactID = leadContactID;

            return View();
        }

        // GET: Volunteers/Create
        [OverrideAuthorization]
        public ActionResult VolunteersRegisteredView(int LeadContactID)
        {

            ViewBag.LeadContactID = LeadContactID;

            //var vols = from vol in db.Volunteers where vol.LeadContactID == LeadContactID select vol;
            var vols = (from vol in db.Volunteers where vol.LeadContactID == LeadContactID select vol).Take(1000);
            //var vols = (from v in db.Volunteers where v.VolunteerAge == 1  select v).Take(1000);
            return View(vols.ToList());
        }

        [OverrideAuthorization]
        [HttpPost]
        public ActionResult VolunteersRegisteredView(int LeadContactID, string submit)
        {

            if (Request["submit"].Equals("Continue adding volunteers"))
            {
                return RedirectToAction("Create", "Volunteers", new { LeadContactID = LeadContactID });
            }

            if (Request["submit"].Equals("Complete registration"))
            //registration complete, no more people to add
            {
                var total = db.ComputeTotal(LeadContactID);
                var email = Session["leaderEmail"] as string;
                var body = "You have successfully registered for the Special Needs Camporee.The total fee due is " + total.ToString("c") + "<br />" + "Your registered volunteers are:" + "<br />" + db.GetVolunteerList(LeadContactID);
                Helpers.EmailHelpers.SendVolEmail("sncracc@gmail.com", email, "Registration Confirmation", body, Server.MapPath("~/App_Data/PDF/"));
                return RedirectToAction("Registered");
            }



            return View();

        }

    }


}
