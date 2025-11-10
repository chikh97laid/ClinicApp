using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_Project.Models
{
    public enum enAppointmentStatus { Pending, Confirm, Completed, Canceled, Rescheduled, NoShow }
    public class Appointment
    {
        [Key]
        public int Id { get; set; } 
        public DateTime CreatedAt { get; set; }
        public enAppointmentStatus Status { get; set; }
        public DateTime? ScheduledDate { get; set; }

        [ForeignKey(nameof(Patient))]
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        [ForeignKey(nameof(Doctor))]
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public Payment? Payment { get; set; }
    }
}
