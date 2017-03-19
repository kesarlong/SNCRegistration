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
    public class GuardiansController : Controller
    {
        private SNCRegistrationEntities db = new SNCRegistrationEntities();


        // GET: Guardians
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
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

            var guardians = from s in db.Guardians
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


            // Original Index code. Delete if nothing broken.
            //try
            //{
            //    return View(db.Guardians.ToList());
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
            //return View(db.Guardians.ToList());
        }

        // GET: Guardians/Details/5
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
        public ActionResult Details(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Guardian guardian = db.Guardians.Find(id);
            //if (guardian == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(guardian);



            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = new GuardianParticipantFamily();

            model.guardian = db.Guardians.Find(id);
            model.participants = db.Participants.Where(i => i.GuardianID == id);
            model.familymembers = db.FamilyMembers.Where(i => i.GuardianID == id);

            return View(model);


        }

        // GET: Guardians/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult GetYear()
        {
            return View("ActiveRegistrationYear");
        }


        // POST: Guardians/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

                    //pass the guardianID to child form as FK                    
                    TempData["myPK"] = guardian.GuardianID;
                    TempData.Keep();

                    return RedirectToAction("Create", "Participants", new { GuardianGuid = guardian.GuardianGuid });

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
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
        public ActionResult Edit(int? id)
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

        // Original Edit, delete these comments if nothing is broken
        // POST: Guardians/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "GuardianID,GuardianFirstName,GuardianLastName,GuardianAddress,GuardianCity,GuardianZip,GuardianCellPhone,GuardianEmail,PacketSentDate,ReceiptDate,ConfirmationSentDate,HealthForm,PhotoAck,Tent,AttendingCode,Comments,Relationship")] Guardian guardian)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(guardian).State = EntityState.Modified;
        //        db.SaveChanges();
        //        //return RedirectToAction("Index");
        //    }
        //    return View(guardian);
        //}




        // POST: Guardians/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
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
               new string[] { "GuardianID", "GuardianFirstName", "GuardianLastName","GuardianAddress","GuardianCity","GuardianZip","GuardianCellPhone","GuardianEmail","PacketSentDate","ReceiptDate","ConfirmationSentDate","HealthForm","PhotoAck","Tent","AttendingCode","CheckedIn", "Comments","Relationship" }))
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
            return View(guardian);


        }


        // GET: Guardians/CheckIn/5
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
        public ActionResult CheckIn(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guardian guardian = db.Guardians.Find(id);
            if ( guardian== null)
            {
                return HttpNotFound();
            }
            return View(guardian);
        }

        // POST: Guardians/CheckIn/5
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
            var guardian = db.Guardians.Find(id);

            if (TryUpdateModel(guardian, "",
               new string[] { "GuardianID", "GuardianFirstName", "GuardianLastName", "GuardianAddress", "GuardianCity", "GuardianZip", "GuardianCellPhone", "GuardianEmail", "PacketSentDate", "ReceiptDate", "ConfirmationSentDate", "HealthForm", "PhotoAck", "Tent", "AttendingCode", "CheckedIn", "Comments", "Relationship" }))
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
            return View(guardian);


        }

        // GET: Guardians/Delete/5
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Guardian guardian = db.Guardians.Find(id);
            db.Guardians.Remove(guardian);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        
    }
}
