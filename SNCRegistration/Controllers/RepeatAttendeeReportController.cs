﻿using ClosedXML.Excel;
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
    public class RepeatAttendeeReportController: Controller
        {

        // GET: RepeatAttendeeReportController
        public ActionResult Index(int? eventYear)
            {

            ViewBag.ddlEventYears = Enumerable.Range(2016, (DateTime.Now.Year - 2016) + 1).OrderByDescending(x => x).ToList();

            List<RepeatAttendeeReportModel> model = new List<RepeatAttendeeReportModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = String.Concat("SELECT ParticipantFirstName, ParticipantLastName, CASE WHEN Returning = 1 THEN 'Yes' ELSE 'No' END AS Returning, Description FROM Participants INNER JOIN Attendance ON AttendingCode = AttendanceID WHERE Returning = 1 AND EventYear = @EventYear Order By Description");

                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear != null ? eventYear.ToString() : DateTime.Now.Year.ToString());
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new RepeatAttendeeReportModel()
                        {

                        ParticipantFirstName = x["ParticipantFirstName"].ToString(),
                        ParticipantLastName = x["ParticipantLastName"].ToString(),
                        Returning = x["Returning"].ToString(),
                        Description = x["Description"].ToString()
                        }).ToList();
                    }
                }
            return View(model);
            }

        //Get the year onchange javascript
        public ActionResult GetRepeatAttendeeByYear(int eventYear)
            {
            List<RepeatAttendeeReportModel> model = new List<RepeatAttendeeReportModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = "SELECT ParticipantFirstName, ParticipantLastName, CASE WHEN Returning = 1 THEN 'Yes' ELSE 'No' END AS Returning, Description FROM Participants INNER JOIN Attendance ON AttendingCode = AttendanceID WHERE Returning = 1 AND EventYear = @EventYear Order By Description";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear);
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new RepeatAttendeeReportModel()
                        {

                        ParticipantFirstName = x["ParticipantFirstName"].ToString(),
                        ParticipantLastName = x["ParticipantLastName"].ToString(),
                        Returning = x["Returning"].ToString(),
                        Description = x["Description"].ToString()
                        }).ToList();
                    }
                }
            return PartialView("_PartialRepeatAttendeeList", model);
            }

        //Export to excel
        public ActionResult RepeatAttendeeReport(int eventYear)
            {
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT ParticipantFirstName, ParticipantLastName, CASE WHEN Returning = 1 THEN 'Yes' ELSE 'No' END AS Returning, Description FROM Participants INNER JOIN Attendance ON AttendingCode = AttendanceID WHERE Returning = 1 AND EventYear = @EventYear Order By Description";
            DataTable dt = new DataTable();
            dt.TableName = "Participants";
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
