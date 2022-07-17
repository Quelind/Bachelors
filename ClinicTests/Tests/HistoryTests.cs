using ClinicAPI;
using ClinicAPI.Controllers;
using ClinicAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Clinic.Tests
{
    [TestClass()]
    public class HistoryTests
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
        public void GetHistory_IsHistory()
        {
            var controller = new HistoryController(Database);
            List<History> history = new List<History>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetHistory();
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(history.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void GetHistory_DatabaseError()
        {
            var controller = new HistoryController(DummyDatabase);
            List<History> history = new List<History>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetHistory();
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void GetHistoryID_IsHistory()
        {
            var controller = new HistoryController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetHistory(1);
            Database.Connection.Dispose();
            List<History> history = new List<History>();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(history.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void GetHistoryID_DatabaseError()
        {
            var controller = new HistoryController(DummyDatabase);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetHistory(1);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void SearchHistory_IsHistory()
        {
            var controller = new HistoryController(Database);
            List<History> history = new List<History>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.HistorySearch("a", 5);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(history.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void SearchHistory_DatabaseError()
        {
            var controller = new HistoryController(DummyDatabase);
            List<History> history = new List<History>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.HistorySearch("a", 5);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
    }
}