namespace ClinicAPI.Models
{
    public class Visit
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string IsLocked { get; set; }
        public string Patient_comment { get; set; }
        public string Confirmed { get; set; }

        public int Fk_timetable { get; set; }

        public int Fk_procedure { get; set; }
        public string Procedure_information { get; set; }
        public string Procedure_name { get; set; }

        public string Fk_room { get; set; }

        public int Fk_patient { get; set; }
        public string Patient_name { get; set; }
        public string Patient_surname { get; set; }
        public string Patient_history { get; set; }
        public string Patient_description { get; set; }
        public string Patient_email { get; set; }

        public int Fk_doctor { get; set; }
        public string Doctor_name { get; set; }
        public string Doctor_surname { get; set; }
        public string Specialization { get; set; }
    }
}