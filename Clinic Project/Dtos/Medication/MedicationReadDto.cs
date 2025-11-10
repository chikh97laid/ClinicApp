using System.ComponentModel.DataAnnotations;

namespace Clinic_Project.Dtos.Medication
{
    public class MedicationReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
