using Clinic_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Clinic_Project.Dtos.Person
{
    public enum enGender {Male, Female }
    public class PersonWriteDto
    {
        [Required, MaxLength(20)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public enGender Gender { get; set; }

        [MaxLength(50), EmailAddress]
        public string? Email { get; set; }

        [MaxLength(20), Phone]
        public string? Phone { get; set; }

        [MaxLength(50)]
        public string? Address { get; set; }
        
    }
}
