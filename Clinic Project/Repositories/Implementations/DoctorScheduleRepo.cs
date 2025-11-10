using Clinic_Project.Data;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;

namespace Clinic_Project.Repositories.Implementations
{
    public class DoctorScheduleRepo : MainRepo<DoctorSchedule>, IDoctorScheduleRepo
    {
        private readonly AppDbContext _context;
        public DoctorScheduleRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
