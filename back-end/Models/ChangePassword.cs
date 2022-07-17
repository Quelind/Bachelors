using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Models
{
    public class ChangePassword
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        public string Username { get; set; }
    }
}