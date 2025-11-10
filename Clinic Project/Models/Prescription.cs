using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_Project.Models
{
    public class Prescription
    {
        [Key]
        public int Id { get; set; }
        public DateTime IssueDate { get; set; }
        
        [Required]
        public string Instructions { get; set; } = string.Empty;

        [ForeignKey(nameof(Patient))]
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        [ForeignKey(nameof(Doctor))]
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        [ForeignKey(nameof(Record))]
        public int RecordId { get; set; }
        public Record? Record { get; set; }
        public ICollection<PrescriptionMedication> Medications { get; set; } = new List<PrescriptionMedication>();
    }
}
