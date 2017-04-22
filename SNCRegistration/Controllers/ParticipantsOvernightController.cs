using ClosedXML.Excel;
using SNCRegistration.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Mvc;


namespace SNCRegistration.Controllers
{
    public class ParticipantsOvernightController : Controller
    {
        // GET: ParticipantsOvernight
        public ActionResult Index(int? eventYear)
            {

            ViewBag.ddlEventYears = Enumerable.Range(2016, (DateTime.Now.Year - 2016) + 1).OrderByDescending(x => x).ToList();
            List<ParticipantsOvernightModel> model = new List<ParticipantsOvernightModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = String.Concat("SELECT ParticipantID, ParticipantFirstName as 'FirstName', ParticipantLastName as 'LastName', Description FROM Participants INNER JOIN Attendance ON Participants.AttendingCode = AttendanceID WHERE AttendanceID = 3 AND Participants.EventYear = @EventYear Union Select GuardianID, GuardianFirstName as 'FirstName', GuardianLastName as 'LastName', Description From Guardians INNER JOIN Attendance ON Guardians.AttendingCode = AttendanceID Where AttendanceID = 3 And Guardians.EventYear = @EventYear Union Select FamilyMemberID, FamilyMemberFirstName as 'FirstName', FamilyMemberLastName as 'LastName', Description From FamilyMembers INNER JOIN Attendance ON FamilyMembers.AttendingCode = AttendanceID Where AttendanceID = 3 And FamilyMembers.EventYear = @EventYear Order By FirstName ASC;");
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear != null ? eventYear.ToString() : DateTime.Now.Year.ToString());
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new ParticipantsOvernightModel()
                        {
                        FirstName = x["FirstName"].ToString(),
                        LastName = x["LastName"].ToString(),
                        Description = x["Description"].ToString()
                        }).ToList();
                    }
                }
            return View(model);
            }

        //Get the year onchange javascript
        public ActionResult GetParticipantsOvernightByYear(int eventYear)
            {
            List<ParticipantsOvernightModel> model = new List<ParticipantsOvernightModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = "SELECT ParticipantID, ParticipantFirstName as 'FirstName', ParticipantLastName as 'LastName', Description FROM Participants INNER JOIN Attendance ON Participants.AttendingCode = AttendanceID WHERE AttendanceID = 3 AND Participants.EventYear = @EventYear Union Select GuardianID, GuardianFirstName as 'FirstName', GuardianLastName as 'LastName', Description From Guardians INNER JOIN Attendance ON Guardians.AttendingCode = AttendanceID Where AttendanceID = 3 And Guardians.EventYear = @EventYear Union Select FamilyMemberID, FamilyMemberFirstName as 'FirstName', FamilyMemberLastName as 'LastName', Description From FamilyMembers INNER JOIN Attendance ON FamilyMembers.AttendingCode = AttendanceID Where AttendanceID = 3 And FamilyMembers.EventYear = @EventYear Order By FirstName ASC;";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear);
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new ParticipantsOvernightModel()
                        {
                        FirstName = x["FirstName"].ToString(),
                        LastName = x["LastName"].ToString(),
                        Description = x["Description"].ToString()
                        }).ToList();
                    }
                }
            return PartialView("_PartialParticipantsOvernightList", model);
            }

        //Export to excel
        public ActionResult ParticipantsOvernight(int eventYear)
            {
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT ParticipantID, ParticipantFirstName as 'FirstName', ParticipantLastName as 'LastName', Description FROM Participants INNER JOIN Attendance ON Participants.AttendingCode = AttendanceID WHERE AttendanceID = 3 AND Participants.EventYear = @EventYear Union Select GuardianID, GuardianFirstName as 'FirstName', GuardianLastName as 'LastName', Description From Guardians INNER JOIN Attendance ON Guardians.AttendingCode = AttendanceID Where AttendanceID = 3 And Guardians.EventYear = @EventYear Union Select FamilyMemberID, FamilyMemberFirstName as 'FirstName', FamilyMemberLastName as 'LastName', Description From FamilyMembers INNER JOIN Attendance ON FamilyMembers.AttendingCode = AttendanceID Where AttendanceID = 3 And FamilyMembers.EventYear = @EventYear Order By FirstName ASC;";
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
                Response.AddHeader("content-disposition", "attachment;filename= ParticipantsOvernightReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    }
                }
            return RedirectToAction("Index", "ParticipantsOvernight");
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