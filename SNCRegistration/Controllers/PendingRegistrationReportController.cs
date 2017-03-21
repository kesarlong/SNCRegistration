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
    public class PendingRegistrationReportController : Controller
    {
        // GET: PendingRegistrationsReport
        public ActionResult Index()
            {
            string constring = ConfigurationManager.ConnectionStrings["ReportConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT ParticipantFirstName, ParticipantLastName, HealthForm, PhotoAck FROM Participants WHERE HealthFOrm = 0 AND PhotoAck = 0 UNION SELECT GuardianFirstName, GuardianLastName, HealthFOrm, PhotoAck FROM Guardians WHERE HealthForm = 0 AND PhotoAck = 0 UNION SELECT FamilyMemberFirstName, FamilyMemberLastName, HealthForm, PhotoAck FROM FamilyMembers WHERE HealthForm = 0 AND PhotoAck = 0;";
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.Fill(dt);
            con.Close();
            IList<PendingRegistrationsReport> model = new List<PendingRegistrationsReport>();
            for (int i = 0; i < dt.Rows.Count; i++)
                {
                model.Add(new PendingRegistrationsReport()
                    {
                    ParticipantFirstName = dt.Rows[i]["ParticipantFirstName"].ToString(),
                    ParticipantLastName = dt.Rows[i]["ParticipantLastName"].ToString(),
                    HealthForm = dt.Rows[i]["HealthForm"].ToString(),
                    PhotoAck = dt.Rows[i]["PhotoAck"].ToString()
                    });
                }
            return View(model);
            }

        public ActionResult PendingRegistrationReport()
            {
            string constring = ConfigurationManager.ConnectionStrings["ReportConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT ParticipantFirstName, ParticipantLastName, HealthForm, PhotoAck FROM Participants WHERE HealthFOrm = 0 AND PhotoAck = 0 UNION SELECT GuardianFirstName, GuardianLastName, HealthFOrm, PhotoAck FROM Guardians WHERE HealthForm = 0 AND PhotoAck = 0 UNION SELECT FamilyMemberFirstName, FamilyMemberLastName, HealthForm, PhotoAck FROM FamilyMembers WHERE HealthForm = 0 AND PhotoAck = 0;";
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
                Response.AddHeader("content-disposition", "attachment;filename= PendingRegistrationReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    }
                }

            return RedirectToAction("Index", "PeopleCheckedInCount");
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
