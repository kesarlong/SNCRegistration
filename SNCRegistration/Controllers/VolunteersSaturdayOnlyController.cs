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
    public class VolunteersSaturdayOnlyController : Controller
    {
        // GET: VolunteersSaturdayOnly
        public ActionResult Index()
            {
            string constring = ConfigurationManager.ConnectionStrings["ReportConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT *  FROM Volunteers INNER JOIN Attendance ON VolunteerAttendingCode = AttendanceID WHERE AttendanceID = 2;";
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.Fill(dt);
            con.Close();
            IList<VolunteersSaturdayOnlyModel> model = new List<VolunteersSaturdayOnlyModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
                {
                model.Add(new VolunteersSaturdayOnlyModel()
                    {

                    VolunteerFirstName = dt.Rows[i]["VolunteerFirstName"].ToString(),
                    VolunteerLastName = dt.Rows[i]["VolunteerLastName"].ToString(),
                    Description = dt.Rows[i]["Description"].ToString(),
                    });
                }
            return View(model);
            }

        public ActionResult VolunteersSaturdayOnly()
            {
            string constring = ConfigurationManager.ConnectionStrings["ReportConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT *  FROM Volunteers INNER JOIN Attendance ON VolunteerAttendingCode = AttendanceID WHERE AttendanceID = 2;";
            DataTable dt = new DataTable();
            dt.TableName = "Volunteers";
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
                Response.AddHeader("content-disposition", "attachment;filename= VolunteersSaturdayOnlyReport.xlsx");

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