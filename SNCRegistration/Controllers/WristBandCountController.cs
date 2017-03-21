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
        public ActionResult Index()
            {
            string constring = ConfigurationManager.ConnectionStrings["ReportConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT LeadContactFirstName, LeadContactLastName FROM LeadContacts UNION SELECT VolunteerFirstName, VolunteerLastName FROM Volunteers;";
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.Fill(dt);
            con.Close();
            IList<WristBandCountModel> model = new List<WristBandCountModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
                {
                model.Add(new WristBandCountModel()
                    {
                    LeadContactFirstName = dt.Rows[i]["LeadContactFirstName"].ToString(),
                    LeadContactLastName = dt.Rows[i]["LeadContactLastName"].ToString(),
                    
                    });
                }
            return View(model);
            }

        public ActionResult WristBandCount()
            {
            string constring = ConfigurationManager.ConnectionStrings["ReportConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT LeadContactFirstName, LeadContactLastName FROM LeadContacts UNION SELECT VolunteerFirstName, VolunteerLastName FROM Volunteers;";
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
                Response.AddHeader("content-disposition", "attachment;filename= WristBandCountReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    }
                }

            return RedirectToAction("Index", "WristBandCountReport");
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
