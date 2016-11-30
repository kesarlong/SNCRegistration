using CrystalDecisions.CrystalReports.Engine;
using SNCRegistration.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SNCRegistration.Controllers
{
    public class ReportingController : Controller
    {
        private SNCRegistrationEntities db = new SNCRegistrationEntities();
        // GET: Reporting
        public ActionResult Index()
        {
            var ParticipantsList = db.Participants.ToList();
            return View(ParticipantsList);
            }
        public ActionResult ExportParticipants()
            {
            List<Participant> allParticipants = new List<Participant>();
            allParticipants = db.Participants.ToList();


            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reporting"), "ParticipantsList.rpt"));

            rd.SetDataSource(allParticipants);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "ParticipantsList.pdf");
            }
        }
}