using System.ComponentModel.DataAnnotations;

namespace Clinic_Project.Dtos.Account
{
    public enum enRoles { Admin, Doctor, Patient }
    public class UserRoleDto
    {
        [Required, MaxLength(20)]
        public string UserId { get; set; } = null!;

        public ICollection<enRoles> Roles { get; set; } = new List<enRoles>();
    }
}
