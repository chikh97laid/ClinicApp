using System.ComponentModel.DataAnnotations;

namespace Clinic_Project.Dtos.Account
{
    public class RoleDto
    {
        [Required]
        public string Name { get; set; } = null!;
    }

}
