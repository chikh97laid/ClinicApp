using Clinic_Project.Data;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Clinic_Project.Repositories.Implementations
{
    public class AppointmentRepo : MainRepo<Appointment>, IAppointmentRepo
    {
        private readonly AppDbContext _context;
        public AppointmentRepo(AppDbContext context) : base(context) 
        {
            _context = context;
        }

        public void UpdateAppointmentStatus(Appointment appointment)
        {
            _context.Attach(appointment);
            _context.Entry(appointment).Property(ap => ap.Status).IsModified = true;
        }

        public override async Task<IEnumerable<Appointment>?> GetAllAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                    .ThenInclude(p => p.Person)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.Person)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.Specialization)
                .AsNoTracking()
                .ToListAsync();

        }

        public override async Task<Appointment?> GetOneAsync(Expression<Func<Appointment, bool>> predicate)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                    .ThenInclude(p => p.Person)
                    .ThenInclude(p => p.User)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.Person)
                    .ThenInclude(p => p.User)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.Specialization)
                .SingleOrDefaultAsync(predicate);
        }

    }
}
