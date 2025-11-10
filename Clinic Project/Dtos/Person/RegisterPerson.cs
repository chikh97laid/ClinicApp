using System.ComponentModel.DataAnnotations;

namespace Clinic_Project.Dtos.Person
{
    public class RegisterPerson
    {
        [Required, MaxLength(20)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public enGender Gender { get; set; }

        [MaxLength(50)]
        public string? Address { get; set; }
    }
}
