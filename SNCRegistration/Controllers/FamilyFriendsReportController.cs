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
        // GET: FamilyFriendsReport
        public ActionResult Index()
            {
            string constring = ConfigurationManager.ConnectionStrings["ReportConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT * FROM FamilyMembers";
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.Fill(dt);
            con.Close();
            IList<ViewModels.FamilyFriendsReport> model = new List<ViewModels.FamilyFriendsReport>();
            for (int i = 0; i < dt.Rows.Count; i++)
                {
                model.Add(new ViewModels.FamilyFriendsReport()
                    {
                    FamilyMemberID = Convert.ToInt32(dt.Rows[i]["FamilyMemberID"]),
                    FamilyMemberFirstName = dt.Rows[i]["FamilyMemberFirstName"].ToString(),
                    FamilyMemberLastName = dt.Rows[i]["FamilyMemberLastName"].ToString(),
                    });
                }
            return View(model);
            }

        public ActionResult FamilyFriendsReport()
            {
            string constring = ConfigurationManager.ConnectionStrings["ReportConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT * FROM FamilyMembers";
            DataTable dt = new DataTable();
            dt.TableName = "FamilyMembers";
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
                Response.AddHeader("content-disposition", "attachment;filename= FamilyMemberReport.xlsx");

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