using System.ComponentModel.DataAnnotations;

namespace Clinic_Project.Dtos.Specialization
{
    public class SpecializationReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
