using ClinicAPI;
using ClinicAPI.Controllers;
using ClinicAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Clinic.Tests
{
    [TestClass()]
    public class UserTests
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
        public void Register_Response_OK()
        {
            var controller = new UserController(Database);
            User user = new User();
            user.Name = "Name";
            user.Surname = "Surname";
            user.Birthdate = "1997-10-21";
            user.Phone = "867288611";
            user.Email = "lukedukeforwow@gmail.com";
            user.Password = "Password";
            user.Token = "Success";
            user.Reset_token = "Success";
            var rand = new Random();
            user.Username = "Username" + rand.Next(100000, 999999).ToString();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.Register(user);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void Register_Response_400()
        {
            var controller = new UserController(Database);
            User user = new User();
            user.Name = "Name";
            user.Surname = "Surname";
            user.Birthdate = "1997-10-21";
            user.Phone = "867288611";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.Register(user);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void Register_DatabaseError()
        {
            var controller = new UserController(DummyDatabase);
            User user = new User();
            user.Name = "Name";
            user.Surname = "Surname";
            user.Birthdate = "1997-10-21";
            user.Phone = "867288611";
            user.Email = "lukedukeforwow@gmail.com";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.Register(user);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void Register_Name_NotNull()
        {
            var controller = new UserController(Database);
            User user = new User();
            user.Surname = "Surname";
            user.Birthdate = "1997-10-21";
            user.Phone = "867288611";
            user.Email = "lukedukeforwow@gmail.com";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.Register(user);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void Register_Surname_NotNull()
        {
            var controller = new UserController(Database);
            User user = new User();
            user.Name = "Name";
            user.Birthdate = "1997-10-21";
            user.Phone = "867288611";
            user.Email = "lukedukeforwow@gmail.com";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.Register(user);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void Register_Birthdate_NotNull()
        {
            var controller = new UserController(Database);
            User user = new User();
            user.Name = "Name";
            user.Surname = "Surname";
            user.Phone = "867288611";
            user.Email = "lukedukeforwow@gmail.com";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.Register(user);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void Register_Phone_NotNull()
        {
            var controller = new UserController(Database);
            User user = new User();
            user.Name = "Name";
            user.Surname = "Surname";
            user.Birthdate = "1997-10-21";
            user.Email = "lukedukeforwow@gmail.com";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.Register(user);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void Register_Email_NotNull()
        {
            var controller = new UserController(Database);
            User user = new User();
            user.Name = "Name";
            user.Surname = "Surname";
            user.Birthdate = "1997-10-21";
            user.Phone = "867288611";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.Register(user);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void Register_NotNull()
        {
            var controller = new UserController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.Register(null);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void VerifyEmail_Response_OK()
        {
            var controller = new UserController(Database);
            VerifyEmail verifyEmail = new VerifyEmail();
            verifyEmail.Token = "Success";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.VerifyEmail(verifyEmail);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void VerifyEmail_Response_400()
        {
            var controller = new UserController(Database);
            VerifyEmail verifyEmail = new VerifyEmail();
            verifyEmail.Token = "Error";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.VerifyEmail(verifyEmail);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void Authenticate_UsernameWrong()
        {
            var controller = new UserController(Database);
            Authenticate authenticate = new Authenticate();
            authenticate.Username = "xdddddddddd";
            authenticate.Password = "admin";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.Authenticate(authenticate);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void Authenticate_PasswordWrong()
        {
            var controller = new UserController(Database);
            Authenticate authenticate = new Authenticate();
            authenticate.Username = "admin";
            authenticate.Password = "nope";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.Authenticate(authenticate);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void Authenticate_Response_OK()
        {
            var controller = new UserController(Database);
            Authenticate authenticate = new Authenticate();
            authenticate.Username = "admin";
            authenticate.Password = "admin";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.Authenticate(authenticate);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void GetUser_IsEmployee()
        {
            var controller = new UserController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetUser(1);
            Database.Connection.Dispose();
            User employee = new User();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(employee.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void GetUser_DatabaseError()
        {
            var controller = new UserController(DummyDatabase);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetUser(1);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void PatchUser_Response_OK()
        {
            var controller = new UserController(Database);
            User user = new User();
            user.Name = "Name";
            user.Surname = "Surname";
            user.Birthdate = "1997-10-21";
            user.Phone = "867288611";
            user.Email = "lukedukeforwow@gmail.com";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PatchUser(5, user);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void PatchUser_Response_400()
        {
            var controller = new UserController(Database);
            User user = new User();
            user.Name = "Name";
            user.Surname = "Surname";
            user.Birthdate = "1997-10-21";
            user.Phone = "867288611";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PatchUser(5, user);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PatchUser_DatabaseError()
        {
            var controller = new UserController(DummyDatabase);
            User user = new User();
            user.Name = "Name";
            user.Surname = "Surname";
            user.Birthdate = "1997-10-21";
            user.Phone = "867288611";
            user.Email = "lukedukeforwow@gmail.com";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PatchUser(5, user);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void PatchUser_ID_Error()
        {
            var controller = new UserController(DummyDatabase);
            User user = new User();
            user.Name = "Name";
            user.Surname = "Surname";
            user.Birthdate = "1997-10-21";
            user.Phone = "867288611";
            user.Email = "lukedukeforwow@gmail.com";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PatchUser(970131, user);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void PatchUser_Name_NotNull()
        {
            var controller = new UserController(Database);
            User user = new User();
            user.Surname = "Surname";
            user.Birthdate = "1997-10-21";
            user.Phone = "867288611";
            user.Email = "lukedukeforwow@gmail.com";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PatchUser(5, user);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PatchUser_Surname_NotNull()
        {
            var controller = new UserController(Database);
            User user = new User();
            user.Name = "Name";
            user.Birthdate = "1997-10-21";
            user.Phone = "867288611";
            user.Email = "lukedukeforwow@gmail.com";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PatchUser(5, user);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PatchUser_Birthdate_NotNull()
        {
            var controller = new UserController(Database);
            User user = new User();
            user.Name = "Name";
            user.Surname = "Surname";
            user.Phone = "867288611";
            user.Email = "lukedukeforwow@gmail.com";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PatchUser(5, user);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PatchUser_Phone_NotNull()
        {
            var controller = new UserController(Database);
            User user = new User();
            user.Name = "Name";
            user.Surname = "Surname";
            user.Birthdate = "1997-10-21";
            user.Email = "lukedukeforwow@gmail.com";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PatchUser(5, user);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PatchUser_Email_NotNull()
        {
            var controller = new UserController(Database);
            User user = new User();
            user.Name = "Name";
            user.Surname = "Surname";
            user.Birthdate = "1997-10-21";
            user.Phone = "867288611";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PatchUser(5, user);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PatchUser_NotNull()
        {
            var controller = new UserController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PatchUser(5, null);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void UpdatePassword_Response_OK()
        {
            var controller = new UserController(Database);
            ChangePassword changePassword = new ChangePassword();
            changePassword.Username = "user3";
            changePassword.OldPassword = "user";
            changePassword.NewPassword = "user";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.UpdatePassword(changePassword, 5);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void UpdatePassword_Response_400()
        {
            var controller = new UserController(Database);
            ChangePassword changePassword = new ChangePassword();
            changePassword.Username = "user3";
            changePassword.OldPassword = "WrongPassword";
            changePassword.NewPassword = "user";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.UpdatePassword(changePassword, 5);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void UpdatePassword_OldPassword_NotNull()
        {
            var controller = new UserController(Database);
            ChangePassword changePassword = new ChangePassword();
            changePassword.Username = "user3";
            changePassword.NewPassword = "user";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.UpdatePassword(changePassword, 5);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void UpdatePassword_NewPassword_NotNull()
        {
            var controller = new UserController(Database);
            ChangePassword changePassword = new ChangePassword();
            changePassword.Username = "user3";
            changePassword.OldPassword = "user";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.UpdatePassword(changePassword, 5);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void UpdatePassword_NotNull()
        {
            var controller = new UserController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.UpdatePassword(null, 5);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void ForgotPassword_Response_OK()
        {
            var controller = new UserController(Database);
            ForgotPassword forgotPassword = new ForgotPassword();
            forgotPassword.Username = "user";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.ForgotPassword(forgotPassword);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void ForgotPassword_Response_400()
        {
            var controller = new UserController(Database);
            ForgotPassword forgotPassword = new ForgotPassword();
            forgotPassword.Username = "user15";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.ForgotPassword(forgotPassword);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void ForgotPassword_NotNull()
        {
            var controller = new UserController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.ForgotPassword(null);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void VerifyPassword_NotNull()
        {
            var controller = new UserController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.VerifyPassword(null);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void VerifyPassword_Get_Test()
        {
            var controller = new VisitController(DummyDatabase);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.DeleteVisit(5);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
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
    }
}