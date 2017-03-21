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
        // GET: TeeShirtOrdersReportController
        public ActionResult Index()
            {
            string constring = ConfigurationManager.ConnectionStrings["ReportConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT LeadContactFirstName, LeadContactLastName, LeadContactShirtOrder, LeadContactShirtSize FROM LeadContacts WHERE LeadContactShirtOrder = 1 UNION SELECT VolunteerFirstName, VolunteerLastName, VolunteerShirtOrder, VolunteerShirtSize FROM Volunteers WHERE VolunteerShirtOrder = 1;";
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.Fill(dt);
            con.Close();
            IList<TeeShirtOrdersModel> model = new List<TeeShirtOrdersModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
                {
                model.Add(new TeeShirtOrdersModel()
                    {
                    LeadContactFirstName = dt.Rows[i]["LeadContactFirstName"].ToString(),
                    LeadContactLastName = dt.Rows[i]["LeadContactLastName"].ToString(),
                    LeadContactShirtOrder = dt.Rows[i]["LeadContactShirtOrder"].ToString(),
                    LeadContactShirtSize = dt.Rows[i]["LeadContactShirtSize"].ToString()
                    });
                }
            return View(model);
            }

        public ActionResult TeeShirtOrdersReport()
            {
            string constring = ConfigurationManager.ConnectionStrings["ReportConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT LeadContactFirstName, LeadContactLastName, LeadContactShirtOrder, LeadContactShirtSize FROM LeadContacts WHERE LeadContactShirtOrder = 1 UNION SELECT VolunteerFirstName, VolunteerLastName, VolunteerShirtOrder, VolunteerShirtSize FROM Volunteers WHERE VolunteerShirtOrder = 1;";
            DataTable dt = new DataTable();
            dt.TableName = "LeadContacts";
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
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
