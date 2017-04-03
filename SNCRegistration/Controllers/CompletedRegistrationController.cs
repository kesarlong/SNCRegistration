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
    public class CompletedRegistrationController : Controller
    {
        // GET: CompletedRegistration
        public ActionResult Index(int? eventYear)
            {

            // Dropdown List For Event Year
            ViewBag.ddlEventYears = Enumerable.Range(2016, (DateTime.Now.Year - 2016) + 1).OrderByDescending(x => x).ToList();

            List<CompletedRegistrationModel> model = new List<CompletedRegistrationModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();

            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = String.Concat("SELECT 'Participant' AS Registrant, ParticipantFirstName, ParticipantLastName, HealthForm, PhotoAck FROM Participants WHERE HealthFOrm = 1 AND PhotoAck = 1 AND EventYear = @EventYear " 
                    + "UNION SELECT 'Guardian', GuardianFirstName, GuardianLastName, HealthFOrm, PhotoAck FROM Guardians WHERE HealthForm = 1 AND PhotoAck = 1 AND EventYear = @EventYear " 
                    + "UNION SELECT 'FamilyMember', FamilyMemberFirstName, FamilyMemberLastName, HealthForm, PhotoAck FROM FamilyMembers WHERE HealthForm = 1 AND PhotoAck = 1  AND EventYear = @EventYear " 
                    + "ORDER BY ParticipantFirstName ASC");
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear != null ? eventYear.ToString() : DateTime.Now.Year.ToString());
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new CompletedRegistrationModel()
                        {
                        Registrant = x["Registrant"].ToString(),
                        ParticipantFirstName = x["ParticipantFirstName"].ToString(),
                        ParticipantLastName = x["ParticipantLastName"].ToString(),
                        HealthForm = x["HealthForm"].ToString(),
                        PhotoAck = x["PhotoAck"].ToString(),
                        }).ToList();
                    }
                }
            return View(model);
            }

        //Get the year onchange javascript
        public ActionResult GetCompletedRegistrationByYear(int eventYear)
            {
            List<CompletedRegistrationModel> model = new List<CompletedRegistrationModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = "SELECT 'Participant' AS Registrant, ParticipantFirstName, ParticipantLastName, HealthForm, PhotoAck FROM Participants WHERE HealthFOrm = 1 AND PhotoAck = 1 AND EventYear = @EventYear " 
                    + "UNION SELECT 'Guardian', GuardianFirstName, GuardianLastName, HealthFOrm, PhotoAck FROM Guardians WHERE HealthForm = 1 AND PhotoAck = 1 AND EventYear = @EventYear "
                    + "UNION SELECT 'FamilyMember', FamilyMemberFirstName, FamilyMemberLastName, HealthForm, PhotoAck FROM FamilyMembers WHERE HealthForm = 1 AND PhotoAck = 1  AND EventYear = @EventYear "
                    + "ORDER BY ParticipantFirstName ASC";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear);
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new CompletedRegistrationModel()
                        {
                        Registrant = x["Registrant"].ToString(),
                        ParticipantFirstName = x["ParticipantFirstName"].ToString(),
                        ParticipantLastName = x["ParticipantLastName"].ToString(),
                        HealthForm = x["HealthForm"].ToString(),
                        PhotoAck = x["PhotoAck"].ToString(),
                        }).ToList();
                    }
                }
            return PartialView("_PartialCompletedRegistrationList", model);
            }
        
        //Export to excel
        public ActionResult CompletedRegistrationReport(int eventYear)
            {
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT 'Participant' AS Registrant, ParticipantFirstName, ParticipantLastName, HealthForm, PhotoAck FROM Participants WHERE HealthFOrm = 1 AND PhotoAck = 1 AND EventYear = @EventYear "
                    + "UNION SELECT 'Guardian', GuardianFirstName, GuardianLastName, HealthFOrm, PhotoAck FROM Guardians WHERE HealthForm = 1 AND PhotoAck = 1 AND EventYear = @EventYear "
                    + "UNION SELECT 'FamilyMember', FamilyMemberFirstName, FamilyMemberLastName, HealthForm, PhotoAck FROM FamilyMembers WHERE HealthForm = 1 AND PhotoAck = 1  AND EventYear = @EventYear "
                    + "ORDER BY ParticipantFirstName ASC";
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            dt.TableName = "Participants";
            con.Open();
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
                Response.AddHeader("content-disposition", "attachment;filename= CompletedRegistrationReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    }
                }
            return RedirectToAction("Index", "CompletedRegistration");
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
