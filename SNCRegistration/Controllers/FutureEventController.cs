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
    public class FutureEventController : Controller
    {
        // GET: FutureEvents
        public ActionResult Index(int? eventYear)
        {

            ViewBag.ddlEventYears = Enumerable.Range(2016, (DateTime.Now.Year - 2016) + 1).OrderByDescending(x => x).ToList();
            List<FutureEventsModel> model = new List<FutureEventsModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
            {
                dt = new DataTable();
                connection.Open();
                query = String.Concat("select LeadContactFirstName, LeadContactLastName, LeadContactEmail, B.BSTypeDescription, UnitChapterNumber from LeadContacts as L inner join BSType as B on L.BSType = B.BSTypeID where Marketing = 1 AND EventYear = @EventYear");
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear != null ? eventYear.ToString() : DateTime.Now.Year.ToString());
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new FutureEventsModel()
                    {
                        LeadContactFirstName = x["LeadContactFirstName"].ToString(),
                        LeadContactLastName = x["LeadContactLastName"].ToString(),
                        LeadContactEmail= x["LeadContactEmail"].ToString(),
                        BSDescription = x["BSTypeDescription"].ToString(),
                        UnitChapterNumber = x["UnitChapterNumber"].ToString()
                    }).ToList();
                }
            }
            return View(model);
        }

        //Get the year onchange javascript
        public ActionResult GetFutureEventsByYear(int eventYear)
        {
            List<FutureEventsModel> model = new List<FutureEventsModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
            {
                dt = new DataTable();
                connection.Open();
                query = "select LeadContactFirstName, LeadContactLastName, LeadContactEmail, B.BSTypeDescription, UnitChapterNumber from LeadContacts as L inner join BSType as B on L.BSType = B.BSTypeID where Marketing = 1 AND EventYear = @EventYear";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear);
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new FutureEventsModel()
                    {
                        LeadContactFirstName = x["LeadContactFirstName"].ToString(),
                        LeadContactLastName = x["LeadContactLastName"].ToString(),
                        LeadContactEmail = x["LeadContactEmail"].ToString(),
                        BSDescription = x["BSTypeDescription"].ToString(),
                        UnitChapterNumber = x["UnitChapterNumber"].ToString()
                    }).ToList();
                }
            }
            return PartialView("_PartialFutureEventList", model);
        }

        //Export to excel
        public ActionResult FutureEvents(int eventYear)
        {
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "select LeadContactFirstName, LeadContactLastName, LeadContactEmail, B.BSTypeDescription, UnitChapterNumber from LeadContacts as L inner join BSType as B on L.BSType = B.BSTypeID where Marketing = 1 AND EventYear = @EventYear";
            DataTable dt = new DataTable();
            dt.TableName = "LeadContacts";
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
                Response.AddHeader("content-disposition", "attachment;filename= FutureEventsReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return RedirectToAction("Index", " FutureEvent");
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
