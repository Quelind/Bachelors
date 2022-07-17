using ClinicAPI;
using ClinicAPI.Controllers;
using ClinicAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Clinic.Tests
{
    [TestClass()]
    public class RoomTests
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
        public void GetRoom_IsRoom()
        {
            var controller = new RoomController(Database);
            List<Room> room = new List<Room>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetRoom();
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(room.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void GetRoom_DatabaseError()
        {
            var controller = new RoomController(DummyDatabase);
            List<Room> room = new List<Room>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetRoom();
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void GetRoomID_IsRoom()
        {
            var controller = new RoomController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetRoom(1);
            Database.Connection.Dispose();
            Room room = new Room();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(room.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void GetRoomID_DatabaseError()
        {
            var controller = new RoomController(DummyDatabase);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetRoom(1);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void GetRoomID_Id_NotNull()
        {
            var controller = new RoomController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetRoom(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Room;
            Assert.IsTrue(response3.Id > -1);
        }
        [TestMethod]
        public void GetRoomID_Type_NotNull()
        {
            var controller = new RoomController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.GetRoom(1);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            var response3 = response2.Value as Room;
            Assert.AreNotEqual(null, response3.Type);
        }
        [TestMethod]
        public void SearchRoom_IsRoom()
        {
            var controller = new RoomController(Database);
            List<Room> room = new List<Room>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.RoomSearch("a");
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.JsonResult;
            Assert.AreEqual(room.GetType(), response2.Value.GetType());
        }
        [TestMethod]
        public void SearchRoom_DatabaseError()
        {
            var controller = new RoomController(DummyDatabase);
            List<Room> room = new List<Room>();
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.RoomSearch("a");
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void PostRoom_Response_OK()
        {
            var controller = new RoomController(Database);
            Room room = new Room();
            room.Name = "Name";
            room.Type = "Type";
            room.Capacity = 400;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostRoom(room);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void PostRoom_DatabaseError()
        {
            var controller = new RoomController(DummyDatabase);
            Room room = new Room();
            room.Name = "Name";
            room.Type = "Type";
            room.Capacity = 400;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostRoom(room);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void PostRoom_Name_NotNull()
        {
            var controller = new RoomController(Database);
            Room room = new Room();
            room.Type = "Type";
            room.Capacity = 400;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostRoom(room);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostRoom_Type_NotNull()
        {
            var controller = new RoomController(Database);
            Room room = new Room();
            room.Name = "Name";
            room.Capacity = 400;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostRoom(room);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostRoom_Capacity_NotNull()
        {
            var controller = new RoomController(Database);
            Room room = new Room();
            room.Name = "Name";
            room.Type = "Type";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostRoom(room);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PostRoom_NotNull()
        {
            var controller = new RoomController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PostRoom(null);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutRoom_Response_OK()
        {
            var controller = new RoomController(Database);
            Room room = new Room();
            room.Name = "Name";
            room.Type = "Type";
            room.Capacity = 400;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutRoom(5, room);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void PutRoom_DatabaseError()
        {
            var controller = new RoomController(DummyDatabase);
            Room room = new Room();
            room.Name = "Name";
            room.Type = "Type";
            room.Capacity = 500;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutRoom(5, room);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void PutRoom_ID_Error()
        {
            var controller = new RoomController(DummyDatabase);
            Room room = new Room();
            room.Name = "Name";
            room.Type = "Type";
            room.Capacity = 500;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutRoom(970131, room);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
        [TestMethod]
        public void PutRoom_Name_NotNull()
        {
            var controller = new RoomController(Database);
            Room room = new Room();
            room.Type = "Type";
            room.Capacity = 400;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutRoom(5, room);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutRoom_Type_NotNull()
        {
            var controller = new RoomController(Database);
            Room room = new Room();
            room.Name = "Name";
            room.Capacity = 400;
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutRoom(5, room);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutRoom_Capacity_NotNull()
        {
            var controller = new RoomController(Database);
            Room room = new Room();
            room.Name = "Name";
            room.Type = "Type";
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutRoom(5, room);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void PutRoom_NotNull()
        {
            var controller = new RoomController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.PutRoom(5, null);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            Assert.AreEqual(400, response2.StatusCode);
        }
        [TestMethod]
        public void zDeleteRoom_Response_200()
        {
            var controller = new RoomController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.DeleteRoom(5);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.AreEqual(200, response2.StatusCode);
        }
        [TestMethod]
        public void DeleteRoom_Response_404()
        {
            var controller = new RoomController(Database);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.DeleteRoom(970131);
            Database.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.NotFoundObjectResult;
            Assert.AreEqual(404, response2.StatusCode);
        }
        [TestMethod]
        public void DeleteRoom_DatabaseError()
        {
            var controller = new RoomController(DummyDatabase);
            Microsoft.AspNetCore.Mvc.IActionResult response = controller.DeleteRoom(5);
            DummyDatabase.Connection.Dispose();
            var response2 = response as Microsoft.AspNetCore.Mvc.ObjectResult;
            Assert.AreEqual(500, response2.StatusCode);
        }
    }
}