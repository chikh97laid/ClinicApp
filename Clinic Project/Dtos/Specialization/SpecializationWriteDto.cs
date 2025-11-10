using System.ComponentModel.DataAnnotations;

namespace Clinic_Project.Dtos.Specialization
{
    public class SpecializationWriteDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
