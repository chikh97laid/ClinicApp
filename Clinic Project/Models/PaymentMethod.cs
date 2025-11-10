using System.ComponentModel.DataAnnotations;

namespace Clinic_Project.Models
{
    public class PaymentMethod
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
