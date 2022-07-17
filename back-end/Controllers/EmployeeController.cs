using ClinicAPI.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ClinicAPI.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private AppDatabase Database { get; set; }
        public EmployeeController(AppDatabase database)
        {
            Database = database;
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetEmployee()
        {
            List<Employee> employees = new List<Employee>();
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
                                        employees.id,
                                        users.name as name,
                                        users.surname as surname,
                                        users.birthdate as birthdate,
                                        users.phone as phone,
                                        users.email as email,
                                        employees.specialization,
                                        employees.fk_user,
                                        employees.fk_room,
                                        employees.image
                                    FROM `employees`
                                    LEFT JOIN users ON employees.fk_user=users.id";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["name"]),
                            Surname = Convert.ToString(reader["surname"]),
                            Specialization = Convert.ToString(reader["specialization"]),
                            Birthdate = Convert.ToString(reader["birthdate"]),
                            Phone = Convert.ToString(reader["phone"]),
                            Email = Convert.ToString(reader["email"]),
                            Fk_user = Convert.ToInt32(reader["fk_user"]),
                            Fk_room = Convert.ToString(reader["fk_room"]),
                            Image = Convert.ToString(reader["image"])
                        });
                    }
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the employee.");
            }
            return Json(employees);
        }
        [Authorize]
        [Produces("application/json")]
        [HttpGet("={search}")]
        public IActionResult EmployeeSearch(string search)
        {
            List<Employee> employees = new List<Employee>();
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
                                        employees.id,
                                        users.name as name,
                                        users.surname as surname,
                                        users.birthdate as birthdate,
                                        users.phone as phone,
                                        users.email as email,
                                        employees.specialization,
                                        employees.fk_user,
                                        employees.fk_room,
                                        employees.image
                                    FROM `employees`
                                    LEFT JOIN users ON employees.fk_user=users.id
                                              WHERE users.name LIKE '%{0}%' 
                                              OR users.surname LIKE '%{0}%'
                                              OR employees.specialization LIKE '%{0}%'", search);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["name"]),
                            Surname = Convert.ToString(reader["surname"]),
                            Specialization = Convert.ToString(reader["specialization"]),
                            Birthdate = Convert.ToString(reader["birthdate"]),
                            Phone = Convert.ToString(reader["phone"]),
                            Email = Convert.ToString(reader["email"]),
                            Fk_user = Convert.ToInt32(reader["fk_user"]),
                            Fk_room = Convert.ToString(reader["fk_room"]),
                            Image = Convert.ToString(reader["image"])
                        });
                    }
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the employee.");
            }
            return Json(employees);
        }
        [Authorize]
        [Produces("application/json")]
        [HttpGet("{id}")]
        public IActionResult GetEmployee(int id)
        {
            Employee employee = new Employee();
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
                                                    employees.id,
                                                    users.name as name,
                                                    users.surname as surname,
                                                    users.birthdate as birthdate,
                                                    users.phone as phone,
                                                    users.email as email,
                                                    employees.specialization,
                                                    employees.fk_user,
                                                    employees.fk_room,
                                                    employees.image
                                              FROM `employees`
                                              LEFT JOIN users ON employees.fk_user=users.id 
                                              WHERE employees.id = {0}", id);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employee.Id = Convert.ToInt32(reader["id"]);
                        employee.Name = Convert.ToString(reader["name"]);
                        employee.Surname = Convert.ToString(reader["surname"]);
                        employee.Birthdate = Convert.ToString(reader["birthdate"]);
                        employee.Phone = Convert.ToString(reader["phone"]);
                        employee.Email = Convert.ToString(reader["email"]);
                        employee.Specialization = Convert.ToString(reader["specialization"]);
                        employee.Fk_user = Convert.ToInt32(reader["fk_user"]);
                        employee.Fk_room = Convert.ToString(reader["fk_room"]);
                        employee.Image = Convert.ToString(reader["image"]);
                    }
                }
            }

            catch
            {
                return StatusCode(500, "Failed to get the employee.");
            }
            return Json(employee);
        }
        [Produces("application/json")]
        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        public IActionResult PostEmployee([FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("No employee provided.");
            }
            if (employee.Name == null)
            {
                return BadRequest("The employee's name cannot be null.");
            }
            if (employee.Surname == null)
            {
                return BadRequest("The employee's surname cannot be null.");
            }
            if (employee.Specialization == null)
            {
                return BadRequest("The employee's specialization cannot be null.");
            }
            if (employee.Birthdate == null)
            {
                return BadRequest("The employee's birthdate cannot be null.");
            }
            if (employee.Phone == null)
            {
                return BadRequest("The employee's phone cannot be null.");
            }
            if (employee.Email == null)
            {
                return BadRequest("The employee's email cannot be null.");
            }
            if (employee.Fk_room == null)
            {
                return BadRequest("The employee's room cannot be null.");
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
                    PostUser(employee);
                }
                catch
                {
                    return StatusCode(500, "Failed to get add user.");
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
                cmd.CommandText = @"INSERT into employees (
                                        specialization,
                                        fk_room,
                                        fk_user,
                                        image) 
                                    VALUES(
                                        @Specialization,
                                        @Fk_room,
                                        @Fk_user,
                                        @Image)";
                cmd.Parameters.AddWithValue("@Specialization", employee.Specialization);
                cmd.Parameters.AddWithValue("@Fk_room", employee.Fk_room);
                cmd.Parameters.AddWithValue("@Fk_user", id);
                if (employee.Image != "")
                {
                    cmd.Parameters.AddWithValue("@Image", employee.Image);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Image", "https://i.imgur.com/5VjZS1K.png");
                }
                int code = cmd.ExecuteNonQuery();
                return Ok("The employee has been added successfully.");
            }
            catch
            {
                return StatusCode(500, "Failed to add the employee.");
            }

        }
        [Produces("application/json")]
        [Authorize(Roles = Role.Admin)]
        [HttpPut("{id}")]
        public IActionResult PutEmployee(int id, [FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("No employee provided.");
            }
            if (employee.Name == null)
            {
                return BadRequest("The employee's name cannot be null.");
            }
            if (employee.Surname == null)
            {
                return BadRequest("The employee's surname cannot be null.");
            }
            if (employee.Specialization == null)
            {
                return BadRequest("The employee's specialization cannot be null.");
            }
            if (employee.Birthdate == null)
            {
                return BadRequest("The employee's birthdate cannot be null.");
            }
            if (employee.Phone == null)
            {
                return BadRequest("The employee's phone cannot be null.");
            }
            if (employee.Email == null)
            {
                return BadRequest("The employee's email cannot be null.");
            }
            if (employee.Fk_room == null)
            {
                return BadRequest("The employee's room cannot be null.");
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
                    PatchUser(employee);
                }
                catch
                {
                    return StatusCode(500, "Could not update the user.");
                }
                cmd.CommandText = @"UPDATE employees
                                    SET specialization=@Specialization,
                                        fk_room=@Fk_room,
                                        image=@Image
                                    WHERE id=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@Specialization", employee.Specialization);
                cmd.Parameters.AddWithValue("@Fk_room", employee.Fk_room);
                if (employee.Image != "")
                {
                    cmd.Parameters.AddWithValue("@Image", employee.Image);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Image", "https://i.imgur.com/5VjZS1K.png");
                }

                int code = cmd.ExecuteNonQuery();
                if (code == 0)
                {
                    return BadRequest("The employee does not exist or the information provided is incorrect.");
                }
                return Ok("The employee has been updated successfully.");
            }
            catch
            {
                return StatusCode(500, "Could not update the employee.");
            }
        }
        private void PatchUser(Employee user)
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
        [Authorize]
        [Produces("application/json")]
        [HttpGet("+{id}")]
        public IActionResult GetRoomByEmployee(int id)
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


                cmd.CommandText = string.Format(@"SELECT
                                              rooms.name,
                                              rooms.type,
                                              rooms.capacity,
                                              employees.fk_room,
                                              employees.id
                                              FROM rooms
                                              LEFT JOIN employees ON rooms.name = employees.fk_room
                                              WHERE employees.id = {0}", id);

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
        [Produces("application/json")]
        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
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
                int fk_user = -1;
                try
                {
                    fk_user = GetEmployeesUser(id);
                }
                catch
                {
                    return StatusCode(500, "Failed to get the user.");
                }
                cmd = Database.Connection.CreateCommand();
                cmd.CommandText = string.Format(@"DELETE 
                                                  FROM employees 
                                                  WHERE id = {0}", id);
                int code = cmd.ExecuteNonQuery();
                cmd.CommandText = string.Format(@"ALTER TABLE employees AUTO_INCREMENT=1;", id);
                code = cmd.ExecuteNonQuery();
                try
                {
                    DeleteUser(fk_user);
                }
                catch
                {
                    return StatusCode(500, "Failed to delete the user.");
                }
                return Ok("The employee has been deleted successfully.");
            }
            catch
            {
                return StatusCode(500, "Failed to delete the employee.");
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
        private int GetEmployeesUser(int id)
        {
            int fk_user = -1;
            MySqlCommand cmd;
            cmd = Database.Connection.CreateCommand();
            cmd.CommandText = string.Format(@"SELECT 
                                                    employees.fk_user
                                              FROM `employees`
                                              WHERE employees.id = {0}", id);

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
        private void PostUser(Employee user)
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
                                        'Employee',
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
        private void SendEmail(Employee user)
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
        [Authorize]
        [Produces("application/json")]
        [HttpGet("~{id}")]
        public IActionResult GetEmployeesByTimetable(int id)
        {
            Timetable timetable = new Timetable();
            List<Employee> employees = new List<Employee>();
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
                    timetable = GetTimetable(id);
                }
                catch
                {
                    return StatusCode(500, "Failed to get the timetable.");
                }
                MySqlCommand cmd;
                cmd = Database.Connection.CreateCommand();
                cmd.CommandText = @"SELECT 
                                        employees.id,
                                        users.name as name,
                                        users.surname as surname,
                                        users.birthdate as birthdate,
                                        users.phone as phone,
                                        users.email as email,
                                        employees.specialization,
                                        employees.fk_user,
                                        employees.fk_room,
                                        employees.image
                                    FROM `employees`
                                    LEFT JOIN users ON employees.fk_user=users.id";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["name"]),
                            Surname = Convert.ToString(reader["surname"]),
                            Specialization = Convert.ToString(reader["specialization"]),
                            Birthdate = Convert.ToString(reader["birthdate"]),
                            Phone = Convert.ToString(reader["phone"]),
                            Email = Convert.ToString(reader["email"]),
                            Fk_user = Convert.ToInt32(reader["fk_user"]),
                            Fk_room = Convert.ToString(reader["fk_room"]),
                            Image = Convert.ToString(reader["image"])
                        });
                    }
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the employee.");
            }
            List<Employee> employeesFiltered = new List<Employee>();
            for (int i = 0; i < employees.Count; i++)
            {
                if (!timetable.IsLocked.Contains("fk" + employees[i].Id + "doc"))
                {
                    employeesFiltered.Add(employees[i]);
                }
            }
            return Json(employeesFiltered);
        }
        private Timetable GetTimetable(int id)
        {
            Timetable timetable = new Timetable();
            MySqlCommand cmd;
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
            return timetable;
        }
        [Authorize(Roles = Role.Admin + "," + Role.Employee)]
        [Produces("application/json")]
        [HttpGet("@{id}")]
        public IActionResult GetEmployeeByUser(int id)
        {
            Employee employee = new Employee();
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
                                                    employees.id,
                                                    users.name as name,
                                                    users.surname as surname,
                                                    users.birthdate as birthdate,
                                                    users.phone as phone,
                                                    users.email as email,
                                                    employees.specialization,
                                                    employees.fk_user,
                                                    employees.fk_room,
                                                    employees.image
                                              FROM `employees`
                                              LEFT JOIN users ON employees.fk_user=users.id 
                                              WHERE employees.fk_user = {0}", id);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employee.Id = Convert.ToInt32(reader["id"]);
                        employee.Name = Convert.ToString(reader["name"]);
                        employee.Surname = Convert.ToString(reader["surname"]);
                        employee.Birthdate = Convert.ToString(reader["birthdate"]);
                        employee.Phone = Convert.ToString(reader["phone"]);
                        employee.Email = Convert.ToString(reader["email"]);
                        employee.Specialization = Convert.ToString(reader["specialization"]);
                        employee.Fk_user = Convert.ToInt32(reader["fk_user"]);
                        employee.Fk_room = Convert.ToString(reader["fk_room"]);
                        employee.Image = Convert.ToString(reader["image"]);
                    }
                }
            }

            catch
            {
                return StatusCode(500, "Failed to get the employee.");
            }
            return Json(employee);
        }
    }
}