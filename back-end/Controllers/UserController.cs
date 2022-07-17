using ClinicAPI.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ClinicAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private AppDatabase Database { get; set; }
        public UserController(AppDatabase database)
        {
            Database = database;
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] Authenticate authenticate)
        {
            try
            {
                Database.Connection.Open();
            }
            catch
            {
                return StatusCode(500, "Could not establish a database connection.");
            }
            User user = new User();
            try
            {
                user = GetUser("username", authenticate.Username);
            }
            catch
            {
                return StatusCode(500, "The user doesn't exist.");
            }
            if (user.Username == null)
            {
                return BadRequest("Username is incorrect");
            }

            if (!BCrypt.Net.BCrypt.Verify(authenticate.Password, user.Password))
            {
                return StatusCode(500, "Password is incorrect");
            }
            var secret = "I wish I was born female.";
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(14),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var handler = new JwtSecurityTokenHandler();
            user.Token = handler.WriteToken(handler.CreateToken(descriptor));
            user.Password = null;

            return Ok(user);
        }
        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("No user provided.");
            }
            if (user.Name == null)
            {
                return BadRequest("The user's name cannot be null.");
            }
            if (user.Surname == null)
            {
                return BadRequest("The user's surname cannot be null.");
            }
            if (user.Birthdate == null)
            {
                return BadRequest("The user's birthdate cannot be null.");
            }
            if (user.Phone == null)
            {
                return BadRequest("The user's phone cannot be null.");
            }
            if (user.Email == null)
            {
                return BadRequest("The user's email cannot be null.");
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
                                        'Unconfirmed',
                                        @Token,
                                        'false')";
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Surname", user.Surname);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Birthdate", user.Birthdate);
                cmd.Parameters.AddWithValue("@Phone", user.Phone);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                var rand = new Random();
                if (user.Token == null)
                {
                    user.Token = rand.Next(100000, 999999).ToString();
                    cmd.Parameters.AddWithValue("@Token", user.Token);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Token", user.Token);
                }

                int code = cmd.ExecuteNonQuery();
                if (code == 0)
                {
                    return BadRequest("The information provided is incorrect.");
                }
                try
                {
                    PostPatient(user);
                }
                catch
                {

                    return StatusCode(500, "Failed to add the patient.");
                }

                SendVerificationEmail(user);

                Database.Connection.Close();
                return Ok("The user has been added successfully.");
            }
            catch
            {
                return StatusCode(500, "Failed to add the user.");
            }
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
        private void PostPatient(User user)
        {
            int id = GetNewestUserID();
            MySqlCommand cmd;
            cmd = Database.Connection.CreateCommand();
            cmd = Database.Connection.CreateCommand();
            cmd.CommandText = @"INSERT into patients (
                                        fk_user,
                                        debt) 
                                    VALUES(
                                        @Fk_user,
                                            0)";
            cmd.Parameters.AddWithValue("@Fk_user", id);
            int code = cmd.ExecuteNonQuery();
        }
        private User GetUser(string key, string filter)
        {
            User user = new User();
            MySqlCommand cmd;
            cmd = Database.Connection.CreateCommand();
            cmd.CommandText = string.Format(@"SELECT *
                                              FROM users 
                                              WHERE users.{0} = '{1}'", key, filter);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    user.Id = Convert.ToInt32(reader["id"]);
                    user.Username = Convert.ToString(reader["username"]);
                    user.Password = Convert.ToString(reader["password"]);
                    user.Name = Convert.ToString(reader["name"]);
                    user.Surname = Convert.ToString(reader["surname"]);
                    user.Role = Convert.ToString(reader["role"]);
                    user.Birthdate = Convert.ToString(reader["birthdate"]);
                    user.Phone = Convert.ToString(reader["phone"]);
                    user.Email = Convert.ToString(reader["email"]);
                    user.Token = Convert.ToString(reader["token"]);
                    user.Reset_token = Convert.ToString(reader["reset_token"]);
                }
            }
            return user;
        }
        private void SendVerificationEmail(User user)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("info.clinicapi@yahoo.com"));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Clinic Email verification";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text =
                      $@"<h4>Verify Email</h4>
                         <p>Thanks for registering!</p>
                         <p>Welcome! Use this code to activate your account!</p>
                         <p><b>{user.Token}</b></p>"
            };
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.mail.yahoo.com", 587);
            smtp.Authenticate("info.clinicapi@yahoo.com", "ogtaumtcrikpzcwl");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        [Produces("application/json")]
        [AllowAnonymous]
        [HttpPost("verify")]
        public IActionResult VerifyEmail([FromBody] VerifyEmail token)
        {
            try
            {
                Database.Connection.Open();
            }
            catch
            {
                return StatusCode(500, "Could not establish a database connection.");
            }
            User user = new User();
            try
            {
                user = GetUser("token", token.Token);
            }
            catch
            {
                return StatusCode(500, "The user doesn't exist.");
            }
            if (user.Token == null)
            {
                return BadRequest("Token is incorrect.");
            }

            try
            {
                MySqlCommand cmd;
                cmd = Database.Connection.CreateCommand();
                cmd.CommandText = @"UPDATE users
                                    SET verified='true',
                                        token = 'NULL',
                                        role = 'User'
                                    WHERE id=@id";
                cmd.Parameters.AddWithValue("@id", user.Id);

                int code = cmd.ExecuteNonQuery();
                return Ok("The user has been verified successfully.");
            }
            catch
            {
                return StatusCode(500, "Could not verify the user.");
            }
        }
        [Produces("application/json")]
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            User user = new User();
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
                                              FROM users 
                                              WHERE id = {0}", id);

                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    user.Id = Convert.ToInt32(reader["id"]);
                    user.Name = Convert.ToString(reader["name"]);
                    user.Surname = Convert.ToString(reader["surname"]);
                    user.Role = Convert.ToString(reader["role"]);
                    user.Birthdate = Convert.ToString(reader["birthdate"]);
                    user.Phone = Convert.ToString(reader["phone"]);
                    user.Email = Convert.ToString(reader["email"]);
                }
            }
            catch
            {
                return StatusCode(500, "Failed to get the user.");
            }
            return Json(user);
        }
        [Produces("application/json")]
        [Authorize]
        [HttpPatch("{id}")]
        public IActionResult PatchUser(int id, [FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("No user provided.");
            }
            if (user.Name == null)
            {
                return BadRequest("The user's name cannot be null.");
            }
            if (user.Surname == null)
            {
                return BadRequest("The user's surname cannot be null.");
            }
            if (user.Birthdate == null)
            {
                return BadRequest("The user's birthdate cannot be null.");
            }
            if (user.Phone == null)
            {
                return BadRequest("The user's phone cannot be null.");
            }
            if (user.Email == null)
            {
                return BadRequest("The user's email cannot be null.");
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
                cmd.CommandText = @"UPDATE users
                                    SET name=@Name,
                                        surname=@Surname,
                                        birthdate=@Birthdate,
                                        phone=@Phone,
                                        email=@Email
                                    WHERE id=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Surname", user.Surname);
                cmd.Parameters.AddWithValue("@Birthdate", user.Birthdate);
                cmd.Parameters.AddWithValue("@Phone", user.Phone);
                cmd.Parameters.AddWithValue("@Email", user.Email);

                int code = cmd.ExecuteNonQuery();
                if (code == 0)
                {
                    return BadRequest("The user does not exist or the information provided is incorrect.");
                }
                return Ok("The user has been updated successfully.");
            }
            catch
            {
                return StatusCode(500, "Could not update the user.");
            }
        }
        [Authorize]
        [Produces("application/json")]
        [HttpPatch("update-password/{id}")]
        public IActionResult UpdatePassword(ChangePassword password, int id)
        {
            if (password == null)
            {
                return BadRequest("Password is incorrect");
            }
            if (password.OldPassword == null)
            {
                return BadRequest("No old password provided.");
            }
            if (password.NewPassword == null)
            {
                return BadRequest("No new password provided.");
            }
            try
            {
                Database.Connection.Open();
            }
            catch
            {
                return StatusCode(500, "Could not establish a database connection.");
            }
            User user = new User();
            try
            {
                user = GetUser("username", password.Username);
            }
            catch
            {
                return StatusCode(500, "The user doesn't exist.");
            }
            if (user.Username == null)
            {
                return BadRequest("Username is incorrect");
            }

            if (!BCrypt.Net.BCrypt.Verify(password.OldPassword, user.Password))
                return BadRequest("Password is incorrect.");
            try
            {
                MySqlCommand cmd;
                cmd = Database.Connection.CreateCommand();
                cmd.CommandText = @"UPDATE users
                                    SET password=@Password
                                    WHERE id=@id";
                cmd.Parameters.AddWithValue("@id", id);
                password.NewPassword = BCrypt.Net.BCrypt.HashPassword(password.NewPassword);
                cmd.Parameters.AddWithValue("@Password", password.NewPassword);
                int code = cmd.ExecuteNonQuery();
                if (code == 0)
                {
                    return BadRequest("The information provided is incorrect.");
                }

                return Ok("The password has been changed successfully.");
            }
            catch
            {
                return StatusCode(500, "Failed to change the password.");
            }
        }
        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPatch("forgot-password")]
        public IActionResult ForgotPassword([FromBody] ForgotPassword forgotPassword)
        {
            if (forgotPassword == null)
            {
                return BadRequest("Password is incorrect");
            }
            try
            {
                Database.Connection.Open();
            }
            catch
            {
                return StatusCode(500, "Could not establish a database connection.");
            }
            User user = new User();
            try
            {
                user = GetUser("username", forgotPassword.Username);
            }
            catch
            {
                return StatusCode(500, "The user doesn't exist.");
            }
            if (user.Username == null)
            {
                return BadRequest("Username is incorrect");
            }

            try
            {
                MySqlCommand cmd;
                cmd = Database.Connection.CreateCommand();
                cmd.CommandText = @"UPDATE users
                                    SET reset_token=@Reset_token
                                    WHERE username=@Username";
                var rand = new Random();
                user.Reset_token = rand.Next(100000, 999999).ToString();
                cmd.Parameters.AddWithValue("@Reset_token", user.Reset_token);
                cmd.Parameters.AddWithValue("@Username", user.Username);

                int code = cmd.ExecuteNonQuery();
                if (code == 0)
                {
                    return BadRequest("The user does not exist or the information provided is incorrect.");
                }
                SendPasswordResetEmail(user);
                return Ok("Check your email for the token.");
            }
            catch
            {
                return StatusCode(500, "Could not reset the password.");
            }
        }
        private void SendPasswordResetEmail(User user)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("info.clinicapi@yahoo.com"));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Clinic Password Reset";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text =
                      $@"<h4>Reset Password</h4>
                         <p>Use this code to reset your password!</p>
                         <p><b>{user.Reset_token}</b></p>"
            };
            using var smtp = new SmtpClient();
            // 587, SecureSocketOptions.StartTls
            smtp.Connect("smtp.mail.yahoo.com", 587);
            smtp.Authenticate("info.clinicapi@yahoo.com", "ogtaumtcrikpzcwl");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPatch("verify-password")]
        public IActionResult VerifyPassword(VerifyPassword resetPassword)
        {
            if (resetPassword == null)
            {
                return BadRequest("No new password provided.");
            }

            try
            {
                Database.Connection.Open();
            }
            catch
            {
                return StatusCode(500, "Could not establish a database connection.");
            }
            User user = new User();
            try
            {
                user = GetUser("reset_token", resetPassword.Reset_token);
            }
            catch
            {
                return StatusCode(500, "The user doesn't exist.");
            }
            if (user.Reset_token == null)
            {
                return BadRequest("Reset token is incorrect.");
            }

            try
            {
                MySqlCommand cmd;
                cmd = Database.Connection.CreateCommand();
                cmd.CommandText = @"UPDATE users
                                    SET reset_token='NULL',
                                        password = @Password
                                    WHERE reset_token=@Reset_token";
                resetPassword.Password = BCrypt.Net.BCrypt.HashPassword(resetPassword.Password);
                cmd.Parameters.AddWithValue("@Password", resetPassword.Password);
                cmd.Parameters.AddWithValue("@Reset_token", resetPassword.Reset_token);

                int code = cmd.ExecuteNonQuery();
                if (code == 0)
                {
                    return BadRequest("Could not verify the password.");
                }
                return Ok("The password has been verified successfully.");
            }
            catch
            {
                return StatusCode(500, "Could not update the patient.");
            }
        }
    }
}
