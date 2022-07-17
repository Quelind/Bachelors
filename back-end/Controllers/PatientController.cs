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
    [Route("api/patient")]
    [ApiController]
    public class PatientController : Controller
    {
        private AppDatabase Database { get; set; }
        public PatientController(AppDatabase database)
        {
            Database = database;
        }
        [HttpGet]
        [Authorize(Roles = Role.Admin + "," + Role.Employee)]
        public IActionResult GetPatient()
        {
            List<Patient> patients = new List<Patient>();
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
                                        patients.id,
                                        users.name as name,
                                        users.surname as surname,
                                        users.birthdate as birthdate,
                                        users.phone as phone,
                                        users.email as email,
                                        patients.information,
                                        patients.fk_user,
                                        patients.debt
                                    FROM `patients`
                                    LEFT JOIN users ON patients.fk_user=users.id";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        patients.Add(new Patient()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["name"]),
                            Surname = Convert.ToString(reader["surname"]),
                            Information = Convert.ToString(reader["information"]),
                            Birthdate = Convert.ToString(reader["birthdate"]),
                            Phone = Convert.ToString(reader["phone"]),
                            Email = Convert.ToString(reader["email"]),
                            Fk_user = Convert.ToInt32(reader["fk_user"]),
                            Debt = Convert.ToDouble(reader["debt"])
                        });
                    }
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the patients.");
            }
            return Json(patients);
        }
        [Produces("application/json")]
        [Authorize(Roles = Role.Admin + "," + Role.Employee)]
        [HttpGet("{id}")]
        public IActionResult GetPatient(int id)
        {
            Patient patient = new Patient();
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
                                                    patients.id,
                                                    users.name as name,
                                                    users.surname as surname,
                                                    users.birthdate as birthdate,
                                                    users.phone as phone,
                                                    users.email as email,
                                                    patients.information,
                                                    patients.fk_user,
                                                    patients.debt
                                              FROM `patients`
                                              LEFT JOIN users ON patients.fk_user = users.id
                                              WHERE patients.id = {0}", id);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        patient.Id = Convert.ToInt32(reader["id"]);
                        patient.Name = Convert.ToString(reader["name"]);
                        patient.Surname = Convert.ToString(reader["surname"]);
                        patient.Information = Convert.ToString(reader["information"]);
                        patient.Birthdate = Convert.ToString(reader["birthdate"]);
                        patient.Phone = Convert.ToString(reader["phone"]);
                        patient.Email = Convert.ToString(reader["email"]);
                        patient.Fk_user = Convert.ToInt32(reader["fk_user"]);
                        patient.Debt = Convert.ToDouble(reader["debt"]);
                    }
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the patient.");
            }
            return Json(patient);
        }
        [Authorize(Roles = Role.Admin + "," + Role.Employee)]
        [Produces("application/json")]
        [HttpGet("={search}")]
        public IActionResult PatientSearch(string search)
        {
            List<Patient> patients = new List<Patient>();
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
                                        patients.id,
                                        users.name as name,
                                        users.surname as surname,
                                        users.birthdate as birthdate,
                                        users.phone as phone,
                                        users.email as email,
                                        patients.information,
                                        patients.fk_user,
                                        patients.debt
                                    FROM `patients`
                                    LEFT JOIN users ON patients.fk_user=users.id
                                              WHERE users.name LIKE '%{0}%' 
                                              OR users.surname LIKE '%{0}%'
                                              OR patients.information LIKE '%{0}%'", search);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        patients.Add(new Patient()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["name"]),
                            Surname = Convert.ToString(reader["surname"]),
                            Information = Convert.ToString(reader["information"]),
                            Birthdate = Convert.ToString(reader["birthdate"]),
                            Phone = Convert.ToString(reader["phone"]),
                            Email = Convert.ToString(reader["email"]),
                            Fk_user = Convert.ToInt32(reader["fk_user"]),
                            Debt = Convert.ToDouble(reader["debt"])
                        });
                    }
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the patient.");
            }
            return Json(patients);
        }
        [Produces("application/json")]
        [Authorize(Roles = Role.Admin + "," + Role.Employee)]
        [HttpPost]
        public IActionResult PostPatient([FromBody] Patient patient)
        {
            if (patient == null)
            {
                return BadRequest("No patient provided.");
            }
            if (patient.Name == null)
            {
                return BadRequest("The patient's name cannot be null.");
            }
            if (patient.Surname == null)
            {
                return BadRequest("The patient's surname cannot be null.");
            }
            if (patient.Information == null)
            {
                return BadRequest("The patient's information cannot be null.");
            }
            if (patient.Birthdate == null)
            {
                return BadRequest("The patient's birthdate cannot be null.");
            }
            if (patient.Phone == null)
            {
                return BadRequest("The patient's phone cannot be null.");
            }
            if (patient.Email == null)
            {
                return BadRequest("The patient's email cannot be null.");
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
                    PostUser(patient);
                }
                catch
                {
                    return StatusCode(500, "Failed to add the user.");
                }
                int id;
                try
                {
                    id = GetNewestUserID();
                }
                catch
                {
                    return StatusCode(500, "Failed to get the user.");
                }
                cmd.CommandText = @"INSERT into patients (
                                        information,
                                        fk_user,
                                        debt) 
                                    VALUES(
                                        @Information, 
                                        @Fk_user,
                                         '0')";
                cmd.Parameters.AddWithValue("@Information", patient.Information);
                cmd.Parameters.AddWithValue("@Fk_user", id);
                int code = cmd.ExecuteNonQuery();
                if (code == 0)
                {
                    return BadRequest("The information provided is incorrect.");
                }
                return Ok("The patient has been added successfully.");
            }
            catch
            {
                return StatusCode(500, "Failed to add the patient.");
            }

        }
        [Produces("application/json")]
        [Authorize(Roles = Role.Admin + "," + Role.Employee)]
        [HttpPut("{id}")]
        public IActionResult PutPatient(int id, [FromBody] Patient patient)
        {
            if (patient == null)
            {
                return BadRequest("No patient provided.");
            }
            if (patient.Name == null)
            {
                return BadRequest("The patient's name cannot be null.");
            }
            if (patient.Surname == null)
            {
                return BadRequest("The patient's surname cannot be null.");
            }
            if (patient.Information == null)
            {
                return BadRequest("The patient's information cannot be null.");
            }
            if (patient.Birthdate == null)
            {
                return BadRequest("The patient's birthdate cannot be null.");
            }
            if (patient.Phone == null)
            {
                return BadRequest("The patient's phone cannot be null.");
            }
            if (patient.Email == null)
            {
                return BadRequest("The patient's email cannot be null.");
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
                try
                {
                    PatchUser(patient);
                }
                catch
                {
                    return StatusCode(500, "Could not update the user.");
                }
                MySqlCommand cmd;
                cmd = Database.Connection.CreateCommand();
                cmd.CommandText = @"UPDATE patients
                                    SET information=@Information
                                    WHERE id=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@Information", patient.Information);

                int code = cmd.ExecuteNonQuery();
                if (code == 0)
                {
                    return BadRequest("The patient does not exist or the information provided is incorrect.");
                }
                return Ok("The patient has been updated successfully.");
            }
            catch
            {
                return StatusCode(500, "Could not update the patient.");
            }
        }
        private void PatchUser(Patient user)
        {
            MySqlCommand cmd;
            cmd = Database.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE users
                                    SET name=@Name,
                                        surname=@Surname,
                                        birthdate=@Birthdate,
                                        phone=@Phone,
                                        email=@Email
                                    WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", user.Fk_user);
            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Surname", user.Surname);
            cmd.Parameters.AddWithValue("@Birthdate", user.Birthdate);
            cmd.Parameters.AddWithValue("@Phone", user.Phone);
            cmd.Parameters.AddWithValue("@Email", user.Email);

            int code = cmd.ExecuteNonQuery();
        }
        [Authorize(Roles = Role.Admin + "," + Role.Employee)]
        [Produces("application/json")]
        [HttpDelete("{id}")]
        public IActionResult DeletePatient(int id)
        {
            int fk_user = -1;
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
                try
                {
                    fk_user = GetPatientsUser(id);
                }
                catch
                {
                    return StatusCode(500, "Failed to get the user.");
                }
                cmd = Database.Connection.CreateCommand();
                cmd.CommandText = string.Format(@"DELETE 
                                                  FROM patients 
                                                  WHERE id = {0}", id);
                int code = cmd.ExecuteNonQuery();
                if (code == 0)
                {
                    return NotFound("The patient does not exist.");
                }
                cmd.CommandText = string.Format(@"ALTER TABLE patients AUTO_INCREMENT=1;", id);
                code = cmd.ExecuteNonQuery();
                try
                {
                    DeleteUser(fk_user);
                }
                catch
                {
                    return StatusCode(500, "Failed to delete the patient.");
                }
                return Ok("The patient has been deleted successfully.");
            }
            catch
            {
                return StatusCode(500, "Failed to delete the patient.");
            }
        }
        private void DeleteUser(int id)
        {
            MySqlCommand cmd;
            cmd = Database.Connection.CreateCommand();
            cmd.CommandText = string.Format(@"DELETE 
                                                  FROM users 
                                                  WHERE id = {0}", id);
            int code = cmd.ExecuteNonQuery();
        }
        private int GetPatientsUser(int id)
        {
            int fk_user = -1;
            MySqlCommand cmd;
            cmd = Database.Connection.CreateCommand();
            cmd.CommandText = string.Format(@"SELECT 
                                                    patients.fk_user
                                              FROM `patients`
                                              WHERE patients.id = {0}", id);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    fk_user = Convert.ToInt32(reader["fk_user"]);
                }
            }
            return fk_user;
        }
        private int GetNewestUserID()
        {
            int id = -1;
            MySqlCommand cmd;
            cmd = Database.Connection.CreateCommand();
                cmd = Database.Connection.CreateCommand();
                cmd.CommandText = string.Format(@"SELECT id FROM users WHERE id = ( SELECT MAX(id) FROM users )");
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    id = Convert.ToInt32(reader["id"]);
                }
            return id;
        }
        private void PostUser(Patient user)
        {
            var rand = new Random();
            user.Password = "password" + rand.Next(100000, 999999).ToString();
            user.Username = (user.Name.Substring(0, 3) + user.Surname.Substring(0, 3)).ToLower() + rand.Next(100, 999).ToString();
            SendEmail(user);
            MySqlCommand cmd;
            cmd = Database.Connection.CreateCommand();
            cmd.CommandText = @"INSERT into users (
                                        name,
                                        surname,
                                        username,
                                        password,
                                        birthdate,
                                        phone,
                                        email,
                                        role,
                                        token,
                                        verified) 
                                    VALUES(
                                        @Name, 
                                        @Surname,
                                        @Username,
                                        @Password,
                                        @Birthdate, 
                                        @Phone, 
                                        @Email,
                                        'User',
                                        'NULL',
                                        'true')";
            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Surname", user.Surname);
            cmd.Parameters.AddWithValue("@Username", user.Username);
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@Birthdate", user.Birthdate);
            cmd.Parameters.AddWithValue("@Phone", user.Phone);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            int code = cmd.ExecuteNonQuery();
        }
        private void SendEmail(Patient user)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("info.clinicapi@yahoo.com"));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Clinic Account created";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text =
                      $@"<h4>Account created</h4>
                         <p>You have been registered!</p>
                         <p>Use these credentials to login:</p>
                         <p><b>{user.Username}</b></p>
                         <p><b>{user.Password}</b></p>"
            };
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.mail.yahoo.com", 587);
            smtp.Authenticate("info.clinicapi@yahoo.com", "ogtaumtcrikpzcwl");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        [Produces("application/json")]
        [Authorize]
        [HttpPatch("{id}")]
        public IActionResult EraseDebt(int id)
        {
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
                MySqlCommand cmd;
                cmd = Database.Connection.CreateCommand();
                cmd.CommandText = @"UPDATE patients
                                    SET debt=0
                                    WHERE id=@id";
                cmd.Parameters.AddWithValue("@id", id);

                int code = cmd.ExecuteNonQuery();
                if (code == 0)
                {
                    return BadRequest("The patient does not exist or the information provided is incorrect.");
                }
                return Ok("The patient has been updated successfully.");
            }
            catch
            {
                return StatusCode(500, "Could not update the patient.");
            }
        }
        [Produces("application/json")]
        [HttpGet("+{id}")]
        [Authorize(Roles = Role.Admin)]
        public IActionResult RequestPayment(int id)
        {
            Patient patient = new Patient();
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
                                                    patients.id,
                                                    users.name as name,
                                                    users.surname as surname,
                                                    users.birthdate as birthdate,
                                                    users.phone as phone,
                                                    users.email as email,
                                                    patients.information,
                                                    patients.fk_user,
                                                    patients.debt
                                              FROM `patients`
                                              LEFT JOIN users ON patients.fk_user = users.id
                                              WHERE patients.id = {0}", id);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        patient.Id = Convert.ToInt32(reader["id"]);
                        patient.Name = Convert.ToString(reader["name"]);
                        patient.Surname = Convert.ToString(reader["surname"]);
                        patient.Information = Convert.ToString(reader["information"]);
                        patient.Birthdate = Convert.ToString(reader["birthdate"]);
                        patient.Phone = Convert.ToString(reader["phone"]);
                        patient.Email = Convert.ToString(reader["email"]);
                        patient.Fk_user = Convert.ToInt32(reader["fk_user"]);
                        patient.Debt = Convert.ToDouble(reader["debt"]);
                    }
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the patient.");
            }
            SendRequestPaymentEmail(patient);
            return Ok(patient);
        }
        private void SendRequestPaymentEmail(Patient patient)
        {

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("info.clinicapi@yahoo.com"));
            email.To.Add(MailboxAddress.Parse(patient.Email));
            email.Subject = "Clinic requested payment";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text =
                      $@"<h4>Good day!</h4>
                         <p>The clinic has requested you to pay your tab</p>
                         <p>Please use this link to pay:</p>
                         <p><a href=""http://localhost:8080/Payments"">Payments</a></p>"

            };
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.mail.yahoo.com", 587);
            smtp.Authenticate("info.clinicapi@yahoo.com", "ogtaumtcrikpzcwl");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        [Produces("application/json")]
        [Authorize]
        [HttpGet("~{id}")]
        public IActionResult GetPatientByUser(int id)
        {
            Patient patient = new Patient();
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
                                                    patients.id,
                                                    users.name as name,
                                                    users.surname as surname,
                                                    users.birthdate as birthdate,
                                                    users.phone as phone,
                                                    users.email as email,
                                                    patients.information,
                                                    patients.fk_user,
                                                    patients.debt
                                              FROM `patients`
                                              LEFT JOIN users ON patients.fk_user = users.id
                                              WHERE patients.fk_user = {0}", id);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        patient.Id = Convert.ToInt32(reader["id"]);
                        patient.Name = Convert.ToString(reader["name"]);
                        patient.Surname = Convert.ToString(reader["surname"]);
                        patient.Information = Convert.ToString(reader["information"]);
                        patient.Birthdate = Convert.ToString(reader["birthdate"]);
                        patient.Phone = Convert.ToString(reader["phone"]);
                        patient.Email = Convert.ToString(reader["email"]);
                        patient.Fk_user = Convert.ToInt32(reader["fk_user"]);
                        patient.Debt = Convert.ToDouble(reader["debt"]);
                    }
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the patient.");
            }
            return Json(patient);
        }
    }
}
