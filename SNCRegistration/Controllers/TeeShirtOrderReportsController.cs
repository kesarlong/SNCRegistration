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
    public class TeeShirtOrdersReportController : Controller

    {
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin")]
        // GET: TeeShirtOrdersReportController
        public ActionResult Index(int? eventYear)
            {

            // Dropdown List For Event Year
            ViewBag.ddlEventYears = Enumerable.Range(2016, (DateTime.Now.Year - 2016) + 1).OrderByDescending(x => x).ToList();

            List<TeeShirtOrdersModel> model = new List<TeeShirtOrdersModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();

            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = String.Concat("SELECT VolunteerID, Volunteers.UnitChapterNumber as GroupNumber, VolunteerFirstName as FirstName, VolunteerLastName as LastName, VolunteerShirtSize as ShirtSize FROM Volunteers where volunteershirtsize != '00' AND volunteershirtorder = 1 AND EventYear = @EventYear union SELECT LeadContactID, LeadContacts.UnitChapterNumber as GroupNumber, LeadContactFirstName as FirstName, LeadContactLastName as LastName, LeadContactShirtSize as ShirtSize FROM LeadContacts where LeadContactshirtsize != '00' AND leadcontactshirtorder = 1 AND EventYear = @EventYear Order By GroupNumber");
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear != null ? eventYear.ToString() : DateTime.Now.Year.ToString());
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new TeeShirtOrdersModel()
                        {
                        GroupNumber = x["GroupNumber"].ToString(),
                        FirstName = x["FirstName"].ToString(),
                        LastName = x["LastName"].ToString(),
                        ShirtSize = x["ShirtSize"].ToString()
                        }).ToList();
                    }
                }
            return View(model);
            }

        //Get the year onchange javascript
        public ActionResult GetTeeShirtsOrdersByYear(int eventYear)
            {
            List<TeeShirtOrdersModel> model = new List<TeeShirtOrdersModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = "SELECT VolunteerID, Volunteers.UnitChapterNumber as GroupNumber, VolunteerFirstName as FirstName, VolunteerLastName as LastName, VolunteerShirtSize as ShirtSize FROM Volunteers where volunteershirtsize != '00' AND volunteershirtorder = 1 AND EventYear = @EventYear union SELECT LeadContactID, LeadContacts.UnitChapterNumber as GroupNumber, LeadContactFirstName as FirstName, LeadContactLastName as LastName, LeadContactShirtSize as ShirtSize FROM LeadContacts where LeadContactshirtsize != '00' AND leadcontactshirtorder = 1 AND EventYear = @EventYear Order By GroupNumber";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear);
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new TeeShirtOrdersModel()
                        {
                        GroupNumber = x["GroupNumber"].ToString(),
                        FirstName = x["FirstName"].ToString(),
                        LastName = x["LastName"].ToString(),
                        ShirtSize = x["ShirtSize"].ToString()
                        }).ToList();
                    }
                }
            return PartialView("_PartialTeeShirtOrdersList", model);
            }


        //Export to excel
        public ActionResult TeeShirtOrdersReport(int eventYear)
            {
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT VolunteerID, Volunteers.UnitChapterNumber as GroupNumber, VolunteerFirstName as FirstName, VolunteerLastName as LastName, VolunteerShirtSize as ShirtSize FROM Volunteers where volunteershirtsize != '00' AND volunteershirtorder = 1 AND EventYear = @EventYear union SELECT LeadContactID, LeadContacts.UnitChapterNumber as GroupNumber, LeadContactFirstName as FirstName, LeadContactLastName as LastName, LeadContactShirtSize as ShirtSize FROM LeadContacts where LeadContactshirtsize != '00' AND leadcontactshirtorder = 1 AND EventYear = @EventYear Order By GroupNumber";
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
                Response.AddHeader("content-disposition", "attachment;filename= TeeShirtOrdersReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    }
                }
            return RedirectToAction("Index", "TeeShirtOrdersReport");
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
