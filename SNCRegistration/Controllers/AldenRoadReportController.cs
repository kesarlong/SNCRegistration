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
    public class AldenRoadReportController : Controller
        {
        readonly string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
        private SNCRegistrationEntities db = new SNCRegistrationEntities();
        // GET: Dashboard
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin")]

        public ActionResult Index(int? eventYear)
            {
            ViewBag.ddlEventYears = Enumerable.Range(2016, (DateTime.Now.Year - 2016) + 1).OrderByDescending(x => x).ToList();
            List<AldenRoadReportModel> model = new List<AldenRoadReportModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = String.Concat("SELECT ParticipantID, ParticipantFirstName, ParticipantLastName, ParticipantAge, ParticipantSchool, "
                    + "ParticipantTeacher, CASE WHEN ClassroomScouting = 1 THEN 'Yes' ELSE 'No' END AS ClassroomScouting, "
                    + "CASE WHEN Participants.Returning = 1 THEN 'Yes' ELSE 'No' END AS Returning, "
                    + "CASE WHEN Participants.HealthForm = 1 THEN 'Yes' ELSE 'No' END AS HeatlhForm, "
                    + "Attendance.Description AS 'Attending', Participants.GuardianID, GuardianFirstName, GuardianLastName, "
                    + "CASE WHEN Participants.CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS Checkedin, '' AS 'Campsite' FROM Participants INNER JOIN Guardians ON Guardians.GuardianID = Participants.GuardianID "
                    + "INNER JOIN Attendance ON AttendanceID = Participants.AttendingCode INNER JOIN Age ON ParticipantAge = AgeID WHERE ParticipantSchool LIKE '%Alden%' AND Participants.EventYear = @EventYear ORDER BY ParticipantFirstName;");
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear != null ? eventYear.ToString() : DateTime.Now.Year.ToString());
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new AldenRoadReportModel()
                        {
                        ParticipantID = Convert.ToInt32(x["ParticipantID"]),
                        ParticipantFirstName = x["ParticipantFirstName"].ToString(),
                        ParticipantLastName = x["ParticipantLastName"].ToString(),
                        GuardianFirstName = x["GuardianFirstName"].ToString(),
                        GuardianLastName = x["GuardianLastName"].ToString(),
                        ParticipantSchool = x["ParticipantSchool"].ToString(),
                        Description = x["Attending"].ToString()
                        }).ToList();
                    }
                }
            return View(model);
            }

        //Get the year onchange javascript
        public ActionResult GetAldenRoadReportByYear(int eventYear)
            {
            List<AldenRoadReportModel> model = new List<AldenRoadReportModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = "SELECT ParticipantID, ParticipantFirstName, ParticipantLastName, ParticipantAge, ParticipantSchool, "
                    + "ParticipantTeacher, CASE WHEN ClassroomScouting = 1 THEN 'Yes' ELSE 'No' END AS ClassroomScouting, "
                    + "CASE WHEN Participants.Returning = 1 THEN 'Yes' ELSE 'No' END AS Returning, "
                    + "CASE WHEN Participants.HealthForm = 1 THEN 'Yes' ELSE 'No' END AS HeatlhForm, "
                    + "Attendance.Description AS 'Attending', Participants.GuardianID, GuardianFirstName, GuardianLastName, "
                    + "CASE WHEN Participants.CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS Checkedin, '' AS 'Campsite' FROM Participants INNER JOIN Guardians ON Guardians.GuardianID = Participants.GuardianID "
                    + "INNER JOIN Attendance ON AttendanceID = Participants.AttendingCode INNER JOIN Age ON ParticipantAge = AgeID WHERE ParticipantSchool LIKE '%Alden%' AND Participants.EventYear = @EventYear ORDER BY ParticipantFirstName;";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear);
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new AldenRoadReportModel()
                        {
                        ParticipantID = Convert.ToInt32(x["ParticipantID"]),
                        ParticipantFirstName = x["ParticipantFirstName"].ToString(),
                        ParticipantLastName = x["ParticipantLastName"].ToString(),
                        GuardianFirstName = x["GuardianFirstName"].ToString(),
                        GuardianLastName = x["GuardianLastName"].ToString(),
                        ParticipantSchool = x["ParticipantSchool"].ToString(),
                        Description= x["Attending"].ToString()
                        }).ToList();
                    }
                }
            return PartialView("_PartialAldenRoadReportList", model);
            }

        //Export to excel
        public ActionResult AldenRoadReport(int eventYear)
            {
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT ParticipantID, ParticipantFirstName, ParticipantLastName, ParticipantAge, ParticipantSchool, "
                    + "ParticipantTeacher, CASE WHEN ClassroomScouting = 1 THEN 'Yes' ELSE 'No' END AS ClassroomScouting, "
                    + "CASE WHEN Participants.Returning = 1 THEN 'Yes' ELSE 'No' END AS Returning, "
                    + "CASE WHEN Participants.HealthForm = 1 THEN 'Yes' ELSE 'No' END AS HeatlhForm, "
                    + "Attendance.Description AS 'Attending', Participants.GuardianID, GuardianFirstName, GuardianLastName, "
                    + "CASE WHEN Participants.CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS Checkedin, '' AS 'Campsite' FROM Participants INNER JOIN Guardians ON Guardians.GuardianID = Participants.GuardianID "
                    + "INNER JOIN Attendance ON AttendanceID = Participants.AttendingCode INNER JOIN Age ON ParticipantAge = AgeID WHERE ParticipantSchool LIKE '%Alden%' AND Participants.EventYear = @EventYear ORDER BY ParticipantFirstName;";
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
                Response.AddHeader("content-disposition", "attachment;filename= AldenRoadReportReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    }
                }
            return RedirectToAction("Index", "AldenRoadReport");
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