using SNCRegistration.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SNCRegistration.Controllers
{
    public class DashboardController : Controller
        {

        readonly string constring = ConfigurationManager.ConnectionStrings["SNCRegistrationConnectionString"].ConnectionString;
        private SNCRegistrationEntities db = new SNCRegistrationEntities();
        // GET: Dashboard
        [CustomAuthorize(Roles = "SystemAdmin, FullAdmin, VolunteerAdmin")]
        public ActionResult Index()
            {
            ViewBag.ParticipantsCount = db.Participants.Count();
            ViewBag.VounteersFridayOnlyCount = GetVounteersFridayOnlyCount();
            ViewBag.VounteersSaturdayOnlyCount = GetVounteersSaturdayOnlyCount();
            ViewBag.VolunteersFridayThruSaturdayCount = GetVolunteersFridayThruSaturdayCount();
            ViewBag.VolunteersOvernightCount = GetVolunteersOvernightCount();
            ViewBag.VolunteersCount = GetVolunteersCount();
            ViewBag.FamilyMembersCount = db.FamilyMembers.Count();
            ViewBag.PendingRegistrationCount = GetPendingRegistrationCount();
            ViewBag.CompletedRegistrationCount = GetCompletedRegistrationCount();
            ViewBag.FirstTimeAttendeeCount = GetFirstTimeAttendeeCount();
            ViewBag.RepeatAttendeeCount = GetRepeatAttendeeCount();
            ViewBag.TeeShirtOrdersCount = GetTeeShirtOrdersCount();
            ViewBag.BoothCount = GetBoothCount();
            ViewBag.WristBandCount = GetWristBandCount();
            ViewBag.PeopleCheckedInCount = GetPeopleCheckedInCount();
            ViewBag.PendingCheckedInCount = GetPendingCheckedInCount();
            ViewBag.AldenRoadCount = GetAldenRoadCount();
            ViewBag.ParkingPassCount = db.Guardians.Count();
            return View();
            }
        protected override void Dispose(bool disposing)
            {
            if (disposing)
                {
                db.Dispose();
                }
            base.Dispose(disposing);
            }

        private int GetVounteersFridayOnlyCount()
            {
            using (var connection = new SqlConnection(constring))
                {
                connection.Open();
                string query = "SELECT Count(*)  FROM Volunteers INNER JOIN Attendance ON VolunteerAttendingCode = AttendanceID WHERE AttendanceID = @AttendanceID";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                    cmd.Parameters.AddWithValue("@AttendanceID", 1);
                    return (int)cmd.ExecuteScalar();
                    }
                }
            }

        private int GetVounteersSaturdayOnlyCount()
            {
            using (var connection = new SqlConnection(constring))
                {
                connection.Open();
                string query = "SELECT Count(*)  FROM Volunteers INNER JOIN Attendance ON VolunteerAttendingCode = AttendanceID WHERE AttendanceID = @AttendanceID";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                    cmd.Parameters.AddWithValue("@AttendanceID", 2);
                    return (int)cmd.ExecuteScalar();
                    }
                }
            }

        private int GetVolunteersOvernightCount()
            {
            using (var connection = new SqlConnection(constring))
                {
                connection.Open();
                string query = "SELECT Count(*)  FROM Volunteers INNER JOIN Attendance ON VolunteerAttendingCode = AttendanceID WHERE AttendanceID = @AttendanceID";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                    cmd.Parameters.AddWithValue("@AttendanceID", 3);
                    return (int)cmd.ExecuteScalar();
                    }
                }
            }

        private int GetVolunteersFridayThruSaturdayCount()
            {
            using (var connection = new SqlConnection(constring))
                {
                connection.Open();
                string query = "SELECT Count(*)  FROM Volunteers INNER JOIN Attendance ON VolunteerAttendingCode = AttendanceID WHERE AttendanceID = @AttendanceID";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                    cmd.Parameters.AddWithValue("@AttendanceID", 4);
                    return (int)cmd.ExecuteScalar();
                    }
                }
            }

        private int GetVolunteersCount()
            {
            using (var connection = new SqlConnection(constring))
                {
                connection.Open();
                string query = "SELECT Count(*)  FROM Volunteers";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                    return (int)cmd.ExecuteScalar();
                    }
                }
            }

        private int GetPendingRegistrationCount()
            {
            using (var connection = new SqlConnection(constring))
                {
                connection.Open();
                string query = "select COUNT(*) from ( SELECT ParticipantFirstName, ParticipantLastName, HealthForm, PhotoAck FROM Participants WHERE HealthForm = @HealthForm AND PhotoAck = @PhotoAck UNION SELECT GuardianFirstName, GuardianLastName, HealthForm, PhotoAck FROM Guardians WHERE HealthForm = @HealthForm AND PhotoAck = @PhotoAck UNION SELECT FamilyMemberFirstName, FamilyMemberLastName, HealthForm, PhotoAck FROM FamilyMembers WHERE HealthForm = @HealthForm AND PhotoAck = @PhotoAck) as totalCount";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                    cmd.Parameters.AddWithValue("@HealthForm", 0);
                    cmd.Parameters.AddWithValue("@PhotoAck", 0);
                    return (int)cmd.ExecuteScalar();
                    }
                }
            }

        private int GetCompletedRegistrationCount()
            {
            using (var connection = new SqlConnection(constring))
                {
                connection.Open();
                string query = "select COUNT(*) from ( SELECT ParticipantFirstName, ParticipantLastName, HealthForm, PhotoAck FROM Participants WHERE HealthForm = @HealthForm AND PhotoAck = @PhotoAck UNION SELECT GuardianFirstName, GuardianLastName, HealthFOrm, PhotoAck FROM Guardians WHERE HealthForm = @HealthForm AND PhotoAck = @PhotoAck UNION SELECT FamilyMemberFirstName, FamilyMemberLastName, HealthForm, PhotoAck FROM FamilyMembers WHERE HealthForm = @HealthForm AND PhotoAck = @PhotoAck) as totalCount";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                    cmd.Parameters.AddWithValue("@HealthForm", 1);
                    cmd.Parameters.AddWithValue("@PhotoAck", 1);
                    return (int)cmd.ExecuteScalar();
                    }
                }
            }

        private int GetFirstTimeAttendeeCount()
            {
            using (var connection = new SqlConnection(constring))
                {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Participants INNER JOIN Attendance ON AttendingCode = AttendanceID WHERE Returning = @Returning";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                    cmd.Parameters.AddWithValue("@Returning", 0);
                    return (int)cmd.ExecuteScalar();
                    }
                }
            }

        private int GetRepeatAttendeeCount()
            {
            using (var connection = new SqlConnection(constring))
                {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Participants INNER JOIN Attendance ON AttendingCode = AttendanceID WHERE Returning = @Returning";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                    cmd.Parameters.AddWithValue("@Returning", 1);
                    return (int)cmd.ExecuteScalar();
                    }
                }
            }

        private int GetTeeShirtOrdersCount()
            {
            using (var connection = new SqlConnection(constring))
                {
                connection.Open();
                string query = "SELECT COUNT(*) FROM ( SELECT LeadContactFirstName, LeadContactLastName, LeadContactShirtOrder, LeadContactShirtSize FROM LeadContacts WHERE LeadContactShirtOrder = @LeadContactShirtOrder UNION SELECT VolunteerFirstName, VolunteerLastName, VolunteerShirtOrder, VolunteerShirtSize FROM Volunteers WHERE VolunteerShirtOrder = @VolunteerShirtOrder) AS totalCount";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                    cmd.Parameters.AddWithValue("@LeadContactShirtOrder", 1);
                    cmd.Parameters.AddWithValue("@VolunteerShirtOrder", 1);
                    return (int)cmd.ExecuteScalar();
                    }
                }
            }

        private int GetBoothCount()
            {
            using (var connection = new SqlConnection(constring))
                {
                connection.Open();
                string query = "SELECT COUNT(Booth) FROM LeadContacts WHERE Booth IS NOT NULL;";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                    return (int)cmd.ExecuteScalar();
                    }
                }
            }

        private int GetWristBandCount()
            {
            using (var connection = new SqlConnection(constring))
                {
                connection.Open();
                string query = "SELECT COUNT(*) FROM (SELECT LeadContactFirstName, LeadContactLastName FROM LeadContacts UNION SELECT VolunteerFirstName, VolunteerLastName FROM Volunteers) AS totalCount";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                    return (int)cmd.ExecuteScalar();
                    }
                }
            }

        private int GetPeopleCheckedInCount()
            {
            using (var connection = new SqlConnection(constring))
                {
                connection.Open();
                string query = "SELECT COUNT(*) from (SELECT ParticipantFirstName, ParticipantLastName, CheckedIn FROM Participants WHERE CheckedIn = @CheckedIn UNION SELECT GuardianFirstName, GuardianLastName, CheckedIn FROM Guardians WHERE CheckedIn = @CheckedIn UNION SELECT FamilyMemberFirstName, FamilyMemberLastName, CheckedIn FROM FamilyMembers WHERE CheckedIn = @CheckedIn UNION SELECT LeadContactFirstName, LeadContactLastName, CheckedIn FROM LeadContacts WHERE CheckedIn = @CheckedIn UNION SELECT VolunteerFirstName, VolunteerLastName, CheckedIn FROM Volunteers WHERE CheckedIn = @CheckedIn) as totalCount";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                    cmd.Parameters.AddWithValue("@CheckedIn", 1);
                    return (int)cmd.ExecuteScalar();
                    }
                }
            }

        private int GetPendingCheckedInCount()
            {
            using (var connection = new SqlConnection(constring))
                {
                connection.Open();
                string query = "SELECT COUNT(*) from (SELECT ParticipantFirstName, ParticipantLastName, CheckedIn FROM Participants WHERE CheckedIn = @CheckedIn UNION SELECT GuardianFirstName, GuardianLastName, CheckedIn FROM Guardians WHERE CheckedIn = @CheckedIn UNION SELECT FamilyMemberFirstName, FamilyMemberLastName, CheckedIn FROM FamilyMembers WHERE CheckedIn = @CheckedIn UNION SELECT LeadContactFirstName, LeadContactLastName, CheckedIn FROM LeadContacts WHERE CheckedIn = @CheckedIn UNION SELECT VolunteerFirstName, VolunteerLastName, CheckedIn FROM Volunteers WHERE CheckedIn = @CheckedIn) as totalCount";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                    cmd.Parameters.AddWithValue("@CheckedIn", 0);
                    return (int)cmd.ExecuteScalar();
                    }
                }
            }

        private int GetAldenRoadCount()
            {
            using (var connection = new SqlConnection(constring))
                {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Participants INNER JOIN Age ON ParticipantAge = AgeID WHERE ParticipantSchool LIKE @ParticipantSchool";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                    cmd.Parameters.AddWithValue("@ParticipantSchool", "%Alden%");
                    return (int)cmd.ExecuteScalar();
                    }
                }
            }
        }
    }