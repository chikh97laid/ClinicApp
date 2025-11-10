using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_Project.Models
{
    public class DoctorSchedule
    {
        [Key]
        public int Id { get; set; }
        
        [Required, MaxLength(50)]
        public string WorkingDays { get; set; } = "Sunday-Thursday";
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool IsAvailable { get; set; } = true;

        [ForeignKey(nameof(Doctor))]
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
