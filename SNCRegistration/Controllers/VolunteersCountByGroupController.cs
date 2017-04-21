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
    public class VolunteersCountByGroupController : Controller

    {
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
        // GET: VolunteersCountByGroupController
        public ActionResult Index(int? eventYear)
            {

            // Dropdown List For Event Year
            ViewBag.ddlEventYears = Enumerable.Range(2016, (DateTime.Now.Year - 2016) + 1).OrderByDescending(x => x).ToList();

            List<VolunteersCountByGroupModel> model = new List<VolunteersCountByGroupModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();

            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = String.Concat("SELECT B.BSTypeDescription as GroupType, UnitChapterNumber as GroupNumber, COUNT(*) As Total FROM Volunteers inner join bstype as B on volunteers.bstype = B.bstypeid Where EventYear = @EventYear GROUP BY B.BSTypeDescription, UnitChapterNumber union " 
                + "SELECT B.BSTypeDescription as GroupType, UnitChapterNumber as GroupNumber, COUNT(*) As Total FROM LeadContacts inner join bstype as B on leadContacts.bstype = B.bstypeid Where EventYear = @EventYear GROUP BY B.BSTypeDescription, UnitChapterNumber");
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear != null ? eventYear.ToString() : DateTime.Now.Year.ToString());
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new VolunteersCountByGroupModel()
                        {
                        GroupType = x["GroupType"].ToString(),
                        GroupNumber = x["GroupNumber"].ToString(),
                        Total = Convert.ToInt32(x["Total"].ToString())
                        }).ToList();
                    }
                }
            return View(model);
            }

        //Get the year onchange javascript
        public ActionResult GetVolunteersCountByGroupByYear(int eventYear)
            {
            List<VolunteersCountByGroupModel> model = new List<VolunteersCountByGroupModel>();
            string query = String.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(constring))
                {
                dt = new DataTable();
                connection.Open();
                query = "SELECT B.BSTypeDescription as GroupType, UnitChapterNumber as GroupNumber, COUNT(*) As Total FROM Volunteers inner join bstype as B on volunteers.bstype = B.bstypeid Where EventYear = @EventYear GROUP BY B.BSTypeDescription, UnitChapterNumber union "
                + "SELECT B.BSTypeDescription as GroupType, UnitChapterNumber as GroupNumber, COUNT(*) As Total FROM LeadContacts inner join bstype as B on leadContacts.bstype = B.bstypeid Where EventYear = @EventYear GROUP BY B.BSTypeDescription, UnitChapterNumber";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                    adapter.SelectCommand.Parameters.AddWithValue("@EventYear", eventYear);
                    adapter.Fill(dt);
                    model = dt.AsEnumerable().Select(x => new VolunteersCountByGroupModel()
                        {
                        GroupType = x["GroupType"].ToString(),
                        GroupNumber = x["GroupNumber"].ToString(),
                        Total = Convert.ToInt32(x["Total"].ToString())
                        }).ToList();
                    }
                }
            return PartialView("_PartialVolunteersCountByGroupList", model);
            }


        //Export to excel
        public ActionResult VolunteersCountByGroup(int eventYear)
            {
            string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT B.BSTypeDescription as GroupType, UnitChapterNumber as GroupNumber, COUNT(*) As Total FROM Volunteers inner join bstype as B on volunteers.bstype = B.bstypeid Where EventYear = @EventYear GROUP BY B.BSTypeDescription, UnitChapterNumber union "
                + "SELECT B.BSTypeDescription as GroupType, UnitChapterNumber as GroupNumber, COUNT(*) As Total FROM LeadContacts inner join bstype as B on leadContacts.bstype = B.bstypeid Where EventYear = @EventYear GROUP BY B.BSTypeDescription, UnitChapterNumber";
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
                Response.AddHeader("content-disposition", "attachment;filename= VolunteersCountyByGroup.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    }
                }
            return RedirectToAction("Index", "VolunteersCountByGroup");
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
