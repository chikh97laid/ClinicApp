using Clinic_Project.Data;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Clinic_Project.Repositories.Implementations
{
    public class PatientRepo : MainRepo<Patient>, IPatientRepo
    {
        private readonly AppDbContext _context;
        public PatientRepo(AppDbContext context) : base(context) 
        {
            _context = context;
        }

        public override async Task<IEnumerable<Patient>?> GetAllAsync()
        {
            return await _context.Patients
                .Include(p => p.Person)
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<Patient?> GetOneAsync(Expression<Func<Patient, bool>> predicate)
        {
            return await _context.Patients
                .Include(p => p.Person)
                .ThenInclude(p => p.User)
                .AsNoTracking()
                .SingleOrDefaultAsync(predicate);
        }

    }
}
