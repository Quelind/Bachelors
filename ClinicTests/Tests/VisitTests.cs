using ClinicAPI;
using ClinicAPI.Controllers;
using ClinicAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Clinic.Tests
{
    [TestClass()]
    public class VisitTests
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
        public void GetVisit_IsVisit()
        {
            var controller = new VisitController(Database);
            List<Visit> visit = new List<Visit>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetVisit();
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(visit.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void GetVisit_DatabaseError()
        {
            var controller = new VisitController(DummyDatabase);
            List<Visit> visit = new List<Visit>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetVisit();
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void GetVisitID_IsVisit()
        {
            var controller = new VisitController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetVisit(1);
            Database.Connection.Dispose();
            Visit visit = new Visit();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(visit.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void GetVisitID_DatabaseError()
        {
            var controller = new VisitController(DummyDatabase);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetVisit(1);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void GetVisitID_Fk_doctor_NotNull()
        {
            var controller = new VisitController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetVisit(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Visit;
            Assert.AreNotEqual(null, response3.Fk_doctor);
        }
        [TestMethod]
        public void GetVisitID_Id_NotNull()
        {
            var controller = new VisitController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetVisit(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Visit;
            Assert.IsTrue(response3.Id > -1);
        }
        [TestMethod]
        public void GetVisitID_Fk_patient_NotNull()
        {
            var controller = new VisitController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetVisit(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Visit;
            Assert.AreNotEqual(null, response3.Fk_patient);
        }
        [TestMethod]
        public void GetVisitID_Fk_procedure_NotNull()
        {
            var controller = new VisitController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetVisit(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Visit;
            Assert.AreNotEqual(null, response3.Fk_procedure);
        }
        [TestMethod]
        public void GetVisitID_Fk_timetable_NotNull()
        {
            var controller = new VisitController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetVisit(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Visit;
            Assert.AreNotEqual(null, response3.Fk_timetable);
        }
        [TestMethod]
        public void GetVisitID_Fk_room_NotNull()
        {
            var controller = new VisitController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetVisit(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Visit;
            Assert.AreNotEqual(null, response3.Fk_room);
        }
        [TestMethod]
        public void SearchVisit_IsVisit()
        {
            var controller = new VisitController(Database);
            List<Visit> visit = new List<Visit>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.VisitSearch("a");
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(visit.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void SearchVisit_DatabaseError()
        {
            var controller = new VisitController(DummyDatabase);
            List<Visit> visit = new List<Visit>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.VisitSearch("a");
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void PostVisit_Response_OK()
        {
            var controller = new VisitController(Database);
            Visit visit = new Visit();
            visit.Patient_comment = "Patient_comment";
            visit.Fk_patient = 1;
            visit.Fk_doctor = 1;
            visit.Fk_timetable = 1;
            visit.Fk_procedure = 0;
            visit.Fk_room = "311A";
            visit.Patient_history = "History";
            visit.Patient_description = "Description";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostVisit(visit);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void PostVisit_DatabaseError()
        {
            var controller = new VisitController(DummyDatabase);
            Visit visit = new Visit();
            visit.Patient_comment = "Patient_comment";
            visit.Fk_patient = 1;
            visit.Fk_doctor = 1;
            visit.Fk_timetable = 1;
            visit.Fk_procedure = 0;
            visit.Fk_room = "311A";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostVisit(visit);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void PostVisit_Patient_comment_NotNull()
        {
            var controller = new VisitController(Database);
            Visit visit = new Visit();
            visit.Fk_patient = 1;
            visit.Fk_doctor = 1;
            visit.Fk_timetable = 1;
            visit.Fk_procedure = 0;
            visit.Fk_room = "311A";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostVisit(visit);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostVisit_Fk_patient_NotNull()
        {
            var controller = new VisitController(Database);
            Visit visit = new Visit();
            visit.Patient_comment = "Patient_comment";
            visit.Fk_doctor = 1;
            visit.Fk_timetable = 1;
            visit.Fk_procedure = 0;
            visit.Fk_patient = -1;
            visit.Fk_room = "311A";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostVisit(visit);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostVisit_Fk_doctor_NotNull()
        {
            var controller = new VisitController(Database);
            Visit visit = new Visit();
            visit.Patient_comment = "Patient_comment";
            visit.Fk_patient = 1;
            visit.Fk_timetable = 1;
            visit.Fk_procedure = 0;
            visit.Fk_room = "311A";
            visit.Fk_doctor = -1;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostVisit(visit);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostVisit_Fk_timetable_NotNull()
        {
            var controller = new VisitController(Database);
            Visit visit = new Visit();
            visit.Patient_comment = "Patient_comment";
            visit.Fk_patient = 1;
            visit.Fk_doctor = 1;
            visit.Fk_procedure = 0;
            visit.Fk_room = "311A";
            visit.Fk_timetable = -1;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostVisit(visit);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostVisit_Fk_room_NotNull()
        {
            var controller = new VisitController(Database);
            Visit visit = new Visit();
            visit.Patient_comment = "Patient_comment";
            visit.Fk_patient = 1;
            visit.Fk_doctor = 1;
            visit.Fk_timetable = 1;
            visit.Fk_procedure = 0;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostVisit(visit);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostVisit_Fk_procedure_NotNull()
        {
            var controller = new VisitController(Database);
            Visit visit = new Visit();
            visit.Patient_comment = "Patient_comment";
            visit.Fk_patient = 1;
            visit.Fk_doctor = 1;
            visit.Fk_timetable = 1;
            visit.Fk_procedure = -1;
            visit.Fk_room = "311A";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostVisit(visit);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostVisit_Room_NotNull()
        {
            var controller = new VisitController(Database);
            Visit visit = new Visit();
            visit.Patient_comment = "Patient_comment";
            visit.Fk_patient = 1;
            visit.Fk_doctor = 1;
            visit.Fk_timetable = 1;
            visit.Fk_procedure = 0;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostVisit(visit);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostVisit_NotNull()
        {
            var controller = new VisitController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostVisit(null);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutVisit_Response_OK()
        {
            var controller = new VisitController(Database);
            Visit visit = new Visit();
            visit.Patient_comment = "Patient_comment";
            visit.Fk_patient = 1;
            visit.Fk_doctor = 1;
            visit.Fk_timetable = 1;
            visit.Fk_procedure = 0;
            visit.Fk_room = "311A";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutVisit(5, visit);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void PutVisit_DatabaseError()
        {
            var controller = new VisitController(DummyDatabase);
            Visit visit = new Visit();
            visit.Patient_comment = "Patient_comment";
            visit.Fk_patient = 1;
            visit.Fk_doctor = 1;
            visit.Fk_timetable = 1;
            visit.Fk_procedure = 0;
            visit.Fk_room = "311A";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutVisit(5, visit);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void PutVisit_ID_Error()
        {
            var controller = new VisitController(DummyDatabase);
            Visit visit = new Visit();
            visit.Patient_comment = "Patient_comment";
            visit.Fk_patient = 1;
            visit.Fk_doctor = 1;
            visit.Fk_timetable = 1;
            visit.Fk_procedure = 0;
            visit.Fk_room = "311A";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutVisit(970131, visit);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void PutVisit_Patient_comment_NotNull()
        {
            var controller = new VisitController(Database);
            Visit visit = new Visit();
            visit.Fk_patient = 1;
            visit.Fk_doctor = 1;
            visit.Fk_timetable = 1;
            visit.Fk_procedure = 0;
            visit.Fk_room = "311A";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutVisit(5, visit);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutVisit_Fk_patient_NotNull()
        {
            var controller = new VisitController(Database);
            Visit visit = new Visit();
            visit.Patient_comment = "Patient_comment";
            visit.Fk_doctor = 1;
            visit.Fk_timetable = 1;
            visit.Fk_procedure = 0;
            visit.Fk_room = "311A";
            visit.Fk_patient = -1;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutVisit(5, visit);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutVisit_Fk_doctor_NotNull()
        {
            var controller = new VisitController(Database);
            Visit visit = new Visit();
            visit.Patient_comment = "Patient_comment";
            visit.Fk_patient = 1;
            visit.Fk_timetable = 1;
            visit.Fk_procedure = 0;
            visit.Fk_room = "311A";
            visit.Fk_doctor = -1;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutVisit(5, visit);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutVisit_Fk_timetable_NotNull()
        {
            var controller = new VisitController(Database);
            Visit visit = new Visit();
            visit.Patient_comment = "Patient_comment";
            visit.Fk_patient = 1;
            visit.Fk_doctor = 1;
            visit.Fk_procedure = 0;
            visit.Fk_room = "311A";
            visit.Fk_timetable = -1;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutVisit(5, visit);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutVisit_Fk_room_NotNull()
        {
            var controller = new VisitController(Database);
            Visit visit = new Visit();
            visit.Patient_comment = "Patient_comment";
            visit.Fk_patient = 1;
            visit.Fk_doctor = 1;
            visit.Fk_timetable = 1;
            visit.Fk_procedure = 0;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutVisit(5, visit);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutVisit_Fk_procedure_NotNull()
        {
            var controller = new VisitController(Database);
            Visit visit = new Visit();
            visit.Patient_comment = "Patient_comment";
            visit.Fk_patient = 1;
            visit.Fk_doctor = 1;
            visit.Fk_timetable = 1;
            visit.Fk_room = "311A";
            visit.Fk_procedure = -1;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutVisit(5, visit);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutVisit_Room_NotNull()
        {
            var controller = new VisitController(Database);
            Visit visit = new Visit();
            visit.Patient_comment = "Patient_comment";
            visit.Fk_patient = 1;
            visit.Fk_doctor = 1;
            visit.Fk_timetable = 1;
            visit.Fk_procedure = 0;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutVisit(5, visit);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutVisit_NotNull()
        {
            var controller = new VisitController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutVisit(5, null);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void ConfirmVisit_DatabaseError()
        {
            var controller = new VisitController(DummyDatabase);

            Microsoft.AspNetCore.Mvc.IActionResult response = controller.ConfirmVisit(5);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void ConfirmVisit_ID_Error()
        {
            var controller = new VisitController(DummyDatabase);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.ConfirmVisit(970131);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void ConfirmVisit_Response_200()
        {
            var controller = new VisitController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.ConfirmVisit(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void ConfirmVisit_Response_404()
        {
            var controller = new VisitController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.ConfirmVisit(970131);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.NotFoundObjectResult;
            Assert.AreEqual(404, response2.StatusCode);
        }
        [TestMethod]
        public void GetActiveVisits_IsVisit()
        {
            var controller = new VisitController(Database);
            List<Visit> visit = new List<Visit>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetActiveVisits();
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(visit.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void GetActiveVisitsSearch_IsVisit()
        {
            var controller = new VisitController(Database);
            List<Visit> visit = new List<Visit>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetActiveVisitsSearch("a");
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(visit.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void GetActiveVisitsSearch_DatabaseError()
        {
            var controller = new VisitController(DummyDatabase);
            List<Visit> visit = new List<Visit>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetActiveVisitsSearch("a");
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void ChangeDoctor_NotNull()
        {
            var controller = new VisitController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.ChangeDoctor(5, null);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void ChangeDoctor_DoctorNotNull()
        {
            var controller = new VisitController(Database);
            Visit visit = new Visit();
            visit.Fk_doctor = -1;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.ChangeDoctor(5, visit);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void ChangeDoctor_DatabaseError()
        {
            var controller = new VisitController(DummyDatabase);
            Visit visit = new Visit();
            visit.Fk_doctor = 1;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.ChangeDoctor(5, visit);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void ChangeDoctor_ID_Error()
        {
            var controller = new VisitController(DummyDatabase);
            Visit visit = new Visit();
            visit.Fk_doctor = 1;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.ChangeDoctor(970131, visit);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void ChangeDoctor_Response_OK()
        {
            var controller = new VisitController(Database);
            Visit visit = new Visit();
            visit.Patient_comment = "I feel a severe pain in my root canal";
            visit.Fk_patient = 2;
            visit.Fk_doctor = 1;
            visit.Fk_timetable = 19;
            visit.Fk_procedure = 0;
            visit.Fk_room = "313B";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.ChangeDoctor(2, visit);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void zDeleteVisit_Response_200()
        {
            var controller = new VisitController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.DeleteVisit(5);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void DeleteVisit_Response_404()
        {
            var controller = new VisitController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.DeleteVisit(970131);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.NotFoundObjectResult;
            Assert.AreEqual(404, response2.StatusCode);
        }
        [TestMethod]
        public void DeleteVisit_DatabaseError()
        {
            var controller = new VisitController(DummyDatabase);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.DeleteVisit(5);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
    }
}