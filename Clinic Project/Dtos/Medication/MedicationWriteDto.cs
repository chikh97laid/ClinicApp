using System.ComponentModel.DataAnnotations;

namespace Clinic_Project.Dtos.Medication
{
    public class MedicationWriteDto
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
