using System.ComponentModel.DataAnnotations;

namespace Clinic_Project.Dtos.PaymentMethod
{
    public class PaymentMethodReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
