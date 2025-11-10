using Clinic_Project.Data;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Clinic_Project.Repositories.Implementations
{
    public class RecordRepo : MainRepo<Record>, IRecordRepo
    {
        private readonly AppDbContext _context;
        public RecordRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Record>?> GetAllAsync()
        {
            return await _context.Records
                .Include(r => r.Patient)
                    .ThenInclude(p => p.Person)
                .Include(r => r.Doctor)
                    .ThenInclude(p => p.Person)
                .Include(r => r.Doctor)
                    .ThenInclude(d => d.Specialization)
                .AsNoTracking()
                .ToListAsync();

        }

        public override async Task<Record?> GetOneAsync(Expression<Func<Record, bool>> predicate)
        {
            return await _context.Records
                .Include(r => r.Patient)
                    .ThenInclude(p => p.Person)
                    .ThenInclude(p => p.User)
                .Include(r => r.Doctor)
                    .ThenInclude(p => p.Person)
                    .ThenInclude(p => p.User)
                .Include(r => r.Doctor)
                    .ThenInclude(d => d.Specialization)
                .AsNoTracking()
                .SingleOrDefaultAsync(predicate);
        }

    }
}
