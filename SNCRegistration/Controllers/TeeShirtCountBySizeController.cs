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
    public class TeeShirtCountBySizeController : Controller

    {
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin")]
        // GET: TeeShirtCountBySizeController
        public ActionResult Index(int? eventYear)
            {

            // Dropdown List For Event Year
            ViewBag.ddlEventYears = Enumerable.Range(2016, (DateTime.Now.Year - 2016) + 1).OrderByDescending(x => x).ToList();

            List<TeeShirtCountBySizeModel> model = new List<TeeShirtCountBySizeModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();

            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = String.Concat("SELECT VolunteerShirtSize as ShirtSize, COUNT(*) As Total FROM Volunteers where volunteershirtsize != '00' AND EventYear = @EventYear GROUP BY VolunteerShirtSize union " +
                "SELECT LeadContactShirtSize as ShirtSize, COUNT(*) As Total FROM LeadContacts where LeadContactshirtsize != '00' AND EventYear = @EventYear GROUP BY LeadContactShirtSize");
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear != null ? eventYear.ToString() : DateTime.Now.Year.ToString());
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new TeeShirtCountBySizeModel()
                        {
                        ShirtSize = x["ShirtSize"].ToString(),
                        Total = Convert.ToInt32(x["Total"].ToString())
                        }).ToList();
                    }
                }
            return View(model);
            }

        //Get the year onchange javascript
        public ActionResult GetTeeShirtCountBySizeByYear(int eventYear)
            {
            List<TeeShirtCountBySizeModel> model = new List<TeeShirtCountBySizeModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = "SELECT VolunteerShirtSize as ShirtSize, COUNT(*) As Total FROM Volunteers where volunteershirtsize != '00' AND EventYear = @EventYear GROUP BY VolunteerShirtSize union " +
                "SELECT LeadContactShirtSize as ShirtSize, COUNT(*) As Total FROM LeadContacts where LeadContactshirtsize != '00' AND EventYear = @EventYear GROUP BY LeadContactShirtSize";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear);
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new TeeShirtCountBySizeModel()
                        {
                        ShirtSize = x["ShirtSize"].ToString(),
                        Total = Convert.ToInt32(x["Total"].ToString())
                        }).ToList();
                    }
                }
            return PartialView("_PartialTeeShirtCountBySizeList", model);
            }


        //Export to excel
        public ActionResult TeeShirtCountBySize(int eventYear)
            {
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT VolunteerShirtSize as ShirtSize, COUNT(*) As Total FROM Volunteers where volunteershirtsize != '00' AND EventYear = @EventYear GROUP BY VolunteerShirtSize union " +
                "SELECT LeadContactShirtSize as ShirtSize, COUNT(*) As Total FROM LeadContacts where LeadContactshirtsize != '00' AND EventYear = @EventYear GROUP BY LeadContactShirtSize";
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
            return RedirectToAction("Index", "TeeShirtCountBySize");
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
