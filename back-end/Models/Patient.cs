namespace ClinicAPI.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Information { get; set; }
        public string Birthdate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Fk_user { get; set; }
        public double Debt { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}