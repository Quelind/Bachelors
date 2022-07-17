using ClinicAPI;
using ClinicAPI.Controllers;
using ClinicAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Clinic.Tests
{
    [TestClass()]
    public class ProcedureTests
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
        public void GetProcedure_IsProcedure()
        {
            var controller = new ProcedureController(Database);
            List<Procedure> procedure = new List<Procedure>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetProcedure();
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(procedure.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void GetProcedure_DatabaseError()
        {
            var controller = new ProcedureController(DummyDatabase);
            List<Procedure> procedure = new List<Procedure>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetProcedure();
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void GetProcedureID_IsProcedure()
        {
            var controller = new ProcedureController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetProcedure(1);
            Database.Connection.Dispose();
            Procedure procedure = new Procedure();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(procedure.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void GetProcedureID_DatabaseError()
        {
            var controller = new ProcedureController(DummyDatabase);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetProcedure(1);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void GetProcedureID_Room_type_NotNull()
        {
            var controller = new ProcedureController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetProcedure(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Procedure;
            Assert.AreNotEqual(null, response3.Room_type);
        }
        [TestMethod]
        public void GetProcedureID_Id_NotNull()
        {
            var controller = new ProcedureController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetProcedure(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Procedure;
            Assert.IsTrue(response3.Id > -1);
        }
        [TestMethod]
        public void GetProcedureID_Requirement_NotNull()
        {
            var controller = new ProcedureController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetProcedure(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Procedure;
            Assert.AreNotEqual(null, response3.Requirement);
        }
        [TestMethod]
        public void GetProcedureID_Personnel_count_NotNull()
        {
            var controller = new ProcedureController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetProcedure(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Procedure;
            Assert.AreNotEqual(null, response3.Personnel_count);
        }
        [TestMethod]
        public void GetProcedureID_Information_NotNull()
        {
            var controller = new ProcedureController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetProcedure(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Procedure;
            Assert.AreNotEqual(null, response3.Information);
        }
        [TestMethod]
        public void GetProcedureID_Duration_NotNull()
        {
            var controller = new ProcedureController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetProcedure(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Procedure;
            Assert.AreNotEqual(null, response3.Duration);
        }
        [TestMethod]
        public void SearchProcedure_IsProcedure()
        {
            var controller = new ProcedureController(Database);
            List<Procedure> procedure = new List<Procedure>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.ProcedureSearch("a");
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(procedure.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void SearchProcedure_DatabaseError()
        {
            var controller = new ProcedureController(DummyDatabase);
            List<Procedure> procedure = new List<Procedure>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.ProcedureSearch("a");
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void PostProcedure_Response_OK()
        {
            var controller = new ProcedureController(Database);
            Procedure procedure = new Procedure();
            procedure.Name = "Name";
            procedure.Requirement = "Requirement";
            procedure.Room_type = "Procedural";
            procedure.Information = "No eating for 30 mins after the procedure";
            procedure.Duration = 60;
            procedure.Personnel_count = 2;
            procedure.Price = 50;
            procedure.Image = "Img.jpg";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostProcedure(procedure);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void PostProcedure_Response_400()
        {
            var controller = new ProcedureController(Database);
            Procedure procedure = new Procedure();
            procedure.Name = "Name";
            procedure.Requirement = "Requirement";
            procedure.Room_type = "Procedural";
            procedure.Information = "No eating for 30 mins after the procedure";
            procedure.Personnel_count = 2;
            procedure.Price = 50;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostProcedure(procedure);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostProcedure_DatabaseError()
        {
            var controller = new ProcedureController(DummyDatabase);
            Procedure procedure = new Procedure();
            procedure.Name = "Name";
            procedure.Requirement = "Requirement";
            procedure.Room_type = "Procedural";
            procedure.Information = "No eating for 30 mins after the procedure";
            procedure.Duration = 60;
            procedure.Personnel_count = 2;
            procedure.Price = 50;

            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostProcedure(procedure);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void PostProcedure_Name_NotNull()
        {
            var controller = new ProcedureController(Database);
            Procedure procedure = new Procedure();
            procedure.Requirement = "Requirement";
            procedure.Room_type = "Procedural";
            procedure.Information = "No eating for 30 mins after the procedure";
            procedure.Duration = 60;
            procedure.Personnel_count = 2;
            procedure.Price = 50;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostProcedure(procedure);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostProcedure_Requirement_NotNull()
        {
            var controller = new ProcedureController(Database);
            Procedure procedure = new Procedure();
            procedure.Name = "Name";
            procedure.Room_type = "Procedural";
            procedure.Information = "No eating for 30 mins after the procedure";
            procedure.Duration = 60;
            procedure.Personnel_count = 2;
            procedure.Price = 50;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostProcedure(procedure);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostProcedure_Room_type_NotNull()
        {
            var controller = new ProcedureController(Database);
            Procedure procedure = new Procedure();
            procedure.Name = "Name";
            procedure.Requirement = "Requirement";
            procedure.Information = "No eating for 30 mins after the procedure";
            procedure.Duration = 60;
            procedure.Personnel_count = 2;
            procedure.Price = 50;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostProcedure(procedure);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostProcedure_Information_NotNull()
        {
            var controller = new ProcedureController(Database);
            Procedure procedure = new Procedure();
            procedure.Name = "Name";
            procedure.Requirement = "Requirement";
            procedure.Room_type = "Procedural";
            procedure.Duration = 60;
            procedure.Personnel_count = 2;
            procedure.Price = 50;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostProcedure(procedure);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostProcedure_Duration_NotNull()
        {
            var controller = new ProcedureController(Database);
            Procedure procedure = new Procedure();
            procedure.Name = "Name";
            procedure.Requirement = "Requirement";
            procedure.Room_type = "Procedural";
            procedure.Information = "No eating for 30 mins after the procedure";
            procedure.Personnel_count = 2;
            procedure.Price = 50;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostProcedure(procedure);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostProcedure_Price_NotNull()
        {
            var controller = new ProcedureController(Database);
            Procedure procedure = new Procedure();
            procedure.Name = "Name";
            procedure.Requirement = "Requirement";
            procedure.Room_type = "Procedural";
            procedure.Information = "No eating for 30 mins after the procedure";
            procedure.Duration = 60;
            procedure.Personnel_count = 2;
            procedure.Image = "Img.jpg";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostProcedure(procedure);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostProcedure_Image_Null()
        {
            var controller = new ProcedureController(Database);
            Procedure procedure = new Procedure();
            procedure.Name = "Name";
            procedure.Requirement = "Requirement";
            procedure.Information = "No eating for 30 mins after the procedure";
            procedure.Room_type = "Procedural";
            procedure.Duration = 60;
            procedure.Personnel_count = 2;
            procedure.Price = 50;
            procedure.Image = "";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostProcedure(procedure);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void PostProcedure_NotNull()
        {
            var controller = new ProcedureController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostProcedure(null);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutProcedure_Response_OK()
        {
            var controller = new ProcedureController(Database);
            Procedure procedure = new Procedure();
            procedure.Name = "Name";
            procedure.Requirement = "Requirement";
            procedure.Room_type = "Procedural";
            procedure.Information = "No eating for 30 mins after the procedure";
            procedure.Duration = 60;
            procedure.Personnel_count = 2;
            procedure.Price = 50;
            procedure.Image = "Img.jpg";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutProcedure(3, procedure);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void PutProcedure_Response_400()
        {
            var controller = new ProcedureController(Database);
            Procedure procedure = new Procedure();
            procedure.Name = "Name";
            procedure.Requirement = "Requirement";
            procedure.Room_type = "Procedural";
            procedure.Information = "No eating for 30 mins after the procedure";
            procedure.Personnel_count = 2;
            procedure.Price = 50;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutProcedure(3, procedure);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutProcedure_DatabaseError()
        {
            var controller = new ProcedureController(DummyDatabase);
            Procedure procedure = new Procedure();
            procedure.Name = "Name";
            procedure.Requirement = "Requirement";
            procedure.Room_type = "Procedural";
            procedure.Information = "No eating for 30 mins after the procedure";
            procedure.Duration = 60;
            procedure.Personnel_count = 2;
            procedure.Price = 50;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutProcedure(3, procedure);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void PutProcedure_ID_Error()
        {
            var controller = new ProcedureController(DummyDatabase);
            Procedure procedure = new Procedure();
            procedure.Name = "Name";
            procedure.Requirement = "Requirement";
            procedure.Room_type = "Procedural";
            procedure.Information = "No eating for 30 mins after the procedure";
            procedure.Duration = 60;
            procedure.Personnel_count = 2;
            procedure.Price = 50;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutProcedure(970131, procedure);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void PutProcedure_Name_NotNull()
        {
            var controller = new ProcedureController(Database);
            Procedure procedure = new Procedure();
            procedure.Requirement = "Requirement";
            procedure.Room_type = "Procedural";
            procedure.Information = "No eating for 30 mins after the procedure";
            procedure.Duration = 60;
            procedure.Personnel_count = 2;
            procedure.Price = 50;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutProcedure(3, procedure);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutProcedure_Requirement_NotNull()
        {
            var controller = new ProcedureController(Database);
            Procedure procedure = new Procedure();
            procedure.Name = "Name";
            procedure.Room_type = "Procedural";
            procedure.Information = "No eating for 30 mins after the procedure";
            procedure.Duration = 60;
            procedure.Personnel_count = 2;
            procedure.Price = 50;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutProcedure(3, procedure);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutProcedure_Room_type_NotNull()
        {
            var controller = new ProcedureController(Database);
            Procedure procedure = new Procedure();
            procedure.Name = "Name";
            procedure.Requirement = "Requirement";
            procedure.Information = "No eating for 30 mins after the procedure";
            procedure.Duration = 60;
            procedure.Personnel_count = 2;
            procedure.Price = 50;
            procedure.Image = "Image.jpg";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutProcedure(3, procedure);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutProcedure_Information_NotNull()
        {
            var controller = new ProcedureController(Database);
            Procedure procedure = new Procedure();
            procedure.Name = "Name";
            procedure.Requirement = "Requirement";
            procedure.Room_type = "Procedural";
            procedure.Duration = 60;
            procedure.Personnel_count = 2;
            procedure.Price = 50;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutProcedure(3, procedure);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutProcedure_Duration_NotNull()
        {
            var controller = new ProcedureController(Database);
            Procedure procedure = new Procedure();
            procedure.Name = "Name";
            procedure.Requirement = "Requirement";
            procedure.Room_type = "Procedural";
            procedure.Information = "No eating for 30 mins after the procedure";
            procedure.Personnel_count = 2;
            procedure.Price = 50;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutProcedure(3, procedure);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutProcedure_Price_NotNull()
        {
            var controller = new ProcedureController(Database);
            Procedure procedure = new Procedure();
            procedure.Name = "Name";
            procedure.Requirement = "Requirement";
            procedure.Room_type = "Procedural";
            procedure.Information = "No eating for 30 mins after the procedure";
            procedure.Duration = 60;
            procedure.Personnel_count = 2;
            procedure.Price = -1;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutProcedure(3, procedure);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutProcedure_Image_Null()
        {
            var controller = new ProcedureController(Database);
            Procedure procedure = new Procedure();
            procedure.Name = "Name";
            procedure.Requirement = "Requirement";
            procedure.Information = "No eating for 30 mins after the procedure";
            procedure.Room_type = "Procedural";
            procedure.Duration = 60;
            procedure.Personnel_count = 2;
            procedure.Price = 50;
            procedure.Image = "";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutProcedure(3, procedure);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void PutProcedure_NotNull()
        {
            var controller = new ProcedureController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutProcedure(3, null);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void zDeleteProcedure_Response_200()
        {
            var controller = new ProcedureController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.DeleteProcedure(3);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void zDeleteProceduree_Response_200()
        {
            var controller = new ProcedureController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.DeleteProcedure(4);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void DeleteProcedure_Response_404()
        {
            var controller = new ProcedureController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.DeleteProcedure(970131);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.NotFoundObjectResult;
            Assert.AreEqual(404, response2.StatusCode);
        }
        [TestMethod]
        public void DeleteProcedure_DatabaseError()
        {
            var controller = new ProcedureController(DummyDatabase);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.DeleteProcedure(3);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
    }
}