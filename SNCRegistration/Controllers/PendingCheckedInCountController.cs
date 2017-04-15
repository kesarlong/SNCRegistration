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
    public class PendingCheckedInCountController : Controller
    {
        readonly string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
        private SNCRegistrationEntities db = new SNCRegistrationEntities();

        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
        // GET: PendingCheckedInCount
        public ActionResult Index(int? eventYear)
            {
            ViewBag.ddlEventYears = Enumerable.Range(2016, (DateTime.Now.Year - 2016) + 1).OrderByDescending(x => x).ToList();
            List<PendingCheckedInCountModel> model = new List<PendingCheckedInCountModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = String.Concat("SELECT UnitChapterNumber = ' ', 'Participants' AS Registrant, ParticipantFirstName, ParticipantLastName, CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM Participants WHERE CheckedIn = 0 AND EventYear = @EventYear UNION SELECT UnitChapterNumber = ' ', 'Guardians', GuardianFirstName, GuardianLastName, CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM Guardians WHERE CheckedIn = 0 AND EventYear = @EventYear UNION SELECT UnitChapterNumber = ' ', 'FamilyMembers', FamilyMemberFirstName, FamilyMemberLastName,  CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM FamilyMembers WHERE CheckedIn = 0 AND EventYear = @EventYear UNION SELECT UnitChapterNumber, 'LeadContacts', LeadContactFirstName, LeadContactLastName,  CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM LeadContacts WHERE CheckedIn = 0 AND EventYear = @EventYear UNION SELECT UnitChapterNumber, 'Volunteers', VolunteerFirstName, VolunteerLastName, CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM Volunteers WHERE CheckedIn = 0 AND EventYear = @EventYear ORDER BY ParticipantFirstName ASC");
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear != null ? eventYear.ToString() : DateTime.Now.Year.ToString());
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new PendingCheckedInCountModel()
                        {
                        UnitChapterNumber = x["UnitChapterNumber"].ToString(),
                        Registrant = x["Registrant"].ToString(),
                        ParticipantFirstName = x["ParticipantFirstName"].ToString(),
                        ParticipantLastName = x["ParticipantLastName"].ToString(),
                        CheckedIn = x["CheckedIn"].ToString()
                        }).ToList();
                    }
                }
            return View(model);
            }

        //Get the year onchange javascript
        public ActionResult GetPendingCheckedInCountByYear(int eventYear)
            {
            List<PendingCheckedInCountModel> model = new List<PendingCheckedInCountModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = "SELECT UnitChapterNumber = ' ', 'Participants' AS Registrant, ParticipantFirstName, ParticipantLastName, CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM Participants WHERE CheckedIn = 0 AND EventYear = @EventYear UNION SELECT UnitChapterNumber = ' ', 'Guardians', GuardianFirstName, GuardianLastName, CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM Guardians WHERE CheckedIn = 0 AND EventYear = @EventYear UNION SELECT UnitChapterNumber = ' ', 'FamilyMembers', FamilyMemberFirstName, FamilyMemberLastName,  CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM FamilyMembers WHERE CheckedIn = 0 AND EventYear = @EventYear UNION SELECT UnitChapterNumber, 'LeadContacts', LeadContactFirstName, LeadContactLastName,  CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM LeadContacts WHERE CheckedIn = 0 AND EventYear = @EventYear UNION SELECT UnitChapterNumber, 'Volunteers', VolunteerFirstName, VolunteerLastName, CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM Volunteers WHERE CheckedIn = 0 AND EventYear = @EventYear ORDER BY ParticipantFirstName ASC";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear);
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new PendingCheckedInCountModel()
                        {
                        UnitChapterNumber = x["UnitChapterNumber"].ToString(),
                        Registrant = x["Registrant"].ToString(),
                        ParticipantFirstName = x["ParticipantFirstName"].ToString(),
                        ParticipantLastName = x["ParticipantLastName"].ToString(),
                        CheckedIn = x["CheckedIn"].ToString()
                        }).ToList();
                    }
                }
            return PartialView("_PartialPendingCheckedInList", model);
            }

        //Export to excel
        public ActionResult PendingCheckedInCount(int eventYear)
            {
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT UnitChapterNumber = ' ', 'Participants' AS Registrant, ParticipantFirstName, ParticipantLastName, CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM Participants WHERE CheckedIn = 0 AND EventYear = @EventYear UNION SELECT UnitChapterNumber = ' ', 'Guardians', GuardianFirstName, GuardianLastName, CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM Guardians WHERE CheckedIn = 0 AND EventYear = @EventYear UNION SELECT UnitChapterNumber = ' ', 'FamilyMembers', FamilyMemberFirstName, FamilyMemberLastName,  CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM FamilyMembers WHERE CheckedIn = 0 AND EventYear = @EventYear UNION SELECT UnitChapterNumber, 'LeadContacts', LeadContactFirstName, LeadContactLastName,  CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM LeadContacts WHERE CheckedIn = 0 AND EventYear = @EventYear UNION SELECT UnitChapterNumber, 'Volunteers', VolunteerFirstName, VolunteerLastName, CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM Volunteers WHERE CheckedIn = 0 AND EventYear = @EventYear ORDER BY ParticipantFirstName ASC";
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
                Response.AddHeader("content-disposition", "attachment;filename= PendingCheckedInCount.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    }
                }
            return RedirectToAction("Index", "PendingCheckedInCount");
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