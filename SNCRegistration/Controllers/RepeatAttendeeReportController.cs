using ClosedXML.Excel;
using SNCRegistration.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;
using PagedList;

namespace SNCRegistration.Controllers
    {
    public class RepeatAttendeeReportController: Controller
        {
     
        // GET: RepeatAttendeeReportController
        public ActionResult Index()
            {
            string constring = ConfigurationManager.ConnectionStrings["ReportConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT ParticipantFirstName, ParticipantLastName, Returning, Description FROM Participants INNER JOIN Attendance ON AttendingCode = AttendanceID WHERE Returning = 1 Order By Description;";
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.Fill(dt);
            con.Close();
            IList<RepeatAttendeeReport> model = new List<RepeatAttendeeReport>();
            for (int i = 0; i < dt.Rows.Count; i++)
                {
                model.Add(new RepeatAttendeeReport()
                    {

                    ParticipantFirstName = dt.Rows[i]["ParticipantFirstName"].ToString(),
                    ParticipantLastName = dt.Rows[i]["ParticipantLastName"].ToString(),
                    Returning = dt.Rows[i]["Returning"].ToString(),
                    Description = dt.Rows[i]["Description"].ToString()
                    });
                }
            return View(model);
            }

        public ActionResult RepeatAttendeeReport()
            {
            string constring = ConfigurationManager.ConnectionStrings["ReportConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT ParticipantFirstName, ParticipantLastName, Returning, Discription FROM Participants INNER JOIN Attendance ON AttendingCode = AttendanceID WHERE Returning = 1 Order By Description;";
            DataTable dt = new DataTable();
            dt.TableName = "Participants";
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
                Response.AddHeader("content-disposition", "attachment;filename= RepeatAttendeeReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    }
                }

            return RedirectToAction("Index", "RepeatAttendeeReport");
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
