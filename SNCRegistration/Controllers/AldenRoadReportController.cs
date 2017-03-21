﻿using ClosedXML.Excel;
using SNCRegistration.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;
using PagedList;

namespace SNCRegistration.Controllers
    {
    public class AldenRoadReportController : Controller
        {
     
        // GET: Reporting
        public ActionResult Index()
            {
            string constring = ConfigurationManager.ConnectionStrings["ReportConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT * FROM Participants INNER JOIN Age ON ParticipantAge = AgeID WHERE ParticipantSchool LIKE '%Alden%';";
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.Fill(dt);
            con.Close();
            IList<AldenRoadReportModel> model = new List<AldenRoadReportModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
                {
                model.Add(new AldenRoadReportModel()
                    {
                    ParticipantID = Convert.ToInt32(dt.Rows[i]["ParticipantID"]),
                    ParticipantFirstName = dt.Rows[i]["ParticipantFirstName"].ToString(),
                    ParticipantLastName = dt.Rows[i]["ParticipantLastName"].ToString(),
                    ParticipantSchool = dt.Rows[i]["ParticipantSChool"].ToString()
                    });
                }
            return View(model);
            }

        public ActionResult AldenRoadReport()
            {
            string constring = ConfigurationManager.ConnectionStrings["ReportConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = "SELECT * FROM Participants INNER JOIN Age ON ParticipantAge = AgeID WHERE ParticipantSchool LIKE '%Alden%';";
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
                Response.AddHeader("content-disposition", "attachment;filename= AldenRoadReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    }
                }

            return RedirectToAction("Index", "AldenRoadReport");
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
