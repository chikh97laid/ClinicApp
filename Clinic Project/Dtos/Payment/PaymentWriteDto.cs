using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinic_Project.Dtos.Payment
{
    public class PaymentWriteDto
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int AppointmentId { get; set; }
        public int PaymentMethodId { get; set; }
    }

    public class PaymentUpdateWriteDto
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int PaymentMethodId { get; set; }
    }

}
