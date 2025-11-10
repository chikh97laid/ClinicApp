using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinic_Project.Dtos.Record
{
    public class RecordWriteDto
    {
        public DateTime Date { get; set; }
        public string? Diagnosis { get; set; }
        public string? Description { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
    }

    public class RecordUpdateWriteDto
    {
        public string? Diagnosis { get; set; }
        public string? Description { get; set; }
    }
}
