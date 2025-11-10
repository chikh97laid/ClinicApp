using Clinic_Project.Dtos.Person;
using System.ComponentModel.DataAnnotations;

namespace Clinic_Project.Dtos.Account
{
    public class RegisterDto 
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        public RegisterPerson? Person { get; set; } 
    }
}
