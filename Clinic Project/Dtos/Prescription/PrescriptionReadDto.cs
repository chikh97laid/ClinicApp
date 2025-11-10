
using Clinic_Project.Dtos.PrescriptionMedication;
using Clinic_Project.Dtos.Record;

namespace Clinic_Project.Dtos.Prescription
{
    public class PrescriptionReadDto
    {
        public int Id { get; set; }
        public DateTime IssueDate { get; set; }
        public string Instructions { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int PatientAge { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public int RecordId { get; set; }
        public RecordReadDto Record { get; set; } = new RecordReadDto();
        public List<PrescriptionMedicationReadDto> Medications { get; set; } = [];
    }

}
