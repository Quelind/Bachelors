using ClinicAPI.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ClinicAPI.Controllers
{
    [Route("api/visit")]
    [ApiController]
    public class VisitController : Controller
    {
        private AppDatabase Database { get; set; }
        public VisitController(AppDatabase database)
        {
            Database = database;
        }
        DateTime date;
        string date2;
        [HttpGet]
        [Authorize]
        public IActionResult GetVisit()
        {
            try
            {
                Database.Connection.Open();
            }
            catch
            {
                return StatusCode(500, "Could not establish a database connection.");
            }
            int timetableResult = -1;
            try
            {
                timetableResult = GetTimetable();
            }
            catch
            {
                return StatusCode(500, "Failed to get the timetable.");
            }
            
            List<Visit> visits = new List<Visit>();
            try
            {
                MySqlCommand cmd;

                cmd = Database.Connection.CreateCommand();
                cmd.CommandText = string.Format(@"SELECT 
                                        visits.id,
                                        visits.patient_comment,
                                        visits.fk_patient,
                                        visits.fk_doctor,
                                        visits.fk_room,
                                        visits.fk_timetable,
                                        visits.fk_procedure,
                                        visits.confirmed,
                                        patients.fk_user,
                                        patient.name as patient_name,
                                        patient.surname as patient_surname,
                                        employees.fk_user,
                                        employee.name as doctor_name,
                                        employee.surname as doctor_surname,
                                        employees.specialization as specialization,
                                        timetables.date as date,
                                        timetables.time as time,
                                        timetables.isLocked as isLocked,
                                        procedures.name as procedure_name,
                                        procedures.information as procedure_information
                                    FROM `visits`
                                    LEFT JOIN patients ON visits.fk_patient=patients.id
                                    LEFT JOIN employees ON visits.fk_doctor=employees.id
                                    LEFT JOIN timetables ON visits.fk_timetable=timetables.id
                                    LEFT JOIN procedures ON visits.fk_procedure=procedures.id
                                    LEFT JOIN users employee ON employees.fk_user=employee.id
                                    LEFT JOIN users patient ON patients.fk_user=patient.id
                                    WHERE visits.fk_timetable <= {0}
                                    ORDER BY timetables.date, timetables.time;", timetableResult);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        date = Convert.ToDateTime(reader["date"]);
                        date2 = date.ToString("yyyy-MM-dd");
                        visits.Add(new Visit()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Date = date2,
                            Time = Convert.ToString(reader["time"]),
                            IsLocked = Convert.ToString(reader["isLocked"]),
                            Patient_comment = Convert.ToString(reader["patient_comment"]),
                            Fk_patient = Convert.ToInt32(reader["fk_patient"]),
                            Fk_doctor = Convert.ToInt32(reader["fk_doctor"]),
                            Fk_timetable = Convert.ToInt32(reader["fk_timetable"]),
                            Fk_room = Convert.ToString(reader["fk_room"]),
                            Patient_name = Convert.ToString(reader["patient_name"]),
                            Patient_surname = Convert.ToString(reader["patient_surname"]),
                            Doctor_name = Convert.ToString(reader["doctor_name"]),
                            Doctor_surname = Convert.ToString(reader["doctor_surname"]),
                            Specialization = Convert.ToString(reader["specialization"]),
                            Fk_procedure = Convert.ToInt32(reader["fk_procedure"]),
                            Procedure_information = Convert.ToString(reader["procedure_information"]),
                            Procedure_name = Convert.ToString(reader["procedure_name"]),
                            Confirmed = Convert.ToString(reader["confirmed"])
                        });
                    }
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the visits.");
            }

            return Json(visits);
        }
        [Produces("application/json")]
        [HttpGet("{id}")]
        public IActionResult GetVisit(int id)
        {
            Visit visit = new Visit();
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
                cmd.CommandText = @"    SELECT 
                                        visits.id,
                                        visits.patient_comment,
                                        visits.fk_patient,
                                        visits.fk_doctor,
                                        visits.fk_room,
                                        visits.fk_timetable,
                                        visits.fk_procedure,
                                        visits.confirmed,
                                        patients.fk_user,
                                        patient.name as patient_name,
                                        patient.surname as patient_surname,
                                        employees.fk_user,
                                        employee.name as doctor_name,
                                        employee.surname as doctor_surname,
                                        employees.specialization as specialization,
                                        timetables.date as date,
                                        timetables.time as time,
                                        timetables.isLocked as isLocked,
                                        procedures.name as procedure_name,
                                        procedures.information as procedure_information
                                    FROM `visits`
                                    LEFT JOIN patients ON visits.fk_patient=patients.id
                                    LEFT JOIN employees ON visits.fk_doctor=employees.id
                                    LEFT JOIN timetables ON visits.fk_timetable=timetables.id
                                    LEFT JOIN procedures ON visits.fk_procedure=procedures.id
                                    LEFT JOIN users employee ON employees.fk_user=employee.id
                                    LEFT JOIN users patient ON patients.fk_user=patient.id
                                    WHERE visits.id=@id";
                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reader.Read();
                        date = Convert.ToDateTime(reader["date"]);
                        date2 = date.ToString("yyyy-MM-dd");
                        visit.Id = Convert.ToInt32(reader["id"]);
                        visit.Date = date2;
                        visit.Time = Convert.ToString(reader["time"]);
                        visit.Patient_comment = Convert.ToString(reader["patient_comment"]);
                        visit.Fk_patient = Convert.ToInt32(reader["fk_patient"]);
                        visit.Fk_doctor = Convert.ToInt32(reader["fk_doctor"]);
                        visit.Fk_timetable = Convert.ToInt32(reader["fk_timetable"]);
                        visit.Fk_room = Convert.ToString(reader["fk_room"]);
                        visit.Patient_name = Convert.ToString(reader["patient_name"]);
                        visit.Patient_surname = Convert.ToString(reader["patient_surname"]);
                        visit.Doctor_name = Convert.ToString(reader["doctor_name"]);
                        visit.Doctor_surname = Convert.ToString(reader["doctor_surname"]);
                        visit.Specialization = Convert.ToString(reader["specialization"]);
                        visit.Fk_procedure = Convert.ToInt32(reader["fk_procedure"]);
                        visit.Procedure_information = Convert.ToString(reader["procedure_information"]);
                        visit.Procedure_name = Convert.ToString(reader["procedure_name"]);
                        visit.Confirmed = Convert.ToString(reader["confirmed"]);
                    }
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the visit.");
            }
            return Json(visit);
        }
        [Authorize]
        [Produces("application/json")]
        [HttpGet("={search}")]
        public IActionResult VisitSearch(string search)
        {
            try
            {
                Database.Connection.Open();
            }
            catch
            {
                return StatusCode(500, "Could not establish a database connection.");
            }
            int timetableResult = -1;
            try
            {
                timetableResult = GetTimetable();
            }
            catch
            {
                return StatusCode(500, "Failed to get the timetable.");
            }
            List<Visit> visits = new List<Visit>();
            try
            {
                MySqlCommand cmd;

                cmd = Database.Connection.CreateCommand();
                cmd.CommandText = string.Format(@"SELECT 
                                        visits.id,
                                        visits.patient_comment,
                                        visits.fk_patient,
                                        visits.fk_doctor,
                                        visits.fk_room,
                                        visits.fk_timetable,
                                        visits.fk_procedure,
                                        visits.confirmed,
                                        patients.fk_user,
                                        patient.name as patient_name,
                                        patient.surname as patient_surname,
                                        employees.fk_user,
                                        employee.name as doctor_name,
                                        employee.surname as doctor_surname,
                                        employees.specialization as specialization,
                                        timetables.date as date,
                                        timetables.time as time,
                                        timetables.isLocked as isLocked,
                                        procedures.name as procedure_name,
                                        procedures.information as procedure_information
                                    FROM `visits`
                                    LEFT JOIN patients ON visits.fk_patient=patients.id
                                    LEFT JOIN employees ON visits.fk_doctor=employees.id
                                    LEFT JOIN timetables ON visits.fk_timetable=timetables.id
                                    LEFT JOIN procedures ON visits.fk_procedure=procedures.id
                                    LEFT JOIN users employee ON employees.fk_user=employee.id
                                    LEFT JOIN users patient ON patients.fk_user=patient.id
                                              WHERE (date LIKE '%{0}%' 
                                              OR time LIKE '%{0}%'
                                              OR patient.name LIKE '%{0}%'
                                              OR patient.surname LIKE '%{0}%'
                                              OR employee.name LIKE '%{0}%'
                                              OR employee.surname LIKE '%{0}%'
                                              OR visits.fk_room LIKE '%{0}%'
                                              OR employees.specialization LIKE '%{0}%'
                                              OR procedures.name LIKE '%{0}%')
											  AND visits.fk_timetable <= {1}
											  ORDER BY timetables.date, timetables.time;", search, timetableResult);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        date = Convert.ToDateTime(reader["date"]);
                        date2 = date.ToString("yyyy-MM-dd");
                        visits.Add(new Visit()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Date = date2,
                            Time = Convert.ToString(reader["time"]),
                            Patient_comment = Convert.ToString(reader["patient_comment"]),
                            Fk_patient = Convert.ToInt32(reader["fk_patient"]),
                            Fk_doctor = Convert.ToInt32(reader["fk_doctor"]),
                            Fk_timetable = Convert.ToInt32(reader["fk_timetable"]),
                            Fk_room = Convert.ToString(reader["fk_room"]),
                            Patient_name = Convert.ToString(reader["patient_name"]),
                            Patient_surname = Convert.ToString(reader["patient_surname"]),
                            Doctor_name = Convert.ToString(reader["doctor_name"]),
                            Doctor_surname = Convert.ToString(reader["doctor_surname"]),
                            Specialization = Convert.ToString(reader["specialization"]),
                            Fk_procedure = Convert.ToInt32(reader["fk_procedure"]),
                            Procedure_information = Convert.ToString(reader["procedure_information"]),
                            Procedure_name = Convert.ToString(reader["procedure_name"]),
                            Confirmed = Convert.ToString(reader["confirmed"])
                        });
                    }
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the visits.");
            }

            return Json(visits);
        }
        [Produces("application/json")]
        [Authorize]
        [HttpPost]
        public IActionResult PostVisit([FromBody] Visit visit)
        {
            if (visit == null)
            {
                return BadRequest("No visit provided.");
            }
            if (visit.Patient_comment == null)
            {
                return BadRequest("The visits patient's comment cannot be null.");
            }
            if (visit.Fk_patient < 0)
            {
                return BadRequest("The visits patient's ID cannot be negative.");
            }
            if (visit.Fk_doctor < 0)
            {
                return BadRequest("The visits doctor's ID cannot be negative.");
            }
            if (visit.Fk_timetable < 0)
            {
                return BadRequest("The visits timetables's ID cannot be negative.");
            }
            if (visit.Fk_room == null)
            {
                return BadRequest("The visits room's name cannot be null.");
            }
            if (visit.Fk_procedure < 0)
            {
                return BadRequest("The visits procedure's ID cannot be negative.");
            }
            try
            {
                try
                {
                    Database.Connection.Open();
                }
                catch
                {
                    return StatusCode(500, "Could not establish a database connection.");
                }
                AddNewTimetable(visit.Fk_timetable, visit.Fk_doctor);
                if (visit.Patient_history != null)
                {
                    AddHistory(visit);
                }

                try
                {
                    UpdateDebt(visit.Fk_patient, visit.Fk_procedure, -1, "Post");
                }
                catch
                {
                    return StatusCode(500, "Failed to update the debt.");
                }

                MySqlCommand cmd;
                cmd = Database.Connection.CreateCommand();
                cmd.CommandText = @"INSERT into visits (
                                        patient_comment,
                                        fk_patient,
                                        fk_doctor,
                                        fk_room,
                                        fk_timetable,
                                        fk_procedure,
                                        confirmed) 
                                    VALUES(
                                        @Patient_comment, 
                                        @Fk_patient, 
                                        @Fk_doctor, 
                                        @Fk_room,
                                        @Fk_timetable,
                                        @Fk_procedure,
                                        'no')";
                cmd.Parameters.AddWithValue("@Patient_comment", visit.Patient_comment);
                cmd.Parameters.AddWithValue("@Fk_patient", visit.Fk_patient);
                cmd.Parameters.AddWithValue("@Fk_doctor", visit.Fk_doctor);
                cmd.Parameters.AddWithValue("@Fk_room", visit.Fk_room);
                cmd.Parameters.AddWithValue("@Fk_timetable", visit.Fk_timetable);
                cmd.Parameters.AddWithValue("@Fk_procedure", visit.Fk_procedure);
                int code = cmd.ExecuteNonQuery();
                if (code == 0)
                {
                    return BadRequest("The information provided is incorrect.");
                }
                return Ok("The visit has been added successfully.");
            }
            catch
            {
                return StatusCode(500, "Failed to add the visit.");
            }

        }
        private void UpdateDebt(int patientID, int procedureID, int editProcedureID, string type)
        {
            Procedure procedure = new Procedure();
            Procedure editProcedure = new Procedure();
            Patient patient = new Patient();
            procedure = GetProcedure(procedureID);
            if (editProcedureID != -1)
            {
                editProcedure = GetProcedure(editProcedureID);
            }
            patient = GetPatient(patientID);
            MySqlCommand cmd;
            cmd = Database.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE patients
                                    SET debt=@Debt
                                    WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", patient.Id);
            if (type == "Post")
            {
                cmd.Parameters.AddWithValue("@Debt", patient.Debt + procedure.Price);
            }
            if (type == "Delete")
            {
                cmd.Parameters.AddWithValue("@Debt", patient.Debt - procedure.Price);
            }
            if (type == "Edit")
            {
                cmd.Parameters.AddWithValue("@Debt", patient.Debt - procedure.Price + editProcedure.Price);
            }
            int code = cmd.ExecuteNonQuery();
        }
        private Procedure GetProcedure(int id)
        {
            Procedure procedure = new Procedure();
            MySqlCommand cmd;
            cmd = Database.Connection.CreateCommand();
            cmd.CommandText = string.Format(@"SELECT procedures.id,
                                                     procedures.price
                                                  FROM `procedures`
                                                  WHERE procedures.id={0}", id);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    reader.Read();
                    procedure.Id = Convert.ToInt32(reader["id"]);
                    procedure.Price = Convert.ToDouble(reader["price"]);
                }
            }
            return procedure;
        }
        private Patient GetPatient(int id)
        {
            Patient patient = new Patient();
            MySqlCommand cmd;
            cmd = Database.Connection.CreateCommand();
            cmd.CommandText = string.Format(@"SELECT 
                                        patients.id,
                                        patient.email as email,
                                        patients.debt
                                    FROM `patients`
                                    LEFT JOIN users patient ON patients.fk_user=patient.id
                                    WHERE patients.id ={0}", id);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    reader.Read();
                    patient.Id = Convert.ToInt32(reader["id"]);
                    patient.Email = Convert.ToString(reader["email"]);
                    patient.Debt = Convert.ToDouble(reader["debt"]);
                }
            }
            return patient;
        }
        [Authorize]
        [Produces("application/json")]
        [HttpPut("{id}")]
        public IActionResult PutVisit(int id, [FromBody] Visit visit)
        {
            if (visit == null)
            {
                return BadRequest("No visit provided.");
            }
            if (visit.Patient_comment == null)
            {
                return BadRequest("The visits patient's comment cannot be null.");
            }
            if (visit.Fk_patient < 0)
            {
                return BadRequest("The visits patient's ID cannot be negative.");
            }
            if (visit.Fk_doctor < 0)
            {
                return BadRequest("The visits doctor's ID cannot be negative.");
            }
            if (visit.Fk_timetable < 0)
            {
                return BadRequest("The visits timetables's ID cannot be negative.");
            }
            if (visit.Fk_room == null)
            {
                return BadRequest("The visits room's name cannot be null.");
            }
            if (visit.Fk_procedure < 0)
            {
                return BadRequest("The visits procedure's ID cannot be negative.");
            }
            Visit currentVisit;
            try
            {
                MySqlCommand cmd;
                cmd = Database.Connection.CreateCommand();
                try
                {
                    Database.Connection.Open();
                }
                catch
                {
                    return StatusCode(500, "Could not establish a database connection.");
                }
                currentVisit = GetCurrentVisit(id);
                if (currentVisit.Fk_timetable == 0)
                {
                    return NotFound("The visit does not exist.");
                }
                try
                {
                    UpdateDebt(currentVisit.Fk_patient, currentVisit.Fk_procedure, visit.Fk_procedure, "Edit");
                }
                catch
                {
                    return StatusCode(500, "Failed to update the debt.");
                }
                try
                {
                    DeleteOldTimetable(id);
                }
                catch
                {
                    return StatusCode(500, "Failed to delete the timetable.");
                }
                try
                {
                    AddNewTimetable(visit.Fk_timetable, visit.Fk_doctor);
                }
                catch
                {
                    return StatusCode(500, "Failed to add the timetable.");
                }
                cmd.CommandText = @"UPDATE visits
                                    SET patient_comment=@Patient_comment,
                                        fk_patient=@Fk_patient,
                                        fk_doctor=@Fk_doctor,
                                        fk_room=@Fk_room,
                                        fk_timetable=@Fk_timetable,
                                        fk_procedure=@Fk_procedure,
                                        confirmed='no'
                                    WHERE id=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@Patient_comment", visit.Patient_comment);
                cmd.Parameters.AddWithValue("@Fk_patient", visit.Fk_patient);
                cmd.Parameters.AddWithValue("@Fk_doctor", visit.Fk_doctor);
                cmd.Parameters.AddWithValue("@Fk_room", visit.Fk_room);
                cmd.Parameters.AddWithValue("@Fk_timetable", visit.Fk_timetable);
                cmd.Parameters.AddWithValue("@Fk_procedure", visit.Fk_procedure);
                cmd.Parameters.AddWithValue("@Confirmed", visit.Confirmed);

                int code = cmd.ExecuteNonQuery();
                if (code == 0)
                {
                    return BadRequest("The visit does not exist or the information provided is incorrect.");
                }

                return Ok("The visit has been updated successfully.");
            }
            catch
            {
                return StatusCode(500, "Could not update the visit.");
            }
        }
        private Visit GetCurrentVisit(int id)
        {
            Visit visit = new Visit();
            MySqlCommand cmd;
            cmd = Database.Connection.CreateCommand();
            cmd.CommandText = string.Format(@"SELECT visits.id,
                                                     visits.fk_timetable,
                                                     visits.fk_doctor,
                                                     visits.fk_patient,
                                                     visits.fk_procedure
                                                  FROM `visits`
                                                  LEFT JOIN timetables ON visits.fk_timetable=timetables.id
                                                  WHERE visits.id={0}", id);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    reader.Read();
                    visit.Id = Convert.ToInt32(reader["id"]);
                    visit.Fk_timetable = Convert.ToInt32(reader["fk_timetable"]);
                    visit.Fk_doctor = Convert.ToInt32(reader["fk_doctor"]);
                    visit.Fk_patient = Convert.ToInt32(reader["fk_patient"]);
                    visit.Fk_procedure = Convert.ToInt32(reader["fk_procedure"]);
                }
            }
            return visit;
        }
        [Authorize]
        [Produces("application/json")]
        [HttpDelete("{id}")]
        public IActionResult DeleteVisit(int id)
        {
            Visit currentVisit;
            Patient currentPatient;
            Timetable timetableVisit;
            try
            {
                try
                {
                    Database.Connection.Open();
                }
                catch
                {
                    return StatusCode(500, "Could not establish a database connection.");
                }
                currentVisit = GetCurrentVisit(id);
                if (currentVisit.Fk_timetable == 0)
                {
                    return NotFound("The visit does not exist.");
                }
                try
                {
                    UpdateDebt(currentVisit.Fk_patient, currentVisit.Fk_procedure, -1, "Delete");
                }
                catch
                {
                    return StatusCode(500, "Failed to update the debt.");
                }
                try
                {
                    timetableVisit = GetTimetable(currentVisit.Fk_timetable);
                }
                catch
                {
                    return StatusCode(500, "Failed to update the debt.");
                }
                string isLockedTrim = "fk" + currentVisit.Fk_doctor + "doc";
                timetableVisit.IsLocked = timetableVisit.IsLocked.Replace(isLockedTrim, "");
                MySqlCommand cmd;
                cmd = Database.Connection.CreateCommand();
                cmd.CommandText = string.Format(@"DELETE 
                                                  FROM visits 
                                                  WHERE id = {0}", id);
                int code = cmd.ExecuteNonQuery();
                if (code == 0)
                {
                    return NotFound("The visit does not exist.");
                }

                cmd.CommandText = string.Format(@"UPDATE timetables
                                                  SET isLocked = '{0}'
                                                  WHERE id = {1}", timetableVisit.IsLocked, currentVisit.Fk_timetable);
                code = cmd.ExecuteNonQuery();
                if (code == 0)
                {
                    return BadRequest("The visit does not exist or the information provided is incorrect.");
                }
                try
                {
                    currentPatient = GetPatient(currentVisit.Fk_patient);
                }
                catch
                {
                    return StatusCode(500, "Failed to update the debt.");
                }
                cmd.CommandText = string.Format(@"ALTER TABLE visits AUTO_INCREMENT=1;", id);
                code = cmd.ExecuteNonQuery();
                SendDeletionEmail(currentPatient, timetableVisit);
                return Ok("The visit has been deleted successfully.");
            }
            catch
            {
                return StatusCode(500, "Failed to delete the visit.");
            }
        }
        public Timetable GetTimetable(int id)
        {
            Timetable timetable = new Timetable();
            MySqlCommand cmd;
            cmd = Database.Connection.CreateCommand();

            cmd.CommandText = string.Format(@"SELECT *
                                              FROM timetables 
                                              WHERE id = {0}", id);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    reader.Read();
                    timetable.Id = Convert.ToInt32(reader["id"]);
                    timetable.Date = Convert.ToString(reader["date"]);
                    timetable.Time = Convert.ToString(reader["time"]);
                    timetable.IsLocked = Convert.ToString(reader["isLocked"]);
                }
            }
            return timetable;
        }
        [Authorize(Roles = Role.Admin + "," + Role.Employee)]
        [Produces("application/json")]
        [HttpPatch("{id}")]
        public IActionResult ConfirmVisit(int id)
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

                cmd.CommandText = string.Format(@"UPDATE visits
                                                  SET confirmed='yes'
                                                  WHERE id = {0}", id);
                int code = cmd.ExecuteNonQuery();
                if (code == 0)
                {
                    return NotFound("The visit does not exist");
                }
                Visit visit = new Visit();
                try
                {
                    cmd = Database.Connection.CreateCommand();
                    cmd.CommandText = string.Format(@"SELECT 
                                        visits.id,
                                        visits.patient_comment,
                                        visits.fk_patient,
                                        visits.fk_doctor,
                                        visits.fk_room,
                                        visits.fk_timetable,
                                        visits.fk_procedure,
                                        visits.confirmed,
                                        patients.fk_user,
                                        patient.name as patient_name,
                                        patient.surname as patient_surname,
                                        patient.email as email,
                                        employees.fk_user,
                                        employee.name as doctor_name,
                                        employee.surname as doctor_surname,
                                        employees.specialization as specialization,
                                        timetables.date as date,
                                        timetables.time as time,
                                        timetables.isLocked as isLocked,
                                        procedures.name as procedure_name,
                                        procedures.information as procedure_information
                                    FROM `visits`
                                    LEFT JOIN patients ON visits.fk_patient=patients.id
                                    LEFT JOIN employees ON visits.fk_doctor=employees.id
                                    LEFT JOIN timetables ON visits.fk_timetable=timetables.id
                                    LEFT JOIN procedures ON visits.fk_procedure=procedures.id
                                    LEFT JOIN users employee ON employees.fk_user=employee.id
                                    LEFT JOIN users patient ON patients.fk_user=patient.id
                                                  WHERE visits.id = {0}", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reader.Read();
                            date = Convert.ToDateTime(reader["date"]);
                            date2 = date.ToString("yyyy-MM-dd");
                            visit.Id = Convert.ToInt32(reader["id"]);
                            visit.Date = date2;
                            visit.Time = Convert.ToString(reader["time"]);
                            visit.Patient_comment = Convert.ToString(reader["patient_comment"]);
                            visit.Patient_email = Convert.ToString(reader["email"]);
                            visit.Fk_patient = Convert.ToInt32(reader["fk_patient"]);
                            visit.Fk_doctor = Convert.ToInt32(reader["fk_doctor"]);
                            visit.Fk_timetable = Convert.ToInt32(reader["fk_timetable"]);
                            visit.Fk_room = Convert.ToString(reader["fk_room"]);
                            visit.Patient_name = Convert.ToString(reader["patient_name"]);
                            visit.Patient_surname = Convert.ToString(reader["patient_surname"]);
                            visit.Doctor_name = Convert.ToString(reader["doctor_name"]);
                            visit.Doctor_surname = Convert.ToString(reader["doctor_surname"]);
                            visit.Specialization = Convert.ToString(reader["specialization"]);
                            visit.Fk_procedure = Convert.ToInt32(reader["fk_procedure"]);
                            visit.Procedure_information = Convert.ToString(reader["procedure_information"]);
                            visit.Procedure_name = Convert.ToString(reader["procedure_name"]);
                            visit.Confirmed = Convert.ToString(reader["confirmed"]);
                        }
                    }
                }
                catch
                {
                    return StatusCode(500, "Could not update the visit.");
                }
                SendConfirmationEmail(visit);
                return Ok("The visit has been confirmed successfully.");
            }
            catch
            {
                return StatusCode(500, "Could not update the visit.");
            }
        }
        private void SendConfirmationEmail(Visit visit)
        {

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("info.clinicapi@yahoo.com"));
            email.To.Add(MailboxAddress.Parse(visit.Patient_email));
            email.Subject = "Clinic visit confirmed";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text =
                      $@"<h4>Good day!</h4>
                         <p>Your visit has been confirmed!</p>
                         <p>Doctor: {visit.Doctor_name} {visit.Doctor_surname}</p>
                         <p>Room: {visit.Fk_room}</p>
                         <p>Date: {visit.Date}</p>
                         <p>Time: {visit.Time}</p>
                         <p>Procedure: {visit.Procedure_name}</p>
                         <p>Your comment: {visit.Patient_comment}</p>"

            };
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.mail.yahoo.com", 587);
            smtp.Authenticate("info.clinicapi@yahoo.com", "ogtaumtcrikpzcwl");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        private void SendDeletionEmail(Patient patient, Timetable timetable)
        {

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("info.clinicapi@yahoo.com"));
            email.To.Add(MailboxAddress.Parse(patient.Email));
            email.Subject = "Clinic visit cancelled";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text =
                      $@"<h4>Update on your visit:</h4>
                         <p>Your visit has been cancelled!</p>
                         <p>Time: {timetable.Date} {timetable.Time}</p>
                         <p>We apologise for any inconvenience this may cause.</p>"

            };
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.mail.yahoo.com", 587);
            smtp.Authenticate("info.clinicapi@yahoo.com", "ogtaumtcrikpzcwl");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        [Authorize(Roles = Role.Admin)]
        [Produces("application/json")]
        [HttpPatch("+{id}")]
        public IActionResult ChangeDoctor(int id, [FromBody] Visit visit)
        {
            if (visit == null)
            {
                return BadRequest("No visit provided.");
            }
            if (visit.Fk_doctor < 0)
            {
                return BadRequest("The visits doctor's ID cannot be negative.");
            }

            try
            {
                MySqlCommand cmd;
                cmd = Database.Connection.CreateCommand();
                try
                {
                    Database.Connection.Open();
                }
                catch
                {
                    return StatusCode(500, "Could not establish a database connection.");
                }
                try
                {
                    DeleteOldTimetable(id);
                }
                catch
                {
                    return StatusCode(500, "Failed to delete the timetable.");
                }
                try
                {
                    AddNewTimetable(visit.Fk_timetable, visit.Fk_doctor);
                }
                catch
                {
                    return StatusCode(500, "Failed to add the timetable.");
                }
                cmd.CommandText = @"UPDATE visits
                                    SET fk_doctor=@Fk_doctor,
                                        fk_timetable=@Fk_timetable,
                                        confirmed='no'
                                    WHERE id=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@Fk_doctor", visit.Fk_doctor);
                cmd.Parameters.AddWithValue("@Fk_timetable", visit.Fk_timetable);

                int code = cmd.ExecuteNonQuery();

                if (code == 0)
                {
                    return BadRequest("The visit does not exist or the information provided is incorrect.");
                }

                return Ok("The doctor has been changed successfully.");
            }
            catch
            {
                return StatusCode(500, "Could not update the visit.");
            }
        }
        private void AddNewTimetable(int fk_timetable, int fk_doctor)
        {
            MySqlCommand cmd;
            cmd = Database.Connection.CreateCommand();
            int error = 0;
            Timetable timetable = null;
            try
            {
                timetable = GetTimetable(fk_timetable);
            }
            catch
            {
                error = 1;
            }
            string IsLocked = "";
            if (error != 1)
            {
                IsLocked = timetable.IsLocked + "fk" + fk_doctor + "doc";
            }
            else
            {
                IsLocked = "fk" + fk_doctor + "doc";
            }
            cmd.CommandText = string.Format(@"UPDATE timetables
                                                  SET isLocked = @IsLocked
                                                  WHERE id = {1}", IsLocked, fk_timetable);
            cmd.Parameters.AddWithValue("@IsLocked", IsLocked);
            int code = cmd.ExecuteNonQuery();
        }
        private void DeleteOldTimetable(int id)
        {
            Visit currentVisit;
            currentVisit = GetCurrentVisit(id);
            Timetable timetableVisit;
            timetableVisit = GetTimetable(currentVisit.Fk_timetable);
            string isLockedTrim = "fk" + currentVisit.Fk_doctor + "doc";
            timetableVisit.IsLocked = timetableVisit.IsLocked.Replace(isLockedTrim, "");
            MySqlCommand cmd;
            cmd = Database.Connection.CreateCommand();
            cmd.CommandText = string.Format(@"UPDATE timetables
                                                  SET isLocked = '{0}'
                                                  WHERE id = {1}", timetableVisit.IsLocked, currentVisit.Fk_timetable);
            int code = cmd.ExecuteNonQuery();
        }
        private void AddHistory(Visit visit)
        {
            MySqlCommand cmd;
            cmd = Database.Connection.CreateCommand();
            date = DateTime.Now;
            date2 = date.ToString("yyyy-MM-dd");
            cmd.CommandText = @"INSERT into histories (
                                        date,
                                        name,
                                        description,
                                        fk_patient) 
                                    VALUES(
                                        @Date,
                                        @Name, 
                                        @Description, 
                                        @Fk_patient)";
            cmd.Parameters.AddWithValue("@Date", date2);
            cmd.Parameters.AddWithValue("@Name", visit.Patient_history);
            cmd.Parameters.AddWithValue("@Description", visit.Patient_description);
            cmd.Parameters.AddWithValue("@Fk_patient", visit.Fk_patient);
            int code = cmd.ExecuteNonQuery();
        }
        [HttpGet("~")]
        [Produces("application/json")]
        [Authorize]
        public IActionResult GetActiveVisits()
        {
            try
            {
                Database.Connection.Open();
            }
            catch
            {
                return StatusCode(500, "Could not establish a database connection.");
            }
            int timetableResult = -1;
            try
            {
                timetableResult = GetTimetable();
            }
            catch
            {
                return StatusCode(500, "Failed to get the timetable.");
            }
            List<Visit> visits = new List<Visit>();
            try
            {
                MySqlCommand cmd;

                cmd = Database.Connection.CreateCommand();
                cmd.CommandText = string.Format(@"SELECT 
                                        visits.id,
                                        visits.patient_comment,
                                        visits.fk_patient,
                                        visits.fk_doctor,
                                        visits.fk_room,
                                        visits.fk_timetable,
                                        visits.fk_procedure,
                                        visits.confirmed,
                                        patients.fk_user,
                                        patient.name as patient_name,
                                        patient.surname as patient_surname,
                                        employees.fk_user,
                                        employee.name as doctor_name,
                                        employee.surname as doctor_surname,
                                        employees.specialization as specialization,
                                        timetables.date as date,
                                        timetables.time as time,
                                        timetables.isLocked as isLocked,
                                        procedures.name as procedure_name,
                                        procedures.information as procedure_information
                                    FROM `visits`
                                    LEFT JOIN patients ON visits.fk_patient=patients.id
                                    LEFT JOIN employees ON visits.fk_doctor=employees.id
                                    LEFT JOIN timetables ON visits.fk_timetable=timetables.id
                                    LEFT JOIN procedures ON visits.fk_procedure=procedures.id
                                    LEFT JOIN users employee ON employees.fk_user=employee.id
                                    LEFT JOIN users patient ON patients.fk_user=patient.id
                                    WHERE visits.fk_timetable > {0}
                                    ORDER BY timetables.date, timetables.time;", timetableResult);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        date = Convert.ToDateTime(reader["date"]);
                        date2 = date.ToString("yyyy-MM-dd");
                        visits.Add(new Visit()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Date = date2,
                            Time = Convert.ToString(reader["time"]),
                            IsLocked = Convert.ToString(reader["isLocked"]),
                            Patient_comment = Convert.ToString(reader["patient_comment"]),
                            Fk_patient = Convert.ToInt32(reader["fk_patient"]),
                            Fk_doctor = Convert.ToInt32(reader["fk_doctor"]),
                            Fk_timetable = Convert.ToInt32(reader["fk_timetable"]),
                            Fk_room = Convert.ToString(reader["fk_room"]),
                            Patient_name = Convert.ToString(reader["patient_name"]),
                            Patient_surname = Convert.ToString(reader["patient_surname"]),
                            Doctor_name = Convert.ToString(reader["doctor_name"]),
                            Doctor_surname = Convert.ToString(reader["doctor_surname"]),
                            Specialization = Convert.ToString(reader["specialization"]),
                            Fk_procedure = Convert.ToInt32(reader["fk_procedure"]),
                            Procedure_information = Convert.ToString(reader["procedure_information"]),
                            Procedure_name = Convert.ToString(reader["procedure_name"]),
                            Confirmed = Convert.ToString(reader["confirmed"])
                        });
                    }
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the visits.");
            }

            return Json(visits);
        }
        private int GetTimetable()
        {
            int result = 0;
            List<Timetable> timetables = new List<Timetable>();
            MySqlCommand cmd;
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
            DateTime currentTime = DateTime.Now;
            for (int i = 0; i < timetables.Count; i++)
            {
                string[] dateFormat = timetables[i].Date.Split("-");
                string[] timeFormat = timetables[i].Time.Split(":");
                DateTime compare = new DateTime(Int32.Parse(dateFormat[0]), Int32.Parse(dateFormat[1]), Int32.Parse(dateFormat[2]), Int32.Parse(timeFormat[0]), 0, 0);
                if (currentTime.CompareTo(compare) == 1)
                {
                    result = i + 1;
                }
            }
            return result;
        }
        [HttpGet("*{search}")]
        [Produces("application/json")]
        [Authorize]
        public IActionResult GetActiveVisitsSearch(string search)
        {
            try
            {
                Database.Connection.Open();
            }
            catch
            {
                return StatusCode(500, "Could not establish a database connection.");
            }
            int timetableResult = -1;
            try
            {
                timetableResult = GetTimetable();
            }
            catch
            {
                return StatusCode(500, "Failed to get the timetable.");
            }
            List<Visit> visits = new List<Visit>();
            try
            {
                MySqlCommand cmd;

                cmd = Database.Connection.CreateCommand();
                cmd.CommandText = string.Format(@"SELECT 
                                        visits.id,
                                        visits.patient_comment,
                                        visits.fk_patient,
                                        visits.fk_doctor,
                                        visits.fk_room,
                                        visits.fk_timetable,
                                        visits.fk_procedure,
                                        visits.confirmed,
                                        patients.fk_user,
                                        patient.name as patient_name,
                                        patient.surname as patient_surname,
                                        employees.fk_user,
                                        employee.name as doctor_name,
                                        employee.surname as doctor_surname,
                                        employees.specialization as specialization,
                                        timetables.date as date,
                                        timetables.time as time,
                                        timetables.isLocked as isLocked,
                                        procedures.name as procedure_name,
                                        procedures.information as procedure_information
                                    FROM `visits`
                                    LEFT JOIN patients ON visits.fk_patient=patients.id
                                    LEFT JOIN employees ON visits.fk_doctor=employees.id
                                    LEFT JOIN timetables ON visits.fk_timetable=timetables.id
                                    LEFT JOIN procedures ON visits.fk_procedure=procedures.id
                                    LEFT JOIN users employee ON employees.fk_user=employee.id
                                    LEFT JOIN users patient ON patients.fk_user=patient.id
                                              WHERE (date LIKE '%{0}%' 
                                              OR time LIKE '%{0}%'
                                              OR patient.name LIKE '%{0}%'
                                              OR patient.surname LIKE '%{0}%'
                                              OR employee.name LIKE '%{0}%'
                                              OR employee.surname LIKE '%{0}%'
                                              OR visits.fk_room LIKE '%{0}%'
                                              OR employees.specialization LIKE '%{0}%'
                                              OR procedures.name LIKE '%{0}%')
											  AND visits.fk_timetable > {1}
											  ORDER BY timetables.date, timetables.time;", search, timetableResult);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        date = Convert.ToDateTime(reader["date"]);
                        date2 = date.ToString("yyyy-MM-dd");
                        visits.Add(new Visit()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Date = date2,
                            Time = Convert.ToString(reader["time"]),
                            Patient_comment = Convert.ToString(reader["patient_comment"]),
                            Fk_patient = Convert.ToInt32(reader["fk_patient"]),
                            Fk_doctor = Convert.ToInt32(reader["fk_doctor"]),
                            Fk_timetable = Convert.ToInt32(reader["fk_timetable"]),
                            Fk_room = Convert.ToString(reader["fk_room"]),
                            Patient_name = Convert.ToString(reader["patient_name"]),
                            Patient_surname = Convert.ToString(reader["patient_surname"]),
                            Doctor_name = Convert.ToString(reader["doctor_name"]),
                            Doctor_surname = Convert.ToString(reader["doctor_surname"]),
                            Specialization = Convert.ToString(reader["specialization"]),
                            Fk_procedure = Convert.ToInt32(reader["fk_procedure"]),
                            Procedure_information = Convert.ToString(reader["procedure_information"]),
                            Procedure_name = Convert.ToString(reader["procedure_name"]),
                            Confirmed = Convert.ToString(reader["confirmed"])
                        });
                    }
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the visits.");
            }

            return Json(visits);
        }
    }
}