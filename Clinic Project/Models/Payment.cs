using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_Project.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string InvoiceNumber { get; set; } = string.Empty;        
        public DateTime Date { get; set; }        
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; } 

        [ForeignKey(nameof(Appointment))]
        public int AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }


        [ForeignKey(nameof(PaymentMethod))]
        public int PaymentMethodId { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }

    }
}
