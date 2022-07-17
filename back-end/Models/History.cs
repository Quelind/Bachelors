namespace ClinicAPI.Models
{
    public class History
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int Fk_patient { get; set; }
        public string Patient_name { get; set; }
        public string Patient_surname { get; set; }
    }
}