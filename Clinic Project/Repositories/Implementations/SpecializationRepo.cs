using Clinic_Project.Data;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Clinic_Project.Repositories.Implementations
{
    public class SpecializationRepo : MainRepo<Specialization>, ISpecializationRepo
    {
        private readonly AppDbContext _context;
        public SpecializationRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsSpecializationExistAsync(string name)
        {
            return await _context.Specializations.AnyAsync(s => s.Name == name);
        }
    }
}
