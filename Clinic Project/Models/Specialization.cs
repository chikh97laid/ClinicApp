using System.ComponentModel.DataAnnotations;

namespace Clinic_Project.Models
{
    public class Specialization
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
