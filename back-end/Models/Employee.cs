using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Specialization { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public string Birthdate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Fk_user { get; set; }
        public string Fk_room { get; set; }
        public string Image { get; set; }

        public string Password { get; set; }
        public string Username { get; set; }
    }
}