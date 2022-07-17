using ClinicAPI;
using ClinicAPI.Controllers;
using ClinicAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Clinic.Tests
{
    [TestClass()]
    public class PatientTests
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
        public void GetPatient_IsPatient()
        {
            var controller = new PatientController(Database);
            List<Patient> patient = new List<Patient>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetPatient();
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(patient.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void GetPatient_DatabaseError()
        {
            var controller = new PatientController(DummyDatabase);
            List<Patient> patient = new List<Patient>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetPatient();
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void GetPatientID_IsPatient()
        {
            var controller = new PatientController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetPatient(1);
            Database.Connection.Dispose();
            Patient patient = new Patient();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(patient.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void GetPatientID_DatabaseError()
        {
            var controller = new PatientController(DummyDatabase);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetPatient(1);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void GetPatientID_Birthdate_NotNull()
        {
            var controller = new PatientController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetPatient(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Patient;
            Assert.AreNotEqual(null, response3.Birthdate);
        }
        [TestMethod]
        public void GetPatientID_Id_NotNull()
        {
            var controller = new PatientController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetPatient(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Patient;
            Assert.IsTrue(response3.Id > -1);
        }
        [TestMethod]
        public void GetPatientID_Surname_NotNull()
        {
            var controller = new PatientController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetPatient(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Patient;
            Assert.AreNotEqual(null, response3.Surname);
        }
        [TestMethod]
        public void GetPatientID_Information_NotNull()
        {
            var controller = new PatientController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetPatient(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Patient;
            Assert.AreNotEqual(null, response3.Information);
        }
        [TestMethod]
        public void GetPatientID_Phone_NotNull()
        {
            var controller = new PatientController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetPatient(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Patient;
            Assert.AreNotEqual(null, response3.Phone);
        }
        [TestMethod]
        public void GetPatientID_Email_NotNull()
        {
            var controller = new PatientController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetPatient(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Patient;
            Assert.AreNotEqual(null, response3.Email);
        }
        [TestMethod]
        public void GetPatientID_User_NotNull()
        {
            var controller = new PatientController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetPatient(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Patient;
            Assert.IsTrue(response3.Fk_user > -1);
        }
        [TestMethod]
        public void SearchPatient_IsPatient()
        {
            var controller = new PatientController(Database);
            List<Patient> patient = new List<Patient>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PatientSearch("a");
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(patient.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void SearchPatient_DatabaseError()
        {
            var controller = new PatientController(DummyDatabase);
            List<Patient> patient = new List<Patient>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PatientSearch("a");
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void PostPatient_Response_OK()
        {
            var controller = new PatientController(Database);
            Patient patient = new Patient();
            patient.Name = "Name";
            patient.Surname = "Surname";
            patient.Birthdate = "1997-10-21";
            patient.Phone = "867288611";
            patient.Email = "lukedukeforwow@gmail.com";
            patient.Information = "46 Cavity";
            patient.Debt = 50;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostPatient(patient);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void PostPatient_Response_400()
        {
            var controller = new PatientController(Database);
            Patient patient = new Patient();
            patient.Name = "Name";
            patient.Surname = "Surname";
            patient.Birthdate = "1997-10-21";
            patient.Phone = "867288611";
            patient.Information = "46 Cavity";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostPatient(patient);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostPatient_DatabaseError()
        {
            var controller = new PatientController(DummyDatabase);
            Patient patient = new Patient();
            patient.Name = "Name";
            patient.Surname = "Surname";
            patient.Birthdate = "1997-10-21";
            patient.Phone = "867288611";
            patient.Email = "lukedukeforwow@gmail.com";
            patient.Information = "46 Cavity";
            patient.Fk_user = 5;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostPatient(patient);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void PostPatient_Name_NotNull()
        {
            var controller = new PatientController(Database);
            Patient patient = new Patient();
            patient.Surname = "Surname";
            patient.Birthdate = "1997-10-21";
            patient.Phone = "867288611";
            patient.Email = "lukedukeforwow@gmail.com";
            patient.Information = "46 Cavity";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostPatient(patient);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostPatient_Surname_NotNull()
        {
            var controller = new PatientController(Database);
            Patient patient = new Patient();
            patient.Name = "Name";
            patient.Birthdate = "1997-10-21";
            patient.Phone = "867288611";
            patient.Email = "lukedukeforwow@gmail.com";
            patient.Information = "46 Cavity";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostPatient(patient);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostPatient_Birthdate_NotNull()
        {
            var controller = new PatientController(Database);
            Patient patient = new Patient();
            patient.Name = "Name";
            patient.Surname = "Surname";
            patient.Phone = "867288611";
            patient.Email = "lukedukeforwow@gmail.com";
            patient.Information = "46 Cavity";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostPatient(patient);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostPatient_Phone_NotNull()
        {
            var controller = new PatientController(Database);
            Patient patient = new Patient();
            patient.Name = "Name";
            patient.Surname = "Surname";
            patient.Birthdate = "1997-10-21";
            patient.Email = "lukedukeforwow@gmail.com";
            patient.Information = "46 Cavity";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostPatient(patient);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostPatient_Email_NotNull()
        {
            var controller = new PatientController(Database);
            Patient patient = new Patient();
            patient.Name = "Name";
            patient.Surname = "Surname";
            patient.Birthdate = "1997-10-21";
            patient.Phone = "867288611";
            patient.Information = "46 Cavity";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostPatient(patient);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostPatient_Information_NotNull()
        {
            var controller = new PatientController(Database);
            Patient patient = new Patient();
            patient.Name = "Name";
            patient.Surname = "Surname";
            patient.Birthdate = "1997-10-21";
            patient.Phone = "867288611";
            patient.Email = "lukedukeforwow@gmail.com";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostPatient(patient);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostPatient_NotNull()
        {
            var controller = new PatientController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostPatient(null);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutPatient_Response_OK()
        {
            var controller = new PatientController(Database);
            Patient patient = new Patient();
            patient.Name = "Name";
            patient.Surname = "Surname";
            patient.Birthdate = "1997-10-21";
            patient.Phone = "867288611";
            patient.Email = "lukedukeforwow@gmail.com";
            patient.Information = "46 Cavity";
            patient.Fk_user = 5;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutPatient(5, patient);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void PutPatient_Response_400()
        {
            var controller = new PatientController(Database);
            Patient patient = new Patient();
            patient.Name = "Name";
            patient.Surname = "Surname";
            patient.Birthdate = "1997-10-21";
            patient.Phone = "867288611";
            patient.Information = "46 Cavity";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutPatient(5, patient);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutPatient_DatabaseError()
        {
            var controller = new PatientController(DummyDatabase);
            Patient patient = new Patient();
            patient.Name = "Name";
            patient.Surname = "Surname";
            patient.Birthdate = "1997-10-21";
            patient.Phone = "867288611";
            patient.Email = "lukedukeforwow@gmail.com";
            patient.Information = "46 Cavity";
            patient.Fk_user = 5;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutPatient(5, patient);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void PutPatient_ID_Error()
        {
            var controller = new PatientController(DummyDatabase);
            Patient patient = new Patient();
            patient.Name = "Name";
            patient.Surname = "Surname";
            patient.Birthdate = "1997-10-21";
            patient.Phone = "867288611";
            patient.Email = "lukedukeforwow@gmail.com";
            patient.Information = "46 Cavity";
            patient.Fk_user = 5;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutPatient(970131, patient);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void PutPatient_Name_NotNull()
        {
            var controller = new PatientController(Database);
            Patient patient = new Patient();
            patient.Surname = "Surname";
            patient.Birthdate = "1997-10-21";
            patient.Phone = "867288611";
            patient.Email = "lukedukeforwow@gmail.com";
            patient.Information = "46 Cavity";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutPatient(5, patient);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutPatient_Surname_NotNull()
        {
            var controller = new PatientController(Database);
            Patient patient = new Patient();
            patient.Name = "Name";
            patient.Birthdate = "1997-10-21";
            patient.Phone = "867288611";
            patient.Email = "lukedukeforwow@gmail.com";
            patient.Information = "46 Cavity";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutPatient(5, patient);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutPatient_Birthdate_NotNull()
        {
            var controller = new PatientController(Database);
            Patient patient = new Patient();
            patient.Name = "Name";
            patient.Surname = "Surname";
            patient.Phone = "867288611";
            patient.Email = "lukedukeforwow@gmail.com";
            patient.Information = "46 Cavity";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutPatient(5, patient);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutPatient_Phone_NotNull()
        {
            var controller = new PatientController(Database);
            Patient patient = new Patient();
            patient.Name = "Name";
            patient.Surname = "Surname";
            patient.Birthdate = "1997-10-21";
            patient.Email = "lukedukeforwow@gmail.com";
            patient.Information = "46 Cavity";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutPatient(5, patient);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutPatient_Email_NotNull()
        {
            var controller = new PatientController(Database);
            Patient patient = new Patient();
            patient.Name = "Name";
            patient.Surname = "Surname";
            patient.Birthdate = "1997-10-21";
            patient.Phone = "867288611";
            patient.Information = "46 Cavity";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutPatient(5, patient);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutPatient_Information_NotNull()
        {
            var controller = new PatientController(Database);
            Patient patient = new Patient();
            patient.Name = "Name";
            patient.Surname = "Surname";
            patient.Birthdate = "1997-10-21";
            patient.Phone = "867288611";
            patient.Email = "lukedukeforwow@gmail.com";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutPatient(5, patient);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutPatient_NotNull()
        {
            var controller = new PatientController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutPatient(5, null);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void zDeletePatient_Response_200()
        {
            var controller = new PatientController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.DeletePatient(5);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void DeletePatient_Response_404()
        {
            var controller = new PatientController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.DeletePatient(970131);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.NotFoundObjectResult;
            Assert.AreEqual(404, response2.StatusCode);
        }
        [TestMethod]
        public void DeletePatient_DatabaseError()
        {
            var controller = new PatientController(DummyDatabase);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.DeletePatient(5);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void EraseDebt_Response_200()
        {
            var controller = new PatientController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.EraseDebt(4);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void EraseDebt_DatabaseError()
        {
            var controller = new PatientController(DummyDatabase);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.EraseDebt(5);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void RequestPayment_Response_200()
        {
            var controller = new PatientController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.RequestPayment(3);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void RequestPayment_DatabaseError()
        {
            var controller = new PatientController(DummyDatabase);
            List<Patient> patient = new List<Patient>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.RequestPayment(5);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void RequestPayment_ID_Error()
        {
            var controller = new PatientController(DummyDatabase);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.RequestPayment(970131);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void GetPatientByUser_IsPatient()
        {
            var controller = new PatientController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetPatientByUser(3);
            Database.Connection.Dispose();
            Patient patient = new Patient();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(patient.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void GetPatientByUser_DatabaseError()
        {
            var controller = new PatientController(DummyDatabase);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetPatientByUser(3);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void GetPatientByUser_Birthdate_NotNull()
        {
            var controller = new PatientController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetPatientByUser(3);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Patient;
            Assert.AreNotEqual(null, response3.Birthdate);
        }
        [TestMethod]
        public void GetPatientByUser_Id_NotNull()
        {
            var controller = new PatientController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetPatientByUser(3);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Patient;
            Assert.IsTrue(response3.Id > -1);
        }
        [TestMethod]
        public void GetPatientByUser_Surname_NotNull()
        {
            var controller = new PatientController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetPatientByUser(3);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Patient;
            Assert.AreNotEqual(null, response3.Surname);
        }
        [TestMethod]
        public void GetPatientByUser_Information_NotNull()
        {
            var controller = new PatientController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetPatientByUser(3);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Patient;
            Assert.AreNotEqual(null, response3.Information);
        }
        [TestMethod]
        public void GetPatientByUser_Phone_NotNull()
        {
            var controller = new PatientController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetPatientByUser(3);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Patient;
            Assert.AreNotEqual(null, response3.Phone);
        }
        [TestMethod]
        public void GetPatientByUser_Email_NotNull()
        {
            var controller = new PatientController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetPatientByUser(3);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Patient;
            Assert.AreNotEqual(null, response3.Email);
        }
        [TestMethod]
        public void GetPatientByUser_User_NotNull()
        {
            var controller = new PatientController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetPatientByUser(3);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Patient;
            Assert.IsTrue(response3.Fk_user > -1);
        }
    }
}