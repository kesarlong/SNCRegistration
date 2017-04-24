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
    public class VolunteersCoverSheetController : Controller
        {
        readonly string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
        private SNCRegistrationEntities db = new SNCRegistrationEntities();

        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin")]
        // GET: Reporting
        public ActionResult Index(int? eventYear)   
            {

            ViewBag.ddlEventYears = Enumerable.Range(2016, (DateTime.Now.Year - 2016) + 1).OrderByDescending(x => x).ToList();
            List<VolunteersCoverSheetModel> model = new List<VolunteersCoverSheetModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = String.Concat("select l.LeadContactID as ID, l.EventYear as Year, l.LeadContactFirstName as FirstName, l.LeadContactLastName as LastName, CASE WHEN l.CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn, l.LeadContactCellPhone as CellPhone, at.description as Attending, l.Booth as Booth, CASE WHEN l.LeadContactShirtOrder = 1 THEN 'Yes' ELSE 'No' END AS ShirtOrder, l.LeadContactShirtSize as ShirtSize from leadcontacts as l inner join Attendance as at on l.VolunteerAttendingCode = at.attendanceid where EventYear = @EventYear "
                + "union select v.VolunteerID as ID, v.EventYear as Year, v.VolunteerFirstName as FirstName, v.VolunteerLastName as LastName, CASE WHEN v.CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn, '' as CellPhone, at.description as Attending, '' as Booth, CASE WHEN v.VolunteerShirtOrder = 1 THEN 'Yes' ELSE 'No' END AS ShirtOrder, v.VolunteerShirtSize as ShirtSize from volunteers as v inner join Attendance as at on v.VolunteerAttendingCode = at.attendanceid where EventYear = @EventYear");
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear != null ? eventYear.ToString() : DateTime.Now.Year.ToString());
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new VolunteersCoverSheetModel()
                        {
                        FirstName = x["FirstName"].ToString(),
                        LastName = x["LastName"].ToString(),
                        }).ToList();
                    }
                }
            return View(model);
            }

        //Get the year onchange javascript
        public ActionResult GetVolunteersCoverSheetByYear(int eventYear)
            {
            List<VolunteersCoverSheetModel> model = new List<VolunteersCoverSheetModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = "select l.LeadContactID as ID, l.EventYear as Year, l.LeadContactFirstName as FirstName, l.LeadContactLastName as LastName, CASE WHEN l.CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn, l.LeadContactCellPhone as CellPhone, at.description as Attending, l.Booth as Booth, CASE WHEN l.LeadContactShirtOrder = 1 THEN 'Yes' ELSE 'No' END AS ShirtOrder, l.LeadContactShirtSize as ShirtSize from leadcontacts as l inner join Attendance as at on l.VolunteerAttendingCode = at.attendanceid where EventYear = @EventYear "
                + "union select v.VolunteerID as ID, v.EventYear as Year, v.VolunteerFirstName as FirstName, v.VolunteerLastName as LastName, CASE WHEN v.CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn, '' as CellPhone, at.description as Attending, '' as Booth, CASE WHEN v.VolunteerShirtOrder = 1 THEN 'Yes' ELSE 'No' END AS ShirtOrder, v.VolunteerShirtSize as ShirtSize from volunteers as v inner join Attendance as at on v.VolunteerAttendingCode = at.attendanceid where EventYear = @EventYear";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear);
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new VolunteersCoverSheetModel()
                        {
                        
                        FirstName = x["FirstName"].ToString(),
                        LastName = x["LastName"].ToString(),
                        }).ToList();
                    }
                }
            return PartialView("_PartialVolunteersCoverSheetList", model);
            }

        //Export to excel
        public ActionResult VolunteersCoverSheet(int eventYear)
            {
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "select l.LeadContactID as ID, l.EventYear as Year, l.LeadContactFirstName as FirstName, l.LeadContactLastName as LastName, CASE WHEN l.CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn, l.LeadContactCellPhone as CellPhone, at.description as Attending, l.Booth as Booth, CASE WHEN l.LeadContactShirtOrder = 1 THEN 'Yes' ELSE 'No' END AS ShirtOrder, l.LeadContactShirtSize as ShirtSize from leadcontacts as l inner join Attendance as at on l.VolunteerAttendingCode = at.attendanceid where EventYear = @EventYear "
                + "union select v.VolunteerID as ID, v.EventYear as Year, v.VolunteerFirstName as FirstName, v.VolunteerLastName as LastName, CASE WHEN v.CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn, '' as CellPhone, at.description as Attending, '' as Booth, CASE WHEN v.VolunteerShirtOrder = 1 THEN 'Yes' ELSE 'No' END AS ShirtOrder, v.VolunteerShirtSize as ShirtSize from volunteers as v inner join Attendance as at on v.VolunteerAttendingCode = at.attendanceid where EventYear = @EventYear";
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
                Response.AddHeader("content-disposition", "attachment;filename= VolunteersCoverSheet.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    }
                }
            return RedirectToAction("Index", "VolunteersCoverSheet");
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
