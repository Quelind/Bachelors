using ClinicAPI;
using ClinicAPI.Controllers;
using ClinicAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Clinic.Tests
{
    [TestClass()]
    public class TimetableTests
    {
        private static AppDatabase Database { get; set; }
        private static AppDatabase DummyDatabase { get; set; }
        [ClassInitialize]
        public static void SetUp(TestContext context)
        {
            Database = new AppDatabase("Data Source=localhost;port=3306;Initial Catalog=clinic; User Id=root;password=;SslMode=none;convert zero datetime=True");
            DummyDatabase = new AppDatabase("Data Source=localhost;port=3306;Initial Catalog=FAKE; User Id=root;password=;SslMode=none;convert zero datetime=True");
        }
        [TestMethod]
        public void GetTimetable_IsTimetable()
        {
            var controller = new TimetableController(Database);
            List<Timetable> timetable = new List<Timetable>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetTimetable();
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(timetable.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void GetTimetable_DatabaseError()
        {
            var controller = new TimetableController(DummyDatabase);
            List<Timetable> timetable = new List<Timetable>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetTimetable();
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void GetTimetableID_IsTimetable()
        {
            var controller = new TimetableController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetTimetable(1);
            Database.Connection.Dispose();
            Timetable timetable = new Timetable();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(timetable.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void GetTimetableID_DatabaseError()
        {
            var controller = new TimetableController(DummyDatabase);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetTimetable(1);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void GetTimetableUnlocked_IsTimetable()
        {
            var controller = new TimetableController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetTimetableUnlocked("1");
            Database.Connection.Dispose();
            List<Timetable> timetable = new List<Timetable>();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(timetable.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void GetTimetableUnlocked_DatabaseError()
        {
            var controller = new TimetableController(DummyDatabase);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetTimetableUnlocked("1");
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
    }
}