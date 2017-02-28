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

namespace SNCRegistration.Controllers
{
    public class GuardiansController : Controller
    {
        private SNCRegistrationEntities db = new SNCRegistrationEntities();


        // GET: Guardians
        public ActionResult Index()
        {
            try
            {
                return View(db.Guardians.ToList());
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
            return View(db.Guardians.ToList());
        }

        // GET: Guardians/Details/5
        public ActionResult Details(int? id)
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

        // GET: Guardians/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: Guardians/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GuardianID,GuardianFirstName,GuardianLastName,GuardianAddress,GuardianCity,GuardianState,GuardianZip,GuardianCellPhone,GuardianEmail,PacketSentDate,ReceiptDate,ConfirmationSentDate,HealthForm,PhotoAck,Tent,AttendingCode,Comments,Relationship,GuardianGuid")] Guardian guardian)
        {
            if (ModelState.IsValid)
                
            {
                db.Guardians.Add(guardian);

                var myGuid = Guid.NewGuid().ToString();
                guardian.GuardianGuid= myGuid;

                try
                {
                    db.SaveChanges();
                    this.Session["gSession"] = guardian.GuardianGuid;
                    this.Session["myID"] = guardian.GuardianID;

                    //RouteData.Values.Remove("id");

                    return RedirectToAction("Create", "Participants", new { GuardianGuid = this.Session["gSession"] });
                    //return View("Create", "Participants");


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


        // POST: Guardians/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GuardianID,GuardianFirstName,GuardianLastName,GuardianAddress,GuardianCity,GuardianZip,GuardianCellPhone,GuardianEmail,PacketSentDate,ReceiptDate,ConfirmationSentDate,HealthForm,PhotoAck,Tent,AttendingCode,Comments,Relationship")] Guardian guardian)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guardian).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
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
