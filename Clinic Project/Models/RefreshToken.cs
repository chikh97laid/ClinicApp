using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_Project.Models
{
    public class RefreshToken
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }

        [ForeignKey(nameof(User)), Required]
        public string UserId { get; set; } = string.Empty;
        public AppUser User { get; set; } = new AppUser();
    }
}



