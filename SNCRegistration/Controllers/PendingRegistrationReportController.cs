using ClosedXML.Excel;
using SNCRegistration.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SNCRegistration.Controllers
{
    public class PendingRegistrationReportController : Controller
    {
        readonly string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
        private SNCRegistrationEntities db = new SNCRegistrationEntities();

        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
        // GET: PendingRegistrationsReport
        public ActionResult Index(int? eventYear)
            {

            // Dropdown List For Event Year
            ViewBag.ddlEventYears = Enumerable.Range(2016, (DateTime.Now.Year - 2016) + 1).OrderByDescending(x => x).ToList();

            List<PendingRegistrationReportModel> model = new List<PendingRegistrationReportModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = String.Concat("SELECT 'Participant' AS Registrant, ParticipantFirstName, ParticipantLastName FROM Participants "
                    + "UNION SELECT 'Guardian', GuardianFirstName, GuardianLastName FROM Guardians "
                    + "UNION SELECT 'FamilyMember', FamilyMemberFirstName, FamilyMemberLastName FROM FamilyMembers "
                    + "WHERE HealthForm = 0 OR PhotoAck = 0 AND EventYear = @EventYear ORDER BY ParticipantFirstName ASC");
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear != null ? eventYear.ToString() : DateTime.Now.Year.ToString());
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new PendingRegistrationReportModel()
                        {
                        Registrant = x["Registrant"].ToString(),
                        ParticipantFirstName = x["ParticipantFirstName"].ToString(),
                        ParticipantLastName = x["ParticipantLastName"].ToString(),
                        }).ToList();
                    }
                }
            return View(model);
            }

        //Get the year onchange javascript
        public ActionResult GetPendingRegistrationByYear(int eventYear)
            {
            List<PendingRegistrationReportModel> model = new List<PendingRegistrationReportModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = "SELECT 'Participant' AS Registrant, ParticipantFirstName, ParticipantLastName FROM Participants "
                    + "UNION SELECT 'Guardian', GuardianFirstName, GuardianLastName FROM Guardians "
                    + "UNION SELECT 'FamilyMember', FamilyMemberFirstName, FamilyMemberLastName FROM FamilyMembers "
                    + "WHERE HealthForm = 0 OR PhotoAck = 0 AND EventYear = @EventYear ORDER BY ParticipantFirstName ASC";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear);
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new PendingRegistrationReportModel()
                        {
                        Registrant = x["Registrant"].ToString(),
                        ParticipantFirstName = x["ParticipantFirstName"].ToString(),
                        ParticipantLastName = x["ParticipantLastName"].ToString(),
                        }).ToList();
                    }
                }
            return PartialView("_PartialPendingRegistrationList", model);
            }

        //Export to excel
        public ActionResult PendingRegistrationReport(int eventYear)
            {
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT 'Participant' AS Registrant, ParticipantFirstName, ParticipantLastName FROM Participants "
                    + "UNION SELECT 'Guardian', GuardianFirstName, GuardianLastName FROM Guardians "
                    + "UNION SELECT 'FamilyMember', FamilyMemberFirstName, FamilyMemberLastName FROM FamilyMembers "
                    + "WHERE HealthForm = 0 OR PhotoAck = 0 AND EventYear = @EventYear ORDER BY ParticipantFirstName ASC";
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            dt.TableName = "Participants";
            con.Open();
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
                Response.AddHeader("content-disposition", "attachment;filename= PendingRegistrationReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    }
                }
            return RedirectToAction("Index", "PendingRegistrationReport");
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
