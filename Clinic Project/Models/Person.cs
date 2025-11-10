using Clinic_Project.Dtos.Person;
using System.ComponentModel.DataAnnotations;

namespace Clinic_Project.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        
        [Required, MaxLength(20)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public enGender Gender { get; set; }
        
        [MaxLength(50)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }
        
        [MaxLength(100)]
        public string? Address { get; set; }
        public Doctor? Doctor { get; set; }
        public Patient? Patient { get; set; }
        public AppUser? User { get; set; }
    }
}
