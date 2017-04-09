using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SNCRegistration.ViewModels;
using System.IO;
using System.Net.Mime;
using System.Data.Entity.Validation;
using System;
using PagedList;
using System.Data;

namespace SNCRegistration.Controllers

{

    [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
    public class GuardiansController : Controller
	{
		private SNCRegistrationEntities db = new SNCRegistrationEntities();


		// GET: Guardians
		public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? searchYear, int? page)
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
            ViewBag.AllYears = (from y in db.Guardians select y.EventYear).Distinct();


            var guardians = from s in db.Guardians
                            where s.EventYear == searchYear
                            select s;

			if (!String.IsNullOrEmpty(searchString))
			{
				guardians = guardians.Where(s => s.GuardianLastName.Contains(searchString) || s.GuardianFirstName.Contains(searchString));
			}


            switch (sortOrder)
			{
				case "name_desc":
					guardians = guardians.OrderByDescending(s => s.GuardianLastName);
					break;
				default:
					guardians = guardians.OrderBy(s => s.GuardianLastName);
					break;
			}

			int pageSize = 5;
			int pageNumber = (page ?? 1);
			return View(guardians.ToPagedList(pageNumber, pageSize));

		}

		// GET: Guardians/Details/5
		public ActionResult Details(int? id)
		{


			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var model = new GuardianParticipantFamily();

			model.guardian = db.Guardians.Find(id);
			model.participants = db.Participants.Where(i => i.GuardianID == id);
			model.familymembers = db.FamilyMembers.Where(i => i.GuardianID == id);

            this.Session["gUIDSession"] = model.guardian.GuardianGuid;
            this.Session["gIDSession"] = model.guardian.GuardianID;

            if (model == null)
			{
				return HttpNotFound();
			}

			return View(model);


		}


        // GET: Guardians/Create
        [OverrideAuthorization]
        public ActionResult Create()

		{
			ViewBag.Relationship = new SelectList(db.Relationships, "RelationshipCode", "RelationshipDescription");
			ViewBag.Attendance = new SelectList(db.Attendances.Where(i=>i.Participant==true), "AttendanceID", "Description");
			return View();
		}

		public ActionResult GetYear()
		{
			return View("ActiveRegistrationYear");
		}


        // POST: Guardians/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [OverrideAuthorization]
        [HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "GuardianID,GuardianFirstName,GuardianLastName,GuardianAddress,GuardianCity,GuardianState,GuardianZip,GuardianCellPhone,GuardianEmail,PacketSentDate,ReceiptDate,ConfirmationSentDate,HealthForm,PhotoAck,Tent,AttendingCode,Comments,Relationship,GuardianGuid,NumberInTent,CheckedIn")] Guardian guardian)
		{
			if (ModelState.IsValid)
				
			{

				db.Guardians.Add(guardian);

				//create GUID for record ID
				var myGuid = Guid.NewGuid().ToString();
				guardian.GuardianGuid = myGuid;

				//store year of event
				var thisYear = DateTime.Now.Year.ToString();
				guardian.EventYear = int.Parse(thisYear);


				try
				{  
					db.SaveChanges();

					//added back for cancel feature
					this.Session["gSession"] = guardian.GuardianGuid;

					//pass the guardianID to child form as FK                    
					TempData["myPK"] = guardian.GuardianID;
                    TempData["gAttend"] = guardian.AttendingCode;
					TempData.Keep();

					this.Session["pEmail"] = guardian.GuardianEmail;

					return RedirectToAction("Create", "Participants", new { GuardianGuid = this.Session["gSession"] });

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
			
			return View(guardian);
		}

        // GET: Guardians/Edit/5
        [OverrideAuthorization]
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin")]
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Guardian guardian = db.Guardians.Find(id);

            SetRelationshipAttendingViewBag(guardian.Relationship, guardian.AttendingCode);

            if (guardian == null)
			{
				return HttpNotFound();
			}
			return View(guardian);
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
        
        // POST: Guardians/Edit/5
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
			var guardian = db.Guardians.Find(id);

			if (TryUpdateModel(guardian, "",
			   new string[] { "GuardianID", "GuardianFirstName", "GuardianLastName","GuardianAddress","GuardianState","GuardianCity","GuardianZip","GuardianCellPhone","GuardianEmail","PacketSentDate","ReceiptDate","ConfirmationSentDate","HealthForm","PhotoAck","Tent","AttendingCode","CheckedIn", "Comments","Relationship" }))
			{
				try
				{
					db.SaveChanges();

                    TempData["notice"] = "Edits Saved!";

                    return RedirectToAction("Edit", "Guardians", new { id = guardian.GuardianID });
                }
				catch (DataException /* dex */)
				{
					//Log the error (uncomment dex variable name and add a line here to write a log.
					ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
				}
			}

            SetRelationshipAttendingViewBag(guardian.Relationship, guardian.AttendingCode);
            return View(guardian);


		}


		// GET: Guardians/CheckIn/5
		public ActionResult CheckIn(int? id)
		{

			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Guardian guardian = db.Guardians.Find(id);
			if (guardian == null)
			{
				return HttpNotFound();
			}
			return View(guardian);

		}

		// POST: Guardians/CheckIn/5
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
		   var guardian = db.Guardians.Find(id);




            if (TryUpdateModel(guardian, "",
			   new string[] { "HealthForm", "PhotoAck", "CheckedIn" }))
			{


                if (guardian.CheckedIn == true && guardian.HealthForm.Value == false)
                {
                    ModelState.AddModelError("", "Health Form must be received before check in.");
                    return View(guardian);
                }
                else
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
            }
            return View(guardian);


		}


        // GET: Guardians/Delete/5
        [OverrideAuthorization]
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin")]
        public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Guardian guardian = db.Guardians.Find(id);
			if (guardian == null)
			{
				return HttpNotFound();
			}
			return View(guardian);
		}

        // POST: Guardians/Delete/5
        [OverrideAuthorization]
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin")]
        [HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Guardian guardian = db.Guardians.Find(id);


            try
            {
                db.Guardians.Remove(guardian);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception err)
            {

                ModelState.AddModelError("DBerror", "Unable to Delete Guardian. Please delete associated participant and family member records first before deleting Guardian.");
            }



			return View(guardian);
		}


        private void SetRelationshipAttendingViewBag(int? relationship = null, int? attendance = null)
        {

            if (relationship == null)
            {
                ViewBag.relationshipID = new SelectList(db.Relationships, "RelationshipCode", "RelationshipDescription");
            }
            else
                ViewBag.relationshipID = new SelectList(db.Relationships.ToArray(), "RelationshipCode", "RelationshipDescription", relationship);

            if (attendance == null)
            {
                ViewBag.AttendanceID = new SelectList(db.Attendances, "AttendanceID", "Description");
            }
            else
                ViewBag.AttendanceID = new SelectList(db.Attendances.Where(i => i.Participant == true), "AttendanceID", "Description", attendance);
            
        }

    }
}
