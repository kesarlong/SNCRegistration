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
    public class FamilyFriendsReportController : Controller
    {
        readonly string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
        private SNCRegistrationEntities db = new SNCRegistrationEntities();

        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin")]
        // GET: FamilyFriendsReport
        public ActionResult Index(int? eventYear)
            {

            ViewBag.ddlEventYears = Enumerable.Range(2016, (DateTime.Now.Year - 2016) + 1).OrderByDescending(x => x).ToList();

            List<FamilyFriendsReport> model = new List<FamilyFriendsReport>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = String.Concat("SELECT FamilyMemberID, FamilyMemberFirstName, FamilyMemberLastName, FamilyMemberAge, AgeDescription, "
      + "FamilyMembers.GuardianID, GuardianFirstName, GuardianLastName, CASE WHEN FamilyMembers.HealthForm = 1 THEN 'Yes' ELSE 'No' END AS HeatlhForm, "
      + "CASE WHEN FamilyMembers.PhotoAck = 1 THEN 'Yes' ELSE 'No' END AS PhotoAck, Attendance.Description AS 'Attending', "
      + "CASE WHEN FamilyMembers.CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS Checkedin, '' AS 'Campsite' "
      + "FROM FamilyMembers INNER JOIN Guardians ON Guardians.GuardianID = FamilyMembers.GuardianID "
      + "INNER JOIN Attendance ON AttendanceID = FamilyMembers.AttendingCode INNER JOIN Age ON FamilyMemberAge = AgeID "
      + "WHERE FamilyMembers.EventYear = @EventYear ORDER BY FamilyMemberFirstName ASC");

                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear != null ? eventYear.ToString() : DateTime.Now.Year.ToString());
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new FamilyFriendsReport()
                        {
                        FamilyMemberID = Convert.ToInt32(x["FamilyMemberID"]),
                        FamilyMemberFirstName = x["FamilyMemberFirstName"].ToString(),
                        FamilyMemberLastName = x["FamilyMemberLastName"].ToString(),
                        }).ToList();
                    }
                }
            return View(model);
            }

        //Get the year onchange javascript
        public ActionResult GetFamilyFriendsReportByYear(int eventYear)
            {
            List<FamilyFriendsReport> model = new List<FamilyFriendsReport>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = "SELECT FamilyMemberID, FamilyMemberFirstName, FamilyMemberLastName, FamilyMemberAge, AgeDescription, "
      + "FamilyMembers.GuardianID, GuardianFirstName, GuardianLastName, CASE WHEN FamilyMembers.HealthForm = 1 THEN 'Yes' ELSE 'No' END AS HeatlhForm, "
      + "CASE WHEN FamilyMembers.PhotoAck = 1 THEN 'Yes' ELSE 'No' END AS PhotoAck, Attendance.Description AS 'Attending', "
      + "CASE WHEN FamilyMembers.CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS Checkedin, '' AS 'Campsite' "
      + "FROM FamilyMembers INNER JOIN Guardians ON Guardians.GuardianID = FamilyMembers.GuardianID "
      + "INNER JOIN Attendance ON AttendanceID = FamilyMembers.AttendingCode INNER JOIN Age ON FamilyMemberAge = AgeID "
      + "WHERE FamilyMembers.EventYear = @EventYear ORDER BY FamilyMemberFirstName ASC";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear);
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new FamilyFriendsReport()
                        {
                        FamilyMemberID = Convert.ToInt32(x["FamilyMemberID"]),
                        FamilyMemberFirstName = x["FamilyMemberFirstName"].ToString(),
                        FamilyMemberLastName = x["FamilyMemberLastName"].ToString(),
                        }).ToList();
                    }
                }
            return PartialView("_PartialFamilyFriendsReportList", model);
            }

        //Export to excel
        public ActionResult FamilyFriendsReport(int eventYear)
            {
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT FamilyMemberID, FamilyMemberFirstName, FamilyMemberLastName, FamilyMemberAge, AgeDescription, "
      + "FamilyMembers.GuardianID, GuardianFirstName, GuardianLastName, CASE WHEN FamilyMembers.HealthForm = 1 THEN 'Yes' ELSE 'No' END AS HeatlhForm, "
      + "CASE WHEN FamilyMembers.PhotoAck = 1 THEN 'Yes' ELSE 'No' END AS PhotoAck, Attendance.Description AS 'Attending', "
      + "CASE WHEN FamilyMembers.CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS Checkedin, '' AS 'Campsite' "
      + "FROM FamilyMembers INNER JOIN Guardians ON Guardians.GuardianID = FamilyMembers.GuardianID "
      + "INNER JOIN Attendance ON AttendanceID = FamilyMembers.AttendingCode INNER JOIN Age ON FamilyMemberAge = AgeID "
      + "WHERE FamilyMembers.EventYear = @EventYear ORDER BY FamilyMemberFirstName ASC";
            DataTable dt = new DataTable();
            dt.TableName = "FamilyMembers";
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
                Response.AddHeader("content-disposition", "attachment;filename= FamilyFriendsReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    }
                }
            return RedirectToAction("Index", "FamilyFriendsReport");
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
