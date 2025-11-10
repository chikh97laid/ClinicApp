using Clinic_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinic_Project.Dtos.Appointment
{
    public class AppointmentWriteDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
    }
}

