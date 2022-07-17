namespace ClinicAPI.Models
{
    public class Timetable
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string IsLocked { get; set; }
    }
}