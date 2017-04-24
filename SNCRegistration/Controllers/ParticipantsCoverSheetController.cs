using ClosedXML.Excel;
using SNCRegistration.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;
using System.Linq;

namespace SNCRegistration.Controllers
    {
    public class ParticipantsCoverSheetController : Controller
        {
        readonly string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
        private SNCRegistrationEntities db = new SNCRegistrationEntities();

        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin")]
        // GET: Reporting
        public ActionResult Index(int? eventYear)   
            {

            ViewBag.ddlEventYears = Enumerable.Range(2016, (DateTime.Now.Year - 2016) + 1).OrderByDescending(x => x).ToList();
            List<ParticipantsCoverSheetModel> model = new List<ParticipantsCoverSheetModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = String.Concat("select g.guardianID as ID, 'Guardian' as Type, g.guardianfirstname as FirstName, g.guardianlastname as LastName, g.relationship as Relationship, '' as Age, Case When g.checkedin = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn, g.GuardianCellPhone as CellPhone, CASE When g.HealthForm = 1 THEN 'Yes' ELSE 'No' END AS HealthForm, CASE When g.PhotoAck = 1 THEN 'Yes' ELSE 'No' END as PhotoAck, at.description as Attending, g.Comments, g.Eventyear from guardians as g inner join attendance as at on at.attendanceid = g.attendingcode where EventYear = @EventYear "
                + "union select p.ParticipantID as ID, 'Participant', p.ParticipantFirstName as FirstName, p.ParticipantLastName as LastName, '' as Relationship, ag.agedescription as Age, case when p.checkedin = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn, '' as CellPhone, case when p.HealthForm = 1 THEN 'Yes' ELSE 'No' END AS HealthForm, case when p.PhotoAck = 1 THEN 'Yes' ELSE 'No' END as PhotoAck, at.description as Attending, p.Comments, p.Eventyear from participants as p inner join age as ag on p.ParticipantAge = ag.ageid inner join attendance as at on p.attendingcode = at.attendanceid where EventYear = @EventYear "
                + "union select f.FamilyMemberID as ID, 'FamilyMember', f.FamilyMemberFirstName as FirstName, f.FamilyMemberLastName as LastName, '' as Relationship, ag.agedescription as Age, case when f.checkedin = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn, '' as CellPhone, case when f.HealthForm = 1 THEN 'Yes' ELSE 'No' END AS HealthForm, case when f.PhotoAck = 1 THEN 'Yes' ELSE 'No' END as PhotoAck, at.description as Attending, f.Comments, f.Eventyear from familymembers as f inner join age as ag on f.FamilyMemberAge = ag.ageid inner join attendance as at on f.attendingcode = at.attendanceid where EventYear = @EventYear");
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear != null ? eventYear.ToString() : DateTime.Now.Year.ToString());
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new ParticipantsCoverSheetModel()
                        {
                        FirstName = x["FirstName"].ToString(),
                        LastName = x["LastName"].ToString(),
                        }).ToList();
                    }
                }
            return View(model);
            }

        //Get the year onchange javascript
        public ActionResult GetParticipantsCoverSheetByYear(int eventYear)
            {
            List<ParticipantsCoverSheetModel> model = new List<ParticipantsCoverSheetModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = "select g.guardianID as ID, 'Guardian' as Type, g.guardianfirstname as FirstName, g.guardianlastname as LastName, g.relationship as Relationship, '' as Age, Case When g.checkedin = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn, g.GuardianCellPhone as CellPhone, CASE When g.HealthForm = 1 THEN 'Yes' ELSE 'No' END AS HealthForm, CASE When g.PhotoAck = 1 THEN 'Yes' ELSE 'No' END as PhotoAck, at.description as Attending, g.Comments, g.Eventyear from guardians as g inner join attendance as at on at.attendanceid = g.attendingcode where EventYear = @EventYear "
                + "union select p.ParticipantID as ID, 'Participant', p.ParticipantFirstName as FirstName, p.ParticipantLastName as LastName, '' as Relationship, ag.agedescription as Age, case when p.checkedin = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn, '' as CellPhone, case when p.HealthForm = 1 THEN 'Yes' ELSE 'No' END AS HealthForm, case when p.PhotoAck = 1 THEN 'Yes' ELSE 'No' END as PhotoAck, at.description as Attending, p.Comments, p.Eventyear from participants as p inner join age as ag on p.ParticipantAge = ag.ageid inner join attendance as at on p.attendingcode = at.attendanceid where EventYear = @EventYear "
                + "union select f.FamilyMemberID as ID, 'FamilyMember', f.FamilyMemberFirstName as FirstName, f.FamilyMemberLastName as LastName, '' as Relationship, ag.agedescription as Age, case when f.checkedin = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn, '' as CellPhone, case when f.HealthForm = 1 THEN 'Yes' ELSE 'No' END AS HealthForm, case when f.PhotoAck = 1 THEN 'Yes' ELSE 'No' END as PhotoAck, at.description as Attending, f.Comments, f.Eventyear from familymembers as f inner join age as ag on f.FamilyMemberAge = ag.ageid inner join attendance as at on f.attendingcode = at.attendanceid where EventYear = @EventYear";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear);
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new ParticipantsCoverSheetModel()
                        {
                        
                        FirstName = x["FirstName"].ToString(),
                        LastName = x["LastName"].ToString(),
                        }).ToList();
                    }
                }
            return PartialView("_PartialParticipantsCoverSheetList", model);
            }

        //Export to excel
        public ActionResult ParticipantsCoverSheet(int eventYear)
            {
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "select g.guardianID as ID, 'Guardian' as Type, g.guardianfirstname as FirstName, g.guardianlastname as LastName, g.relationship as Relationship, '' as Age, Case When g.checkedin = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn, g.GuardianCellPhone as CellPhone, CASE When g.HealthForm = 1 THEN 'Yes' ELSE 'No' END AS HealthForm, CASE When g.PhotoAck = 1 THEN 'Yes' ELSE 'No' END as PhotoAck, at.description as Attending, g.Comments, g.Eventyear from guardians as g inner join attendance as at on at.attendanceid = g.attendingcode where EventYear = @EventYear "
                + "union select p.ParticipantID as ID, 'Participant', p.ParticipantFirstName as FirstName, p.ParticipantLastName as LastName, '' as Relationship, ag.agedescription as Age, case when p.checkedin = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn, '' as CellPhone, case when p.HealthForm = 1 THEN 'Yes' ELSE 'No' END AS HealthForm, case when p.PhotoAck = 1 THEN 'Yes' ELSE 'No' END as PhotoAck, at.description as Attending, p.Comments, p.Eventyear from participants as p inner join age as ag on p.ParticipantAge = ag.ageid inner join attendance as at on p.attendingcode = at.attendanceid where EventYear = @EventYear "
                + "union select f.FamilyMemberID as ID, 'FamilyMember', f.FamilyMemberFirstName as FirstName, f.FamilyMemberLastName as LastName, '' as Relationship, ag.agedescription as Age, case when f.checkedin = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn, '' as CellPhone, case when f.HealthForm = 1 THEN 'Yes' ELSE 'No' END AS HealthForm, case when f.PhotoAck = 1 THEN 'Yes' ELSE 'No' END as PhotoAck, at.description as Attending, f.Comments, f.Eventyear from familymembers as f inner join age as ag on f.FamilyMemberAge = ag.ageid inner join attendance as at on f.attendingcode = at.attendanceid where EventYear = @EventYear";
            DataTable dt = new DataTable();
            dt.TableName = "Participants";
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear);
            da.Fill(dt);
            con.Close();
            using (XLWorkbook wb = new XLWorkbook())
                {
                wb.Worksheets.Add(dt);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= ParticipantsCoverSheet.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    }
                }
            return RedirectToAction("Index", "ParticipantsCoverSheet");
            }

        private void releaseObject(object obj)
            {
            try
                {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
                }
            catch
                {
                obj = null;
                }
            finally
                {
                GC.Collect();
                }
            }
        }
    }
