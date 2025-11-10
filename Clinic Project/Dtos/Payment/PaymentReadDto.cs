using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Clinic_Project.Models;

namespace Clinic_Project.Dtos.Payment
{
    public class PaymentReadDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentMethodName { get; set; } = string.Empty;
        public bool IsPaid { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int PaymentMethodId { get; set; }
        public int AppointmentId { get; set; }
    }
}
