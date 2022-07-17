using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Models
{
    public class ForgotPassword
    {
        [Required]
        public string Username { get; set; }
    }
}