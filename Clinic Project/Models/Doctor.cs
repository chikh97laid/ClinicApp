using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_Project.Models
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Person))]
        public int PersonId { get; set; }
        public Person? Person { get; set; }

        [ForeignKey(nameof(Specialization))]
        public int SpecializationId {  get; set; }        
        public Specialization? Specialization { get; set; }    
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Record> Records { get; set; } = new List<Record>();
        public DoctorSchedule? DoctorSchedule { get; set; }

    }
}
