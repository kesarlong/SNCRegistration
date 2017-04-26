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
    
    public class VolunteersSaturdayOnlyController : Controller
    {
        readonly string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
        private SNCRegistrationEntities db = new SNCRegistrationEntities();

        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
        // GET: VolunteersSaturdayOnly
        public ActionResult Index(int? eventYear)
            {

            ViewBag.ddlEventYears = Enumerable.Range(2016, (DateTime.Now.Year - 2016) + 1).OrderByDescending(x => x).ToList();
            List<VolunteersSaturdayOnlyModel> model = new List<VolunteersSaturdayOnlyModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = String.Concat("select LeadContactID as ID, 'LeadContact' as Type, UnitChapterNumber as GroupNumber, LeadContactFirstName as FirstName, LeadContactLastName as LastName, Attendance.Description as Attending from leadcontacts inner join Attendance on LeadContacts.VolunteerAttendingCode = Attendance.AttendanceID where volunteerattendingcode = 2 AND leadcontacts.EventYear = @EventYear union " +
                "select VolunteerID as ID, 'Volunteer', UnitChapterNumber as GroupNumber, VolunteerFirstName as FirstName, VolunteerLastName as LastName, Attendance.Description as Attending  from volunteers inner join Attendance on Volunteers.VolunteerAttendingCode = Attendance.AttendanceID where volunteerattendingcode = 2 AND volunteers.EventYear = @EventYear order by GroupNumber, LastName");
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear != null ? eventYear.ToString() : DateTime.Now.Year.ToString());
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new VolunteersSaturdayOnlyModel()
                        {
                        GroupNumber = x["GroupNumber"].ToString(),
                        FirstName = x["FirstName"].ToString(),
                        LastName = x["LastName"].ToString(),
                        Attending = x["Attending"].ToString()
                        }).ToList();
                    }
                }
            return View(model);
            }

        //Get the year onchange javascript
        public ActionResult GetVolunteersSaturdayOnlyByYear(int eventYear)
            {
            List<VolunteersSaturdayOnlyModel> model = new List<VolunteersSaturdayOnlyModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = "select LeadContactID as ID, 'LeadContact' as Type, UnitChapterNumber as GroupNumber, LeadContactFirstName as FirstName, LeadContactLastName as LastName, Attendance.Description as Attending from leadcontacts inner join Attendance on LeadContacts.VolunteerAttendingCode = Attendance.AttendanceID where volunteerattendingcode = 2 AND leadcontacts.EventYear = @EventYear union " +
                "select VolunteerID as ID, 'Volunteer', UnitChapterNumber as GroupNumber, VolunteerFirstName as FirstName, VolunteerLastName as LastName, Attendance.Description as Attending  from volunteers inner join Attendance on Volunteers.VolunteerAttendingCode = Attendance.AttendanceID where volunteerattendingcode = 2 AND volunteers.EventYear = @EventYear order by GroupNumber, LastName";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear);
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new VolunteersSaturdayOnlyModel()
                        {
                        GroupNumber = x["GroupNumber"].ToString(),
                        FirstName = x["FirstName"].ToString(),
                        LastName = x["LastName"].ToString(),
                        Attending = x["Attending"].ToString()
                        }).ToList();
                    }
                }
            return PartialView("_PartialVolunteersSaturdayOnlyList", model);
            }

        //Export to excel
        public ActionResult VolunteersSaturdayOnly(int eventYear)
            {
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "select LeadContactID as ID, 'LeadContact' as Type, UnitChapterNumber as GroupNumber, LeadContactFirstName as FirstName, LeadContactLastName as LastName, Attendance.Description as Attending from leadcontacts inner join Attendance on LeadContacts.VolunteerAttendingCode = Attendance.AttendanceID where volunteerattendingcode = 2 AND leadcontacts.EventYear = @EventYear union " +
                "select VolunteerID as ID, 'Volunteer', UnitChapterNumber as GroupNumber, VolunteerFirstName as FirstName, VolunteerLastName as LastName, Attendance.Description as Attending  from volunteers inner join Attendance on Volunteers.VolunteerAttendingCode = Attendance.AttendanceID where volunteerattendingcode = 2 AND volunteers.EventYear = @EventYear order by GroupNumber, LastName";
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
                Response.AddHeader("content-disposition", "attachment;filename= VolunteersSaturdayOnly.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    }
                }
            return RedirectToAction("Index", "VolunteersSaturdayOnly");
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