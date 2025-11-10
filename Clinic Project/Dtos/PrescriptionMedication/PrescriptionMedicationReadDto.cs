namespace Clinic_Project.Dtos.PrescriptionMedication
{
    public class PrescriptionMedicationReadDto
    {
        public int Id { set; get; }
        public string MedicationName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MedicationId { get; set; }
        public int PrescriptionId { get; set; }
    }
}
