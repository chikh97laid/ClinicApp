using Clinic_Project.Data;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Clinic_Project.Repositories.Implementations
{
    public class DoctorRepo : MainRepo<Doctor>, IDoctorRepo
    {
        private readonly AppDbContext _context;
        public DoctorRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Doctor>?> GetAllAsync()
        {
            return await _context.Doctors
                .Include(d => d.Person)
                .Include(d => d.Specialization)
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<Doctor?> GetOneAsync(Expression<Func<Doctor, bool>> predicate)
        {
            return await _context.Doctors
                .Include(d => d.Person)
                .Include(d => d.Specialization)
                .AsNoTracking()
                .SingleOrDefaultAsync(predicate);
        }
    }
}
