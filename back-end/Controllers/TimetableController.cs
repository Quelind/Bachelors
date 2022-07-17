using ClinicAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ClinicAPI.Controllers
{
    [Route("api/timetable")]
    [ApiController]
    public class TimetableController : Controller
    {
        private AppDatabase Database { get; set; }
        public TimetableController(AppDatabase database)
        {
            Database = database;
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetTimetable()
        {
            List<Timetable> timetables = new List<Timetable>();
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
                                FROM timetables";
                using (var reader = cmd.ExecuteReader())
                {
                    DateTime date;
                    string date2;
                    while (reader.Read())
                    {
                        date = Convert.ToDateTime(reader["date"]);
                        date2 = date.ToString("yyyy-MM-dd");
                        timetables.Add(new Timetable()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Date = date2,
                            Time = Convert.ToString(reader["time"]),
                            IsLocked = Convert.ToString(reader["isLocked"])
                        });
                    }
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the timetables.");
            }
            return Json(timetables);
        }
        [Authorize]
        [HttpGet("={search}")]
        public IActionResult GetTimetableUnlocked(string search)
        {
            List<Timetable> timetables = new List<Timetable>();
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
                                              FROM timetables 
                                              WHERE NOT timetables.isLocked LIKE '%{0}%'", search);
                using (var reader = cmd.ExecuteReader())
                {
                    DateTime date;
                    string date2;
                    while (reader.Read())
                    {
                        date = Convert.ToDateTime(reader["date"]);
                        date2 = date.ToString("yyyy-MM-dd");
                        timetables.Add(new Timetable()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Date = date2,
                            Time = Convert.ToString(reader["time"]),
                            IsLocked = Convert.ToString(reader["isLocked"])
                        });
                    }
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the timetables.");
            }
            return Json(timetables);
        }
        [Authorize]
        [Produces("application/json")]
        [HttpGet("{id}")]
        public IActionResult GetTimetable(int id)
        {
            Timetable timetable = new Timetable();
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
                                              FROM timetables 
                                              WHERE id = {0}", id);

                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    timetable.Id = Convert.ToInt32(reader["id"]);
                    timetable.Date = Convert.ToString(reader["date"]);
                    timetable.Time = Convert.ToString(reader["time"]);
                    timetable.IsLocked = Convert.ToString(reader["isLocked"]);
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the timetable.");
            }
            return Json(timetable);
        }
    }
}
