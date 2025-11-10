using Clinic_Project.Dtos.Person;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_Project.Dtos.Patient
{
    public class PatientReadDto : PersonReadDto
    {
        public int PersonId { get; set; }
    }
}
