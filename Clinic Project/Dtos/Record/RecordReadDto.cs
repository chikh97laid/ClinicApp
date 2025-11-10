namespace Clinic_Project.Dtos.Record
{
    public class RecordReadDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Diagnosis { get; set; }
        public string? Description { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int PatientAge { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
    }
}
