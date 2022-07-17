namespace ClinicAPI.Models
{
    public class Procedure
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Requirement { get; set; }
        public string Room_type { get; set; }
        public string Information { get; set; }
        public int Duration { get; set; }
        public int Personnel_count { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
    }
}