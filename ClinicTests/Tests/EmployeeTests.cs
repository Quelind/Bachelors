using ClinicAPI;
using ClinicAPI.Controllers;
using ClinicAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Clinic.Tests
{
    [TestClass()]
    public class EmployeeTests
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
        public void GetEmployee_IsEmployee()
        {
            var controller = new EmployeeController(Database);
            List<Employee> employee = new List<Employee>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetEmployee();
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(employee.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void GetEmployee_DatabaseError()
        {
            var controller = new EmployeeController(DummyDatabase);
            List<Employee> employee = new List<Employee>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetEmployee();
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void GetEmployeeID_IsEmployee()
        {
            var controller = new EmployeeController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetEmployee(1);
            Database.Connection.Dispose();
            Employee employee = new Employee();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(employee.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void GetEmployeeID_DatabaseError()
        {
            var controller = new EmployeeController(DummyDatabase);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetEmployee(1);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void GetEmployeeID_Birthdate_NotNull()
        {
            var controller = new EmployeeController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetEmployee(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Employee;
            Assert.AreNotEqual(null, response3.Birthdate);
        }
        [TestMethod]
        public void GetEmployeeID_Id_NotNull()
        {
            var controller = new EmployeeController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetEmployee(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Employee;
            Assert.IsTrue(response3.Id > -1);
        }
        [TestMethod]
        public void GetEmployeeID_Surname_NotNull()
        {
            var controller = new EmployeeController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetEmployee(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Employee;
            Assert.AreNotEqual(null, response3.Surname);
        }
        [TestMethod]
        public void GetEmployeeID_Specialization_NotNull()
        {
            var controller = new EmployeeController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetEmployee(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Employee;
            Assert.AreNotEqual(null, response3.Specialization);
        }
        [TestMethod]
        public void GetEmployeeID_Phone_NotNull()
        {
            var controller = new EmployeeController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetEmployee(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Employee;
            Assert.AreNotEqual(null, response3.Phone);
        }
        [TestMethod]
        public void GetEmployeeID_Email_NotNull()
        {
            var controller = new EmployeeController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetEmployee(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Employee;
            Assert.AreNotEqual(null, response3.Email);
        }
        [TestMethod]
        public void GetEmployeeID_User_NotNull()
        {
            var controller = new EmployeeController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetEmployee(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Employee;
            Assert.IsTrue(response3.Fk_user > -1);
        }
        [TestMethod]
        public void SearchEmployee_IsEmployee()
        {
            var controller = new EmployeeController(Database);
            List<Employee> employee = new List<Employee>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.EmployeeSearch("a");
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(employee.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void SearchEmployee_DatabaseError()
        {
            var controller = new EmployeeController(DummyDatabase);
            List<Employee> employee = new List<Employee>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.EmployeeSearch("a");
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void PostEmployee_Response_OK()
        {
            var controller = new EmployeeController(Database);
            Employee employee = new Employee();
            employee.Name = "Name";
            employee.Surname = "Surname";
            employee.Birthdate = "1997-10-21";
            employee.Phone = "867288611";
            employee.Email = "lukedukeforwow@gmail.com";
            employee.Specialization = "Odontologist";
            employee.Fk_room = "311A";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostEmployee(employee);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void PostEmployee_Response_400()
        {
            var controller = new EmployeeController(Database);
            Employee employee = new Employee();
            employee.Name = "Name";
            employee.Surname = "Surname";
            employee.Birthdate = "1997-10-21";
            employee.Phone = "867288611";
            employee.Specialization = "Odontologist";
            employee.Fk_room = "311A";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostEmployee(employee);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostEmployee_DatabaseError()
        {
            var controller = new EmployeeController(DummyDatabase);
            Employee employee = new Employee();
            employee.Name = "Name";
            employee.Surname = "Surname";
            employee.Birthdate = "1997-10-21";
            employee.Phone = "867288611";
            employee.Email = "lukedukeforwow@gmail.com";
            employee.Specialization = "Odontologist";
            employee.Fk_room = "311A";
            employee.Fk_user = 6;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostEmployee(employee);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void PostEmployee_Name_NotNull()
        {
            var controller = new EmployeeController(Database);
            Employee employee = new Employee();
            employee.Surname = "Surname";
            employee.Birthdate = "1997-10-21";
            employee.Phone = "867288611";
            employee.Email = "lukedukeforwow@gmail.com";
            employee.Specialization = "Odontologist";
            employee.Fk_room = "311A";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostEmployee(employee); 
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostEmployee_Surname_NotNull()
        {
            var controller = new EmployeeController(Database);
            Employee employee = new Employee();
            employee.Name = "Name";
            employee.Birthdate = "1997-10-21";
            employee.Phone = "867288611";
            employee.Email = "lukedukeforwow@gmail.com";
            employee.Specialization = "Odontologist";
            employee.Fk_room = "311A";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostEmployee(employee); 
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostEmployee_Birthdate_NotNull()
        {
            var controller = new EmployeeController(Database);
            Employee employee = new Employee();
            employee.Name = "Name";
            employee.Surname = "Surname";
            employee.Phone = "867288611";
            employee.Email = "lukedukeforwow@gmail.com";
            employee.Specialization = "Odontologist";
            employee.Fk_room = "311A";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostEmployee(employee); 
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostEmployee_Phone_NotNull()
        {
            var controller = new EmployeeController(Database);
            Employee employee = new Employee();
            employee.Name = "Name";
            employee.Surname = "Surname";
            employee.Birthdate = "1997-10-21";
            employee.Email = "lukedukeforwow@gmail.com";
            employee.Specialization = "Odontologist";
            employee.Fk_room = "311A";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostEmployee(employee); 
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostEmployee_Email_NotNull()
        {
            var controller = new EmployeeController(Database);
            Employee employee = new Employee();
            employee.Name = "Name";
            employee.Surname = "Surname";
            employee.Birthdate = "1997-10-21";
            employee.Phone = "867288611";
            employee.Specialization = "Odontologist";
            employee.Fk_room = "311A";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostEmployee(employee); 
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostEmployee_Specialization_NotNull()
        {
            var controller = new EmployeeController(Database);
            Employee employee = new Employee();
            employee.Name = "Name";
            employee.Surname = "Surname";
            employee.Birthdate = "1997-10-21";
            employee.Phone = "867288611";
            employee.Email = "lukedukeforwow@gmail.com";
            employee.Fk_room = "311A";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostEmployee(employee);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostEmployee_Room_NotNull()
        {
            var controller = new EmployeeController(Database);
            Employee employee = new Employee();
            employee.Name = "Name";
            employee.Surname = "Surname";
            employee.Birthdate = "1997-10-21";
            employee.Phone = "867288611";
            employee.Email = "lukedukeforwow@gmail.com";
            employee.Specialization = "Odontologist";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostEmployee(employee); 
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostEmployee_Image_Null()
        {
            var controller = new EmployeeController(Database);
            Employee employee = new Employee();
            employee.Name = "Name";
            employee.Surname = "Surname";
            employee.Phone = "867288611";
            employee.Birthdate = "1997-10-21";
            employee.Email = "lukedukeforwow@gmail.com";
            employee.Specialization = "Odontologist";
            employee.Fk_room = "311A";
            employee.Image = "";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostEmployee(employee);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void PostEmployee_NotNull()
        {
            var controller = new EmployeeController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostEmployee(null);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutEmployee_Response_OK()
        {
            var controller = new EmployeeController(Database);
            Employee employee = new Employee();
            employee.Name = "Name";
            employee.Surname = "Surname";
            employee.Birthdate = "1997-10-21";
            employee.Phone = "867288611";
            employee.Email = "lukedukeforwow@gmail.com";
            employee.Specialization = "Odontologist";
            employee.Fk_room = "311A";
            employee.Fk_user = 6;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutEmployee(5, employee); 
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void PutEmployee_Response_400()
        {
            var controller = new EmployeeController(Database);
            Employee employee = new Employee();
            employee.Name = "Name";
            employee.Surname = "Surname";
            employee.Birthdate = "1997-10-21";
            employee.Phone = "867288611";
            employee.Specialization = "Odontologist";
            employee.Fk_room = "311A";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutEmployee(5, employee); 
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutEmployee_DatabaseError()
        {
            var controller = new EmployeeController(DummyDatabase);
            Employee employee = new Employee();
            employee.Name = "Name";
            employee.Surname = "Surname";
            employee.Birthdate = "1997-10-21";
            employee.Phone = "867288611";
            employee.Email = "lukedukeforwow@gmail.com";
            employee.Specialization = "Odontologist";
            employee.Fk_room = "311A";
            employee.Fk_user = 6;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutEmployee(5, employee);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void PutEmployee_ID_Error()
        {
            var controller = new EmployeeController(DummyDatabase);
            Employee employee = new Employee();
            employee.Name = "Name";
            employee.Surname = "Surname";
            employee.Birthdate = "1997-10-21";
            employee.Phone = "867288611";
            employee.Email = "lukedukeforwow@gmail.com";
            employee.Specialization = "Odontologist";
            employee.Fk_room = "311A";
            employee.Fk_user = 6;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutEmployee(970131, employee);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void PutEmployee_Name_NotNull()
        {
            var controller = new EmployeeController(Database);
            Employee employee = new Employee();
            employee.Surname = "Surname";
            employee.Birthdate = "1997-10-21";
            employee.Phone = "867288611";
            employee.Email = "lukedukeforwow@gmail.com";
            employee.Specialization = "Odontologist";
            employee.Fk_room = "311A";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutEmployee(5, employee); 
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutEmployee_Surname_NotNull()
        {
            var controller = new EmployeeController(Database);
            Employee employee = new Employee();
            employee.Name = "Name";
            employee.Birthdate = "1997-10-21";
            employee.Phone = "867288611";
            employee.Email = "lukedukeforwow@gmail.com";
            employee.Specialization = "Odontologist";
            employee.Fk_room = "311A";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutEmployee(5, employee);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutEmployee_Birthdate_NotNull()
        {
            var controller = new EmployeeController(Database);
            Employee employee = new Employee();
            employee.Name = "Name";
            employee.Surname = "Surname";
            employee.Phone = "867288611";
            employee.Email = "lukedukeforwow@gmail.com";
            employee.Specialization = "Odontologist";
            employee.Fk_room = "311A";
            employee.Image = "Image.jpg";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutEmployee(5, employee);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutEmployee_Image_Null()
        {
            var controller = new EmployeeController(Database);
            Employee employee = new Employee();
            employee.Name = "Name";
            employee.Surname = "Surname";
            employee.Phone = "867288611";
            employee.Birthdate = "1997-10-21";
            employee.Email = "lukedukeforwow@gmail.com";
            employee.Specialization = "Odontologist";
            employee.Fk_room = "311A";
            employee.Image = "";
            employee.Fk_user = 6;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutEmployee(5, employee);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void PutEmployee_Image_NotNull()
        {
            var controller = new EmployeeController(Database);
            Employee employee = new Employee();
            employee.Name = "Name";
            employee.Surname = "Surname";
            employee.Phone = "867288611";
            employee.Birthdate = "1997-10-21";
            employee.Email = "lukedukeforwow@gmail.com";
            employee.Specialization = "Odontologist";
            employee.Fk_room = "311A";
            employee.Image = "Image.jpg";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutEmployee(5, employee);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void PutEmployee_Phone_NotNull()
        {
            var controller = new EmployeeController(Database);
            Employee employee = new Employee();
            employee.Name = "Name";
            employee.Surname = "Surname";
            employee.Birthdate = "1997-10-21";
            employee.Email = "lukedukeforwow@gmail.com";
            employee.Specialization = "Odontologist";
            employee.Fk_room = "311A";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutEmployee(5, employee);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutEmployee_Email_NotNull()
        {
            var controller = new EmployeeController(Database);
            Employee employee = new Employee();
            employee.Name = "Name";
            employee.Surname = "Surname";
            employee.Birthdate = "1997-10-21";
            employee.Phone = "867288611";
            employee.Specialization = "Odontologist";
            employee.Fk_room = "311A";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutEmployee(5, employee);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutEmployee_Specialization_NotNull()
        {
            var controller = new EmployeeController(Database);
            Employee employee = new Employee();
            employee.Name = "Name";
            employee.Surname = "Surname";
            employee.Birthdate = "1997-10-21";
            employee.Phone = "867288611";
            employee.Email = "lukedukeforwow@gmail.com";
            employee.Fk_room = "311A";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutEmployee(5, employee); 
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutEmployee_Room_NotNull()
        {
            var controller = new EmployeeController(Database);
            Employee employee = new Employee();
            employee.Name = "Name";
            employee.Surname = "Surname";
            employee.Birthdate = "1997-10-21";
            employee.Phone = "867288611";
            employee.Email = "lukedukeforwow@gmail.com";
            employee.Specialization = "Odontologist";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutEmployee(5, employee); 
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutEmployee_NotNull()
        {
            var controller = new EmployeeController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutEmployee(5, null);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void zDeleteEmployee_Response_200()
        {
            var controller = new EmployeeController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.DeleteEmployee(5);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void zDeleteEmployee_Responsee_200()
        {
            var controller = new EmployeeController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.DeleteEmployee(6);
            controller.DeleteEmployee(6);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void DeleteEmployee_Response_404()
        {
            var controller = new EmployeeController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.DeleteEmployee(970131); 
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.NotFoundObjectResult;
            Assert.AreEqual(404, response2.StatusCode);
        }
        [TestMethod]
        public void DeleteEmployee_DatabaseError()
        {
            var controller = new EmployeeController(DummyDatabase);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.DeleteEmployee(5);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void GetRoomByEmployee_IsRoom()
        {
            var controller = new EmployeeController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetRoomByEmployee(1); 
            Database.Connection.Dispose();
            Room room= new Room();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(room.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void GetRoomByEmployee_DatabaseError()
        {
            var controller = new EmployeeController(DummyDatabase);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetRoomByEmployee(1);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void GetEmployeesByTimetable_IsEmployee()
        {
            var controller = new EmployeeController(Database);
            List<Employee> employee = new List<Employee>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetEmployeesByTimetable(1); 
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(employee.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void GetEmployeesByTimetable_DatabaseError()
        {
            var controller = new EmployeeController(DummyDatabase);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetEmployeesByTimetable(1);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
         [TestMethod]
        public void GetEmployeeByUser_IsEmployee()
        {
            var controller = new EmployeeController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetEmployeeByUser(6);
            Database.Connection.Dispose();
            Employee employee = new Employee();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(employee.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void GetEmployeeByUser_DatabaseError()
        {
            var controller = new EmployeeController(DummyDatabase);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetEmployeeByUser(6);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void GetEmployeeByUser_Birthdate_NotNull()
        {
            var controller = new EmployeeController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetEmployeeByUser(6);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Employee;
            Assert.AreNotEqual(null, response3.Birthdate);
        }
        [TestMethod]
        public void GetEmployeeByUser_Id_NotNull()
        {
            var controller = new EmployeeController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetEmployeeByUser(6);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Employee;
            Assert.IsTrue(response3.Id > -1);
        }
        [TestMethod]
        public void GetEmployeeByUser_Surname_NotNull()
        {
            var controller = new EmployeeController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetEmployeeByUser(6);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Employee;
            Assert.AreNotEqual(null, response3.Surname);
        }
        [TestMethod]
        public void GetEmployeeByUser_Specialization_NotNull()
        {
            var controller = new EmployeeController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetEmployeeByUser(6);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Employee;
            Assert.AreNotEqual(null, response3.Specialization);
        }
        [TestMethod]
        public void GetEmployeeByUser_Phone_NotNull()
        {
            var controller = new EmployeeController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetEmployeeByUser(6);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Employee;
            Assert.AreNotEqual(null, response3.Phone);
        }
        [TestMethod]
        public void GetEmployeeByUser_Email_NotNull()
        {
            var controller = new EmployeeController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetEmployeeByUser(6);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Employee;
            Assert.AreNotEqual(null, response3.Email);
        }
        [TestMethod]
        public void GetEmployeeByUser_User_NotNull()
        {
            var controller = new EmployeeController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetEmployeeByUser(6);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Employee;
            Assert.IsTrue(response3.Fk_user > -1);
        }
    }
}