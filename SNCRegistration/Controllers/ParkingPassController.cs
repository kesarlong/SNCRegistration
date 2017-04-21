using ClosedXML.Excel;
using SNCRegistration.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;
using System.Linq;

namespace SNCRegistration.Controllers
    {
    public class ParkingPassController : Controller
        {
        readonly string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
        private SNCRegistrationEntities db = new SNCRegistrationEntities();

        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
        // GET: Reporting
        public ActionResult Index(int? eventYear)
            {

            ViewBag.ddlEventYears = Enumerable.Range(2016, (DateTime.Now.Year - 2016) + 1).OrderByDescending(x => x).ToList();

            List<ParkingPassModel> model = new List<ParkingPassModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = String.Concat("SELECT DISTINCT GuardianID, GuardianFirstName, GuardianLastName, GuardianCellphone FROM Guardians WHERE EventYear = @EventYear ORDER BY GuardianFirstName");
                  
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear != null ? eventYear.ToString() : DateTime.Now.Year.ToString());
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new ParkingPassModel()
                        {
                        GuardianID = Convert.ToInt32(x["GuardianID"]),
                        GuardianFirstName = x["GuardianFirstName"].ToString(),
                        GuardianLastName = x["GuardianLastName"].ToString(),
                        GuardianCellPhone = x["GuardianCellPhone"].ToString(),
                        }).ToList();
                    }
                }
            return View(model);
            }

        //Get the year onchange javascript
        public ActionResult GetParkingPassByYear(int eventYear)
            {
            List<ParkingPassModel> model = new List<ParkingPassModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = "SELECT DISTINCT GuardianID, GuardianFirstName, GuardianLastName, GuardianCellphone FROM Guardians WHERE EventYear = @EventYear ORDER BY GuardianFirstName";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear);
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new ParkingPassModel()
                        {
                        GuardianID = Convert.ToInt32(x["GuardianID"]),
                        GuardianFirstName = x["GuardianFirstName"].ToString(),
                        GuardianLastName = x["GuardianLastName"].ToString(),
                        GuardianCellPhone = x["GuardianCellPhone"].ToString(),
                        }).ToList();
                    }
                }
            return PartialView("_PartialParkingPassList", model);
            }

        //Export to excel
        public ActionResult ParkingPass(int eventYear)
            {
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT DISTINCT GuardianID, GuardianFirstName, GuardianLastName, GuardianCellphone FROM Guardians WHERE EventYear = @EventYear ORDER BY GuardianFirstName";
            DataTable dt = new DataTable();
            dt.TableName = "Guardians";
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
                Response.AddHeader("content-disposition", "attachment;filename= ParkingPass.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    }
                }
            return RedirectToAction("Index", "ParkingPass");
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
