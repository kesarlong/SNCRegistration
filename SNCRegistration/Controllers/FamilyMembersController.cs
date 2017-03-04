using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SNCRegistration.ViewModels;
using System.Data.Entity.Validation;

namespace SNCRegistration.Controllers
{
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: FamilyMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FamilyMemberID,FamilyMemberFirstName,FamilyMemberLastName,FamilyMemberAge,GuardianID,HealthForm,PhotoAck,AttendingCode,Comments,GuardianGuid")] FamilyMember familyMember)
        {
            if (ModelState.IsValid)
            {

                //to do: fix - static value needs to be dynamic
                familyMember.EventYear = 2017;

                db.FamilyMembers.Add(familyMember);

                try
                {
                    if (TempData["myPK"] != null)
                    {
                        familyMember.GuardianID = (int)TempData["myPK"];
                    }

                    db.SaveChanges();
                    
                    //add another participant for guardian                   
                    if (Request["submit"].Equals("Add another participant"))
                    { return RedirectToAction("Create", "Participants", new { GuardianGuid = familyMember.GuardianGuid }); }

                    //add a family member
                    if (Request["submit"].Equals("Add a family member"))
                    { return RedirectToAction("Create", "FamilyMembers", new { GuardianGuid = familyMember.GuardianGuid}); }

                    //registration complete, no more people to add
                    if (Request["submit"].Equals("Complete registration"))
                    { return Redirect("Registered");}

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

            return View(familyMember);
        }

        // GET: FamilyMembers/Edit/5
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FamilyMemberID,FamilyMemberFirstName,FamilyMemberLastName,GuardianID,HealthForm,PhotoAck,AttendingCode,Comments")] FamilyMember familyMember)
        {
            if (ModelState.IsValid)
            {
                db.Entry(familyMember).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(familyMember);
        }

        // GET: FamilyMembers/Delete/5
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

        public ActionResult Registered()
        {
            return View();
        }
    }
}
