using Clinic_Project.Dtos.Person;
using System.ComponentModel.DataAnnotations;

namespace Clinic_Project.Dtos.Doctor
{
    public class DoctorWriteDto : PersonWriteDto
    {
        public int SpecializationId { get; set; }
    }
}
