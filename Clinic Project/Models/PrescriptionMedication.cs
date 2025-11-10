using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_Project.Models
{
    public class PrescriptionMedication
    {
        [Key]
        public int Id { get; set; }
        
        [Required, MaxLength(50)]
        public string Dosage { get; set; } = string.Empty;
        
        [Required, MaxLength(100)]
        public string Frequency { get; set; } = string.Empty;
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }

        [ForeignKey(nameof(Medication))]
        public int MedicationId { get; set; }
        public Medication? Medication { get; set; }

        [ForeignKey(nameof(Prescription))]
        public int PrescriptionId { get; set; }
        public Prescription? Prescription { get; set; }

    }
}
