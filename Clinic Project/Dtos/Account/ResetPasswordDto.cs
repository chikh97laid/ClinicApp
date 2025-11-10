using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Clinic_Project.Dtos.Account
{
    public class ResetPasswordDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required, PasswordPropertyText]
        public string NewPassword { get; set; } = string.Empty;
    }
}

