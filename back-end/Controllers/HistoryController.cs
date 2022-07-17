using ClinicAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ClinicAPI.Controllers
{
    [Route("api/history")]
    [ApiController]
    public class HistoryController : Controller
    {
        private AppDatabase Database { get; set; }
        public HistoryController(AppDatabase database)
        {
            Database = database;
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetHistory()
        {
            List<History> histories = new List<History>();
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
                cmd.CommandText = @"SELECT 
                                        histories.id,
                                        histories.date,
                                        histories.name,
                                        histories.description,
                                        histories.fk_patient,
                                        patients.fk_user,
                                        patient.name as patient_name,
                                        patient.surname as patient_surname
                                    FROM `histories`
                                    LEFT JOIN patients ON histories.fk_patient=patients.id
                                    LEFT JOIN users patient ON patients.fk_user=patient.id";
                using (var reader = cmd.ExecuteReader())
                {
                    DateTime date;
                    string date2;
                    while (reader.Read())
                    {
                        date = Convert.ToDateTime(reader["date"]);
                        date2 = date.ToString("yyyy-MM-dd");
                        histories.Add(new History()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Date = date2,
                            Name = Convert.ToString(reader["name"]),
                            Description = Convert.ToString(reader["description"]),
                            Fk_patient = Convert.ToInt32(reader["fk_patient"]),
                            Patient_name = Convert.ToString(reader["patient_name"]),
                            Patient_surname = Convert.ToString(reader["patient_surname"])
                        });
                    }
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the histories.");
            }
            return Json(histories);
        }
        [Authorize]
        [Produces("application/json")]
        [HttpGet("{id}")]
        public IActionResult GetHistory(int id)
        {
            List<History> histories = new List<History>();
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
                cmd.CommandText = string.Format(@"SELECT 
                                        histories.id,
                                        histories.date,
                                        histories.name,
                                        histories.description,
                                        histories.fk_patient,
                                        patients.fk_user,
                                        patient.name as patient_name,
                                        patient.surname as patient_surname
                                    FROM `histories`
                                    LEFT JOIN patients ON histories.fk_patient=patients.id
                                    LEFT JOIN users patient ON patients.fk_user=patient.id
                                    WHERE histories.fk_patient = {0}", id);

                using (var reader = cmd.ExecuteReader())
                {
                    DateTime date;
                    string date2;
                    while (reader.Read())
                    {
                        date = Convert.ToDateTime(reader["date"]);
                        date2 = date.ToString("yyyy-MM-dd");
                        histories.Add(new History()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Date = date2,
                            Name = Convert.ToString(reader["name"]),
                            Description = Convert.ToString(reader["description"]),
                            Fk_patient = Convert.ToInt32(reader["fk_patient"]),
                            Patient_name = Convert.ToString(reader["patient_name"]),
                            Patient_surname = Convert.ToString(reader["patient_surname"])
                        });
                    }
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the history.");
            }
            return Json(histories);
        }
        [Authorize]
        [Produces("application/json")]
        [HttpGet("={id}={search}")]
        public IActionResult HistorySearch(string search, int id)
        {
            List<History> histories = new List<History>();
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
                cmd.CommandText = string.Format(@"SELECT 
                                                 histories.id,
                                                 histories.date,
                                                 histories.name,
                                                 histories.description,
                                                 histories.fk_patient,
                                        patients.fk_user,
                                        patient.name as patient_name,
                                        patient.surname as patient_surname
                                    FROM `histories`
                                    LEFT JOIN patients ON histories.fk_patient=patients.id
                                    LEFT JOIN users patient ON patients.fk_user=patient.id
                                              WHERE (histories.name LIKE '%{0}%'
                                              OR histories.description LIKE '%{0}%')
                                              AND histories.fk_patient = {1}", search, id);
                using (var reader = cmd.ExecuteReader())
                {
                    DateTime date;
                    string date2;
                    while (reader.Read())
                    {
                        date = Convert.ToDateTime(reader["date"]);
                        date2 = date.ToString("yyyy-MM-dd");
                        histories.Add(new History()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Date = date2,
                            Name = Convert.ToString(reader["name"]),
                            Description = Convert.ToString(reader["description"]),
                            Patient_name = Convert.ToString(reader["patient_name"]),
                            Patient_surname = Convert.ToString(reader["patient_surname"])
                        });
                    }
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the history.");
            }
            return Json(histories);
        }
    }
}
