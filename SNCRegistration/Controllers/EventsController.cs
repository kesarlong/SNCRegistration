using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SNCRegistration.ViewModels;
using System;

namespace SNCRegistration.Controllers
{
    public class EventsController : Controller
    {
        private SNCRegistrationEntities db = new SNCRegistrationEntities();

        // GET: Events
        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventYear,Enrollment,Reporting")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@event);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventYear,Enrollment,Reporting")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
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
        [HttpPost]
            public ActionResult ActiveRegistrationYear()
        {
            //to do: delete old attempts after working
            //var eventYear = from Event in db.Events where Event.Enrollment == true select Event.EventYear;


            //int myYear = Int32.Parse(eventYear);

            //TempData["myYear"] = eventYear;


            TempData["myYear"] = "2017";
            TempData.Keep();
            //return View();
            //return Content(TempData["myYear"].ToString());
            //return PartialView(TempData["myYear"]);

            //Event @event = db.Events.Where(Event=>Event.Enrollment = True);
            //if (@event == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(@event);

            bool x = true;
            return View(db.Events.ToList().Where(e => e.Enrollment == x));
            //return Content(db.Events.Where(e => e.Enrollment == x));

            //int y = 2017;
            //return View(db.Events.ToList().Where(e => e.EventYear==y));
        }

        private ActionResult Content(IQueryable<Event> queryable)
        {
            throw new NotImplementedException();
        }

        public ViewResult ActiveReportingYear()
        {
            var reportYear = from Event in db.Events where Event.Reporting == true select Event.EventYear;
            return View();
        }
    }
}
