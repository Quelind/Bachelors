using ClinicAPI;
using ClinicAPI.Controllers;
using ClinicAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Clinic.Tests
{
    [TestClass()]
    public class ScheduleTests
    {
        [TestMethod]
        public void GetDays_IsSchedule()
        {
            var controller = new ScheduleController();
            List<Schedule> schedule = new List<Schedule>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetDays();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(schedule.GetType(), response2.Value.GetType());
        }
    }
}