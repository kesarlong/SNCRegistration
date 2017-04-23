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
    public class VolunteersPendingCheckedInCountController : Controller
    {
        readonly string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
        private SNCRegistrationEntities db = new SNCRegistrationEntities();

        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
        // GET: VolunteersPendingCheckedInCount
        public ActionResult Index(int? eventYear)
            {
            ViewBag.ddlEventYears = Enumerable.Range(2016, (DateTime.Now.Year - 2016) + 1).OrderByDescending(x => x).ToList();
            List<VolunteersPendingCheckedInCountModel> model = new List<VolunteersPendingCheckedInCountModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = String.Concat("SELECT LeadContactID as 'ID', UnitChapterNumber, LeadContactFirstName as 'FirstName', LeadContactLastName as 'LastName', CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM LeadContacts WHERE CheckedIn = 0 AND EventYear = @EventYear "
                + "UNION SELECT VolunteerID as 'ID', UnitChapterNumber, VolunteerFirstName as 'FirstName', VolunteerLastName as 'LastName', CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM Volunteers WHERE CheckedIn = 0 AND EventYear = @EventYear ORDER BY FirstName ASC");
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear != null ? eventYear.ToString() : DateTime.Now.Year.ToString());
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new VolunteersPendingCheckedInCountModel()
                        {
                        UnitChapterNumber = x["UnitChapterNumber"].ToString(),
                        FirstName = x["FirstName"].ToString(),
                        LastName = x["LastName"].ToString(),
                        CheckedIn = x["CheckedIn"].ToString()
                        }).ToList();
                    }
                }
            return View(model);
            }

        //Get the year onchange javascript
        public ActionResult GetVolunteersPendingCheckedInCountByYear(int eventYear)
            {
            List<VolunteersPendingCheckedInCountModel> model = new List<VolunteersPendingCheckedInCountModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = "SELECT LeadContactID as 'ID', UnitChapterNumber, LeadContactFirstName as 'FirstName', LeadContactLastName as 'LastName', CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM LeadContacts WHERE CheckedIn = 0 AND EventYear = @EventYear "
                + "UNION SELECT VolunteerID as 'ID', UnitChapterNumber, VolunteerFirstName as 'FirstName', VolunteerLastName as 'LastName', CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM Volunteers WHERE CheckedIn = 0 AND EventYear = @EventYear ORDER BY FirstName ASC";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear);
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new VolunteersPendingCheckedInCountModel()
                        {
                        UnitChapterNumber = x["UnitChapterNumber"].ToString(),
                        FirstName = x["FirstName"].ToString(),
                        LastName = x["LastName"].ToString(),
                        CheckedIn = x["CheckedIn"].ToString()
                        }).ToList();
                    }
                }
            return PartialView("_PartialVolunteersPendingCheckedInList", model);
            }

        //Export to excel
        public ActionResult VolunteersPendingCheckedInCount(int eventYear)
            {
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT LeadContactID as 'ID', UnitChapterNumber, LeadContactFirstName as 'FirstName', LeadContactLastName as 'LastName', CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM LeadContacts WHERE CheckedIn = 0 AND EventYear = @EventYear "
            + "UNION SELECT VolunteerID as 'ID', UnitChapterNumber, VolunteerFirstName as 'FirstName', VolunteerLastName as 'LastName', CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM Volunteers WHERE CheckedIn = 0 AND EventYear = @EventYear ORDER BY FirstName ASC";
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
                Response.AddHeader("content-disposition", "attachment;filename= VolunteersPendingCheckedInCount.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    }
                }
            return RedirectToAction("Index", "VolunteersPendingCheckedInCount");
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