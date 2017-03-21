using ClosedXML.Excel;
using SNCRegistration.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;

namespace SNCRegistration.Controllers
{
    public class BoothCountController : Controller
    {
        // GET: BoothCount
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
        public ActionResult Index()
            {
            string constring = ConfigurationManager.ConnectionStrings["ReportConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT Booth FROM LeadContacts WHERE Booth IS NOT NULL;";
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.Fill(dt);
            con.Close();
            IList<BoothCountModel> model = new List<BoothCountModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
                {
                model.Add(new BoothCountModel()
                    {
                    Booth = dt.Rows[i]["Booth"].ToString(),
                    });
                }
            return View(model);
            }

        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
        public ActionResult BoothCount()
            {
            string constring = ConfigurationManager.ConnectionStrings["ReportConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT Booth FROM LeadContacts WHERE Booth IS NOT NULL;";
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
                Response.AddHeader("content-disposition", "attachment;filename= BoothCountReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    }
                }

            return RedirectToAction("Index", "BoothCount");
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
