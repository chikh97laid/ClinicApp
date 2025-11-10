using Clinic_Project.Data;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Clinic_Project.Repositories.Implementations
{
    public class MedicationRepo : MainRepo<Medication>, IMedicationRepo
    {
        private readonly AppDbContext _context;
        public MedicationRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsMedicationNameExistAsync(string Name)
        {
            return await _context.Medications.AnyAsync(m => m.Name == Name);
        }
    }
}
