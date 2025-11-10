using Clinic_Project.Dtos.Person;

namespace Clinic_Project.Dtos.Doctor
{
    public class DoctorReadDto : PersonReadDto
    {
        public string Specialization { get; set; } = string.Empty;
        public int PersonId { get; set; }
    }
}
