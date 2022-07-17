using ClinicAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ClinicAPI.Controllers
{
    [Route("api/room")]
    [ApiController]
    public class RoomController : Controller
    {
        private AppDatabase Database { get; set; }
        public RoomController(AppDatabase database)
        {
            Database = database;
        }
        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public IActionResult GetRoom()
        {
            List<Room> rooms = new List<Room>();
            try
            {
                MySqlCommand cmd;
                try
                {
                    Database.Connection.Open();
                }
                catch
                {
                    return StatusCode(500, "Could not establish a database connection.");
                }
                cmd = Database.Connection.CreateCommand();
                cmd.CommandText = @"SELECT * 
                                FROM rooms";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rooms.Add(new Room()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["name"]),
                            Type = Convert.ToString(reader["type"]),
                            Capacity = Convert.ToInt32(reader["capacity"])
                        });
                    }
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the rooms.");
            }
            return Json(rooms);
        }
        [Authorize(Roles = Role.Admin)]
        [Produces("application/json")]
        [HttpGet("{id}")]
        public IActionResult GetRoom(int id)
        {
            Room room = new Room();
            try
            {
                MySqlCommand cmd;
                try
                {
                    Database.Connection.Open();
                }
                catch
                {
                    return StatusCode(500, "Could not establish a database connection.");
                }
                cmd = Database.Connection.CreateCommand();


                cmd.CommandText = string.Format(@"SELECT *
                                              FROM rooms 
                                              WHERE id = '{0}'", id);

                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    room.Id = Convert.ToInt32(reader["id"]);
                    room.Name = Convert.ToString(reader["name"]);
                    room.Type = Convert.ToString(reader["type"]);
                    room.Capacity = Convert.ToInt32(reader["capacity"]);
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the room.");
            }
            return Json(room);
        }
        [Authorize(Roles = Role.Admin + "," + Role.Employee)]
        [Produces("application/json")]
        [HttpGet("={search}")]
        public IActionResult RoomSearch(string search)
        {
            List<Room> rooms = new List<Room>();
            try
            {
                MySqlCommand cmd;
                try
                {
                    Database.Connection.Open();
                }
                catch
                {
                    return StatusCode(500, "Could not establish a database connection.");
                }
                cmd = Database.Connection.CreateCommand();
                cmd.CommandText = string.Format(@"SELECT *
                                              FROM rooms 
                                              WHERE rooms.name LIKE '%{0}%' 
                                              OR rooms.type LIKE '%{0}%'
                                              OR rooms.capacity LIKE '%{0}%'", search);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rooms.Add(new Room()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["name"]),
                            Type = Convert.ToString(reader["type"]),
                            Capacity = Convert.ToInt32(reader["capacity"])
                        });
                    }
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the room.");
            }
            return Json(rooms);
        }
        [Produces("application/json")]
        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        public IActionResult PostRoom([FromBody] Room room)
        {
            if (room == null)
            {
                return BadRequest("No room provided.");
            }
            if (room.Name == null)
            {
                return BadRequest("The room's name cannot be null.");
            }
            if (room.Type == null)
            {
                return BadRequest("The room's type cannot be null.");
            }
            if (room.Capacity <= 0)
            {
                return BadRequest("The room's capacity cannot be negative or null.");
            }
            try
            {
                MySqlCommand cmd;
                try
                {
                    Database.Connection.Open();
                }
                catch
                {
                    return StatusCode(500, "Could not establish a database connection.");
                }
                cmd = Database.Connection.CreateCommand();
                cmd.CommandText = @"INSERT into rooms (
                                        name,
                                        type,
                                        capacity) 
                                    VALUES(
                                        @Name, 
                                        @Type, 
                                        @Capacity)";
                cmd.Parameters.AddWithValue("@Name", room.Name);
                cmd.Parameters.AddWithValue("@Type", room.Type);
                cmd.Parameters.AddWithValue("@Capacity", room.Capacity);
                int code  = cmd.ExecuteNonQuery();
                if (code == 0)
                {
                    return BadRequest("The information provided is incorrect.");
                }
                return Ok("The room has been added successfully.");
            }
            catch
            {
                return StatusCode(500, "Failed to add the room.");
            }

        }
        [Authorize(Roles = Role.Admin)]
        [Produces("application/json")]
        [HttpPut("{id}")]
        public IActionResult PutRoom(int id, [FromBody] Room room)
        {
            if (room == null)
            {
                return BadRequest("No room provided.");
            }
            if (room.Name == null)
            {
                return BadRequest("The room's name cannot be null.");
            }
            if (room.Type == null)
            {
                return BadRequest("The room's type cannot be null.");
            }
            if (room.Capacity <= 0)
            {
                return BadRequest("The room's capacity cannot be negative or null.");
            }

            try
            {
                MySqlCommand cmd;
                try
                {
                    Database.Connection.Open();
                }
                catch
                {
                    return StatusCode(500, "Could not establish a database connection.");
                }
                cmd = Database.Connection.CreateCommand();
                cmd.CommandText = @"UPDATE rooms
                                    SET name=@Name,
                                        type=@Type,
                                        capacity=@Capacity
                                    WHERE id=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@Name", room.Name);
                cmd.Parameters.AddWithValue("@Type", room.Type);
                cmd.Parameters.AddWithValue("@capacity", room.Capacity);
                int code = cmd.ExecuteNonQuery();
                if (code == 0)
                {
                    return BadRequest("The room does not exist or the information provided is incorrect.");
                }
                return Ok("The room has been updated successfully.");
            }
            catch
            {
                return StatusCode(500, "Could not update the room.");
            }
        }
        [Authorize(Roles = Role.Admin)]
        [Produces("application/json")]
        [HttpDelete("{id}")]
        public IActionResult DeleteRoom(int id)
        {
            try
            {
                MySqlCommand cmd;
                try
                {
                    Database.Connection.Open();
                }
                catch
                {
                    return StatusCode(500, "Could not establish a database connection.");
                }
                cmd = Database.Connection.CreateCommand();
                cmd.CommandText = string.Format(@"DELETE 
                                                  FROM rooms 
                                                  WHERE id = '{0}'", id);
                int code = cmd.ExecuteNonQuery();
                if (code == 0)
                {
                    return NotFound("The room does not exist.");
                }
                cmd.CommandText = string.Format(@"ALTER TABLE rooms AUTO_INCREMENT=1;", id);
                 code = cmd.ExecuteNonQuery();
                return Ok("The room has been deleted successfully.");
            }
            catch
            {
                return StatusCode(500, "Failed to delete the room.");
            }
        }
    }
}
