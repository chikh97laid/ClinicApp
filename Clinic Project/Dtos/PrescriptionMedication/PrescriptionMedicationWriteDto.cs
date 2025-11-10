using System.ComponentModel.DataAnnotations;

namespace Clinic_Project.Dtos.PrescriptionMedication
{
    public class PrescriptionMedicationBaseDto
    {
        [Required, MaxLength(50)]
        public string Dosage { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Frequency { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class PrescriptionMedicationWriteDto : PrescriptionMedicationBaseDto
    {
        public int MedicationId { get; set; }
        public int PrescriptionId { get; set; }
    }

    public class PrescriptionMedicationUpdateWriteDto : PrescriptionMedicationBaseDto
    {
        public int MedicationId { get; set; }
    }

}
