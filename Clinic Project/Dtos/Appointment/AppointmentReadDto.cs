using Clinic_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinic_Project.Dtos.Appointment
{
    public class AppointmentReadDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public enAppointmentStatus Status { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
    }

    public class AppointmentConfirmedReadDto
    {
        public int Id { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
    }
}
