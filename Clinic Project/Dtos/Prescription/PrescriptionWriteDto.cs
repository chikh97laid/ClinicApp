using System.ComponentModel.DataAnnotations;
using Clinic_Project.Dtos.PrescriptionMedication;
using Clinic_Project.Dtos.Record;
using Clinic_Project.Models;

namespace Clinic_Project.Dtos.Prescription
{
    public class PrescriptionWriteDto
    {
        [Required]
        public string Instructions { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public RecordUpdateWriteDto Record { get; set; } = new RecordUpdateWriteDto();
        public List<PrescriptionMedicationUpdateWriteDto> Medications { get; set; } = [];
    }

    public class PrescriptionUpdateWriteDto
    {

        [Required]
        public string Instructions { get; set; } = string.Empty;
    }

}


