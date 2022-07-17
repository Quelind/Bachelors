using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Models
{
    public class Register
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Birthdate { get; set; }

        [Required]
        [MinLength(9)]
        [MaxLength(9)]
        public string Phone { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ComparePassword { get; set; }
    }
}