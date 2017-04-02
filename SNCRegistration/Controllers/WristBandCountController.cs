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
    public class WristBandCountController : Controller
    {
        // GET: WristBandCount
        public ActionResult Index(int? eventYear)
            {

            // Dropdown List For Event Year
            ViewBag.ddlEventYears = Enumerable.Range(2016, (DateTime.Now.Year - 2016) + 1).OrderByDescending(x => x).ToList();

            List<WristBandCountModel> model = new List<WristBandCountModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();

            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = String.Concat("SELECT Volunteers.UnitChapterNumber, VolunteerFirstName, VolunteerLastName, LeadContactFirstName, LeadContactLastName, Volunteers.EventYear FROM Volunteers JOIN LeadContacts ON LeadContacts.LeadContactID = Volunteers.LeadContactID WHERE LeadContacts.EventYear = @EventYear UNION SELECT LeadContacts.UnitChapterNumber, LeadContactFirstName, LeadContactLastName, LeadContactFirstName, LeadContactLastName, LeadContacts.EventYear FROM LeadContacts WHERE LeadContacts.EventYear = @EventYear ORDER BY LeadContactFirstName ASC");
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear != null ? eventYear.ToString() : DateTime.Now.Year.ToString());
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new WristBandCountModel()
                        {
                        UnitChapterNumber = x["UnitChapterNumber"].ToString(),
                        VolunteerFirstName = x["VolunteerFirstName"].ToString(),
                        VolunteerLastName = x["VolunteerLastName"].ToString(),
                        LeadContactFirstName = x["LeadContactFirstName"].ToString(),
                        LeadContactLastName = x["LeadContactFirstName"].ToString(),
                        }).ToList();
                    }
                }
            return View(model);
            }

        //Get the year onchange javascript
        public ActionResult GetWristBandCountByYear(int eventYear)
            {
            List<WristBandCountModel> model = new List<WristBandCountModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query="SELECT Volunteers.UnitChapterNumber, VolunteerFirstName, VolunteerLastName, LeadContactFirstName, LeadContactLastName, Volunteers.EventYear FROM Volunteers JOIN LeadContacts ON LeadContacts.LeadContactID = Volunteers.LeadContactID WHERE LeadContacts.EventYear = @EventYear UNION SELECT LeadContacts.UnitChapterNumber, LeadContactFirstName, LeadContactLastName, LeadContactFirstName, LeadContactLastName, LeadContacts.EventYear FROM LeadContacts WHERE LeadContacts.EventYear = @EventYear ORDER BY LeadContactFirstName ASC";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear);
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new WristBandCountModel()
                        {
                        UnitChapterNumber = x["UnitChapterNumber"].ToString(),
                        VolunteerFirstName = x["VolunteerFirstName"].ToString(),
                        VolunteerLastName = x["VolunteerLastName"].ToString(),
                        LeadContactFirstName = x["LeadContactFirstName"].ToString(),
                        LeadContactLastName = x["LeadContactFirstName"].ToString(),
                        }).ToList();
                    }
                }
            return PartialView("_PartialWristBandCountList", model);
            }


        //Export to excel
        public ActionResult WristBandCount(int eventYear)
            {
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT Volunteers.UnitChapterNumber, VolunteerFirstName, VolunteerLastName, LeadContactFirstName, LeadContactLastName, Volunteers.EventYear FROM Volunteers JOIN LeadContacts ON LeadContacts.LeadContactID = Volunteers.LeadContactID WHERE LeadContacts.EventYear = @EventYear UNION SELECT LeadContacts.UnitChapterNumber, LeadContactFirstName, LeadContactLastName, LeadContactFirstName, LeadContactLastName, LeadContacts.EventYear FROM LeadContacts WHERE LeadContacts.EventYear = @EventYear ORDER BY LeadContactFirstName ASC";
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            dt.TableName = "Volunteers";
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
                Response.AddHeader("content-disposition", "attachment;filename= WristBandCountReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    }
                }
            return RedirectToAction("Index", "WristBandCount");
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
