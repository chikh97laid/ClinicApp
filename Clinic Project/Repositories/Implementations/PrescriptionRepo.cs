using Clinic_Project.Data;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Clinic_Project.Repositories.Implementations
{
    public class PrescriptionRepo : MainRepo<Prescription>, IPrescriptionRepo
    {
        private readonly AppDbContext _context;
        public PrescriptionRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }


        public override async Task<IEnumerable<Prescription>?> GetAllAsync()
        {
            return await _context.Prescriptions
                .Include(p => p.Patient)
                    .ThenInclude(pa => pa.Person)
                .Include(p => p.Doctor)
                    .ThenInclude(d => d.Person)
                .Include(d => d.Doctor)
                    .ThenInclude(d => d.Specialization)
                .Include(mp => mp.Medications)
                    .ThenInclude(m => m.Medication)
                .Include(p => p.Record)
                .AsNoTracking()
                .ToListAsync();

        }

        public override async Task<Prescription?> GetOneAsync(Expression<Func<Prescription, bool>> predicate)
        {
            return await _context.Prescriptions
                .Include(p => p.Patient)
                    .ThenInclude(pa => pa.Person)
                    .ThenInclude(p => p.User)
                .Include(p => p.Doctor)
                    .ThenInclude(d => d.Person)
                    .ThenInclude(p => p.User)
                .Include(d => d.Doctor)
                    .ThenInclude(d => d.Specialization)
                .Include(mp => mp.Medications)
                    .ThenInclude(m => m.Medication)
                .Include(p => p.Record)
                .AsNoTracking()
                .SingleOrDefaultAsync(predicate);
        }

    }
}
