using ClosedXML.Excel;
using SNCRegistration.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace SNCRegistration.Controllers
{
    public class VolunteersReportController : Controller
    {
        readonly string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
        private SNCRegistrationEntities db = new SNCRegistrationEntities();

        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
        // GET: Reporting
        public ActionResult Index(int? eventYear)
            {

            // Get Volunteer Count to display in view

            ViewBag.ddlEventYears = Enumerable.Range(2016, (DateTime.Now.Year - 2016) + 1).OrderByDescending(x => x).ToList();
            List<VolunteersReportModel> model = new List<VolunteersReportModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = String.Concat("SELECT VolunteerID, Volunteers.UnitChapterNumber, VolunteerFirstName AS 'FirstName', VolunteerLastName AS 'LastName', LeadContactFirstName, LeadContactLastName, Description, VolunteerShirtSize, CASE WHEN Volunteers.CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn, '' AS 'Campsite', '' AS 'Balance Due' FROM Volunteers INNER JOIN Attendance ON Volunteers.VolunteerAttendingCode = AttendanceID JOIN LeadContacts ON LeadContacts.LeadContactID = Volunteers.LeadContactID WHERE Volunteers.EventYear = @EventYear ORDER BY Volunteers.UnitChapterNumber, VolunteerFirstName ASC");
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear != null ? eventYear.ToString() : DateTime.Now.Year.ToString());
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new VolunteersReportModel()
                        {
                        UnitChapterNumber = x["UnitChapterNumber"].ToString(),
                        VolunteerFirstName = x["FirstName"].ToString(),
                        VolunteerLastName = x["LastName"].ToString(),
                        LeadContactFirstName = x["LeadContactFirstName"].ToString(),
                        LeadContactLastName = x["LeadContactLastName"].ToString()
                       
                        }).ToList();
                    }
                }
            return View(model);
            }

        //Get the year onchange javascript
        public ActionResult GetVolunteersReportByYear(int eventYear)
            {
            List<VolunteersReportModel> model = new List<VolunteersReportModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = "SELECT VolunteerID, Volunteers.UnitChapterNumber, VolunteerFirstName AS 'FirstName', VolunteerLastName AS 'LastName', LeadContactFirstName, LeadContactLastName, Description, VolunteerShirtSize, CASE WHEN Volunteers.CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn, '' AS 'Campsite', '' AS 'Balance Due' FROM Volunteers INNER JOIN Attendance ON Volunteers.VolunteerAttendingCode = AttendanceID JOIN LeadContacts ON LeadContacts.LeadContactID = Volunteers.LeadContactID WHERE Volunteers.EventYear = @EventYear ORDER BY Volunteers.UnitChapterNumber, VolunteerFirstName ASC";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear);
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new VolunteersReportModel()
                        {
                        UnitChapterNumber = x["UnitChapterNumber"].ToString(),
                        VolunteerFirstName = x["FirstName"].ToString(),
                        VolunteerLastName = x["LastName"].ToString(),
                        LeadContactFirstName = x["LeadContactFirstName"].ToString(),
                        LeadContactLastName = x["LeadContactLastName"].ToString()
                       
                        }).ToList();
                    }
                }
            return PartialView("_PartialVolunteersReportList", model);
            }

        //Export to excel
        public ActionResult VolunteersReport(int eventYear)
            {
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT VolunteerID, Volunteers.UnitChapterNumber, VolunteerFirstName AS 'FirstName', VolunteerLastName AS 'LastName', LeadContactFirstName, LeadContactLastName, Description, VolunteerShirtSize, CASE WHEN Volunteers.CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn, '' AS 'Campsite', '' AS 'Balance Due' FROM Volunteers INNER JOIN Attendance ON Volunteers.VolunteerAttendingCode = AttendanceID JOIN LeadContacts ON LeadContacts.LeadContactID = Volunteers.LeadContactID WHERE Volunteers.EventYear = @EventYear ORDER BY Volunteers.UnitChapterNumber, VolunteerFirstName ASC";
            DataTable dt = new DataTable();
            dt.TableName = "Volunteers";
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
                Response.AddHeader("content-disposition", "attachment;filename= VolunteersReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    }
                }
            return RedirectToAction("Index", "VolunteersReport");
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
