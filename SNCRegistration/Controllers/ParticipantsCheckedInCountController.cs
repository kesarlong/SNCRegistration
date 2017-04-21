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
    public class ParticipantsCheckedInCountController : Controller
    {
        readonly string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
        private SNCRegistrationEntities db = new SNCRegistrationEntities();

        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
        // GET: ParticipantsCheckedInCount
        public ActionResult Index(int? eventYear)
            {
            ViewBag.ddlEventYears = Enumerable.Range(2016, (DateTime.Now.Year - 2016) + 1).OrderByDescending(x => x).ToList();
            List<ParticipantsCheckedInCountModel> model = new List<ParticipantsCheckedInCountModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = String.Concat("SELECT ParticipantID, ParticipantFirstName as 'FirstName', ParticipantLastName as 'LastName', CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM Participants WHERE CheckedIn = 1 AND EventYear = @EventYear UNION SELECT GuardianID, GuardianFirstName as 'FirstName', GuardianLastName as 'LastName', CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM Guardians WHERE CheckedIn = 1 AND EventYear = @EventYear UNION SELECT FamilyMemberID, FamilyMemberFirstName as 'FirstName', FamilyMemberLastName as 'LastName',  CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM FamilyMembers WHERE CheckedIn = 1 AND EventYear = @EventYear ORDER BY FirstName ASC");
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear != null ? eventYear.ToString() : DateTime.Now.Year.ToString());
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new ParticipantsCheckedInCountModel()
                        {
                        FirstName = x["FirstName"].ToString(),
                        LastName = x["LastName"].ToString(),
                        CheckedIn = x["CheckedIn"].ToString()
                        }).ToList();
                    }
                }
            return View(model);
            }

        //Get the year onchange javascript
        public ActionResult GetParticipantsCheckedInCountByYear(int eventYear)
            {
            List<ParticipantsCheckedInCountModel> model = new List<ParticipantsCheckedInCountModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = "SELECT ParticipantID, ParticipantFirstName as 'FirstName', ParticipantLastName as 'LastName', CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM Participants WHERE CheckedIn = 1 AND EventYear = @EventYear UNION SELECT GuardianID, GuardianFirstName as 'FirstName', GuardianLastName as 'LastName', CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM Guardians WHERE CheckedIn = 1 AND EventYear = @EventYear UNION SELECT FamilyMemberID, FamilyMemberFirstName as 'FirstName', FamilyMemberLastName as 'LastName',  CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM FamilyMembers WHERE CheckedIn = 1 AND EventYear = @EventYear ORDER BY FirstName ASC";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear);
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new ParticipantsCheckedInCountModel()
                        {
                        FirstName = x["FirstName"].ToString(),
                        LastName = x["LastName"].ToString(),
                        CheckedIn = x["CheckedIn"].ToString()
                        }).ToList();
                    }
                }
            return PartialView("_PartialParticipantsCheckedInList", model);
            }

        //Export to excel
        public ActionResult ParticipantsCheckedInCount(int eventYear)
            {
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT ParticipantID, ParticipantFirstName as 'FirstName', ParticipantLastName as 'LastName', CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM Participants WHERE CheckedIn = 1 AND EventYear = @EventYear UNION SELECT GuardianID, GuardianFirstName as 'FirstName', GuardianLastName as 'LastName', CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM Guardians WHERE CheckedIn = 1 AND EventYear = @EventYear UNION SELECT FamilyMemberID, FamilyMemberFirstName as 'FirstName', FamilyMemberLastName as 'LastName',  CASE WHEN CheckedIn = 1 THEN 'Yes' ELSE 'No' END AS CheckedIn FROM FamilyMembers WHERE CheckedIn = 1 AND EventYear = @EventYear ORDER BY FirstName ASC";
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
                Response.AddHeader("content-disposition", "attachment;filename= ParticipantsCheckedInCount.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    }
                }
            return RedirectToAction("Index", "ParticipantsCheckedInCount");
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