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
    public class VolunteersFridayThruSaturdayController : Controller
    {
        // GET: VolunteersFridayThruSaturday
        public ActionResult Index(int? eventYear)
            {

            ViewBag.ddlEventYears = Enumerable.Range(2016, (DateTime.Now.Year - 2016) + 1).OrderByDescending(x => x).ToList();
            List<VolunteersFridayThruSaturdayModel> model = new List<VolunteersFridayThruSaturdayModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = String.Concat("SELECT Volunteers.UnitChapterNumber, VolunteerFirstName, VolunteerLastName, LeadContactFirstName, LeadContactLastName, Description, CASE WHEN Volunteers.CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM Volunteers INNER JOIN Attendance ON VolunteerAttendingCode = AttendanceID JOIN LeadContacts ON LeadContacts.LeadcontactID = Volunteers.LeadContactID WHERE AttendanceID = 4 AND Volunteers.EventYear = @EventYear");
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear != null ? eventYear.ToString() : DateTime.Now.Year.ToString());
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new VolunteersFridayThruSaturdayModel()
                        {
                        VolunteerFirstName = x["VolunteerFirstName"].ToString(),
                        VolunteerLastName = x["VolunteerLastName"].ToString(),
                        Description = x["Description"].ToString()
                        }).ToList();
                    }
                }
            return View(model);
            }

        //Get the year onchange javascript
        public ActionResult GetVolunteersFridayThruSaturdayByYear(int eventYear)
            {
            List<VolunteersFridayThruSaturdayModel> model = new List<VolunteersFridayThruSaturdayModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = "SELECT Volunteers.UnitChapterNumber, VolunteerFirstName, VolunteerLastName, LeadContactFirstName, LeadContactLastName, Description, CASE WHEN Volunteers.CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM Volunteers INNER JOIN Attendance ON VolunteerAttendingCode = AttendanceID JOIN LeadContacts ON LeadContacts.LeadcontactID = Volunteers.LeadContactID WHERE AttendanceID = 4 AND Volunteers.EventYear = @EventYear";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear);
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new VolunteersFridayThruSaturdayModel()
                        {
                        VolunteerFirstName = x["VolunteerFirstName"].ToString(),
                        VolunteerLastName = x["VolunteerLastName"].ToString(),
                        Description = x["Description"].ToString()
                        }).ToList();
                    }
                }
            return PartialView("_PartialVolunteersFridayThruSaturdayList", model);
            }

        //Export to excel
        public ActionResult VolunteersFridayThruSaturday(int eventYear)
            {
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT Volunteers.UnitChapterNumber, VolunteerFirstName, VolunteerLastName, LeadContactFirstName, LeadContactLastName, Description, CASE WHEN Volunteers.CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM Volunteers INNER JOIN Attendance ON VolunteerAttendingCode = AttendanceID JOIN LeadContacts ON LeadContacts.LeadcontactID = Volunteers.LeadContactID WHERE AttendanceID = 4 AND Volunteers.EventYear = @EventYear";
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
                Response.AddHeader("content-disposition", "attachment;filename= VolunteersFridayThruSaturdayReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    }
                }
            return RedirectToAction("Index", " VolunteersFridayThruSaturday");
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