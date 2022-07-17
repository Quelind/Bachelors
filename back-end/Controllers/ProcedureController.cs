using ClinicAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ClinicAPI.Controllers
{
    [Route("api/procedure")]
    [ApiController]
    public class ProcedureController : Controller
    {
        private AppDatabase Database { get; set; }
        public ProcedureController(AppDatabase database)
        {
            Database = database;
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetProcedure()
        {
            List<Procedure> procedures = new List<Procedure>();
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
                                FROM procedures";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        procedures.Add(new Procedure()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["name"]),
                            Requirement = Convert.ToString(reader["requirement"]),
                            Room_type = Convert.ToString(reader["room_type"]),
                            Information = Convert.ToString(reader["information"]),
                            Duration = Convert.ToInt32(reader["duration"]),
                            Personnel_count = Convert.ToInt32(reader["personnel_count"]),
                            Image = Convert.ToString(reader["image"]),
                            Price = Convert.ToDouble(reader["price"])
                        });
                    }
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the procedures.");
            }
            return Json(procedures);
        }
        [Authorize]
        [Produces("application/json")]
        [HttpGet("{id}")]
        public IActionResult GetProcedure(int id)
        {
            Procedure procedure = new Procedure();
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
                                              FROM procedures 
                                              WHERE id = {0}", id);

                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    procedure.Id = Convert.ToInt32(reader["id"]);
                    procedure.Name = Convert.ToString(reader["name"]);
                    procedure.Requirement = Convert.ToString(reader["requirement"]);
                    procedure.Room_type = Convert.ToString(reader["room_type"]);
                    procedure.Information = Convert.ToString(reader["information"]);
                    procedure.Duration = Convert.ToInt32(reader["duration"]);
                    procedure.Personnel_count = Convert.ToInt32(reader["personnel_count"]);
                    procedure.Image = Convert.ToString(reader["image"]);
                    procedure.Price = Convert.ToDouble(reader["price"]);
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the procedure.");
            }
            return Json(procedure);
        }
        [Authorize]
        [Produces("application/json")]
        [HttpGet("={search}")]
        public IActionResult ProcedureSearch(string search)
        {
            List<Procedure> procedures = new List<Procedure>();
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
                                              FROM procedures 
                                              WHERE procedures.name LIKE '%{0}%' 
                                              OR procedures.requirement LIKE '%{0}%'
                                              OR procedures.room_type LIKE '%{0}%'", search);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        procedures.Add(new Procedure()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["name"]),
                            Requirement = Convert.ToString(reader["requirement"]),
                            Room_type = Convert.ToString(reader["room_type"]),
                            Information = Convert.ToString(reader["information"]),
                            Duration = Convert.ToInt32(reader["duration"]),
                            Personnel_count = Convert.ToInt32(reader["personnel_count"]),
                            Image = Convert.ToString(reader["image"]),
                            Price = Convert.ToDouble(reader["price"])
                        });
                    }
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the procedure.");
            }
            return Json(procedures);
        }
        [Produces("application/json")]
        [Authorize(Roles = Role.Admin + "," + Role.Employee)]
        [HttpPost]
        public IActionResult PostProcedure([FromBody] Procedure procedure)
        {
            if (procedure == null)
            {
                return BadRequest("No procedure provided.");
            }
            if (procedure.Name == null)
            {
                return BadRequest("The procedure's name cannot be null.");
            }
            if (procedure.Requirement == null)
            {
                return BadRequest("The procedure's requirement cannot be null.");
            }
            if (procedure.Room_type == null)
            {
                return BadRequest("The procedure's room type cannot be null.");
            }
            if (procedure.Information == null)
            {
                return BadRequest("The procedure's information cannot be null.");
            }
            if (procedure.Duration <= 0)
            {
                return BadRequest("The procedure's duration cannot be negative or zero.");
            }
            if (procedure.Price <= 0)
            {
                return BadRequest("The procedure's price cannot be negative or zero.");
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
                cmd.CommandText = @"INSERT into procedures (
                                        name,
                                        requirement,
                                        room_type,
                                        information,
                                        duration,
                                        personnel_count,
                                        image,
                                        price) 
                                    VALUES(
                                        @Name, 
                                        @Requirement, 
                                        @Room_type, 
                                        @Information, 
                                        @Duration, 
                                        @Personnel_count,
                                        @Image,
                                        @Price)";
                cmd.Parameters.AddWithValue("@Name", procedure.Name);
                cmd.Parameters.AddWithValue("@Requirement", procedure.Requirement);
                cmd.Parameters.AddWithValue("@Room_type", procedure.Room_type);
                cmd.Parameters.AddWithValue("@Information", procedure.Information);
                cmd.Parameters.AddWithValue("@Duration", procedure.Duration);
                cmd.Parameters.AddWithValue("@Personnel_count", procedure.Personnel_count);
                cmd.Parameters.AddWithValue("@Price", procedure.Price);
                if (procedure.Image != "")
                {
                    cmd.Parameters.AddWithValue("@Image", procedure.Image);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Image", "https://i.imgur.com/5VjZS1K.png");
                }
                int code = cmd.ExecuteNonQuery();
                if (code == 0)
                {
                    return BadRequest("The information provided is incorrect.");
                }
                return Ok("The procedure has been added successfully.");
            }
            catch
            {
                return StatusCode(500, "Failed to add the procedure.");
            }

        }
        [Authorize(Roles = Role.Admin + "," + Role.Employee)]
        [Produces("application/json")]
        [HttpPut("{id}")]
        public IActionResult PutProcedure(int id, [FromBody] Procedure procedure)
        {
            if (procedure == null)
            {
                return BadRequest("No procedure provided.");
            }
            if (procedure.Name == null)
            {
                return BadRequest("The procedure's name cannot be null.");
            }
            if (procedure.Requirement == null)
            {
                return BadRequest("The procedure's requirement cannot be null.");
            }
            if (procedure.Room_type == null)
            {
                return BadRequest("The procedure's room type cannot be null.");
            }
            if (procedure.Information == null)
            {
                return BadRequest("The procedure's information cannot be null.");
            }
            if (procedure.Duration <= 0)
            {
                return BadRequest("The procedure's duration cannot be negative or zero.");
            }
            if (procedure.Price <= 0)
            {
                return BadRequest("The procedure's price cannot be negative or zero.");
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
                cmd.CommandText = @"UPDATE procedures
                                    SET name=@Name,
                                        requirement=@Requirement,
                                        room_type=@Room_type,
                                        information=@Information,
                                        duration=@Duration,
                                        personnel_count=@Personnel_count,
                                        image=@Image,
                                        price=@Price
                                    WHERE id=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@Name", procedure.Name);
                cmd.Parameters.AddWithValue("@Requirement", procedure.Requirement);
                cmd.Parameters.AddWithValue("@Room_type", procedure.Room_type);
                cmd.Parameters.AddWithValue("@Information", procedure.Information);
                cmd.Parameters.AddWithValue("@Duration", procedure.Duration);
                cmd.Parameters.AddWithValue("@Personnel_count", procedure.Personnel_count);
                cmd.Parameters.AddWithValue("@Price", procedure.Price);
                if (procedure.Image != "")
                {
                    cmd.Parameters.AddWithValue("@Image", procedure.Image);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Image", "https://i.imgur.com/5VjZS1K.png");
                }

                int code = cmd.ExecuteNonQuery();
                if (code == 0)
                {
                    return BadRequest("The procedure does not exist or the information provided is incorrect.");
                }
                return Ok("The procedure has been updated successfully.");
            }
            catch
            {
                return StatusCode(500, "Could not update the procedure.");
            }
        }
        [Authorize(Roles = Role.Admin + "," + Role.Employee)]
        [Produces("application/json")]
        [HttpDelete("{id}")]
        public IActionResult DeleteProcedure(int id)
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
                                                  FROM procedures 
                                                  WHERE id = {0}", id);
                int code = cmd.ExecuteNonQuery();
                if (code == 0)
                {
                    return NotFound("The procedure does not exist.");
                }
                cmd.CommandText = string.Format(@"ALTER TABLE procedures AUTO_INCREMENT=1;", id);
                code = cmd.ExecuteNonQuery();
                return Ok("The procedure has been deleted successfully.");
            }
            catch
            {
                return StatusCode(500, "Failed to delete the procedure.");
            }
        }
    }
}
