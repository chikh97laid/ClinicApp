using System.ComponentModel.DataAnnotations;

namespace Clinic_Project.Dtos.PaymentMethod
{
    public class PaymentMethodWriteDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
