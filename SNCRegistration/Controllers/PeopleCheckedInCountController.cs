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
    public class PeopleCheckedInCountController : Controller
    {
        // GET: PeopleCheckedInCount
        public ActionResult Index()
            {
            string constring = ConfigurationManager.ConnectionStrings["ReportConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT ParticipantFirstName, ParticipantLastName, CheckedIn FROM Participants WHERE CheckedIn = 1 UNION SELECT GuardianFirstName, GuardianLastName, CheckedIn FROM Guardians WHERE CheckedIn = 1 UNION SELECT FamilyMemberFirstName, FamilyMemberLastName, CheckedIn FROM FamilyMembers WHERE CheckedIn = 1 UNION SELECT LeadContactFirstName, LeadContactLastName, CheckedIn FROM LeadContacts WHERE CheckedIn = 1 UNION SELECT VolunteerFirstName, VolunteerLastName, CheckedIn FROM Volunteers WHERE CheckedIn = 1; ";
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.Fill(dt);
            con.Close();
            IList<PeopleCheckedInCountModel> model = new List<PeopleCheckedInCountModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
                {
                model.Add(new PeopleCheckedInCountModel()
                    {
                    ParticipantFirstName = dt.Rows[i]["ParticipantFirstName"].ToString(),
                    ParticipantLastName = dt.Rows[i]["ParticipantLastName"].ToString(),
                    CheckedIn = dt.Rows[i]["CheckedIn"].ToString(),
                    });
                }
            return View(model);
            }

        public ActionResult PeopleCheckedInCount()
            {
            string constring = ConfigurationManager.ConnectionStrings["ReportConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT ParticipantFirstName, ParticipantLastName, CheckedIn FROM Participants WHERE CheckedIn = 1 UNION SELECT GuardianFirstName, GuardianLastName, CheckedIn FROM Guardians WHERE CheckedIn = 1 UNION SELECT FamilyMemberFirstName, FamilyMemberLastName, CheckedIn FROM FamilyMembers WHERE CheckedIn = 1 UNION SELECT LeadContactFirstName, LeadContactLastName, CheckedIn FROM LeadContacts WHERE CheckedIn = 1 UNION SELECT VolunteerFirstName, VolunteerLastName, CheckedIn FROM Volunteers WHERE CheckedIn = 1; ";
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
                Response.AddHeader("content-disposition", "attachment;filename= PeopleCheckedInCountReport.xlsx");

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
