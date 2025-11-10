using Clinic_Project.Data;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Clinic_Project.Repositories.Implementations
{
    public class PrescriptionMedicationRepo : MainRepo<PrescriptionMedication>, IPrescriptionMedicationRepo
    {
        private readonly AppDbContext _context;
        public PrescriptionMedicationRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<PrescriptionMedication>?> GetAllAsync()
        {
            return await _context.prescriptionMedications
                .Include(pm => pm.Medication)
                .AsNoTracking()
                .ToListAsync();

        }

        public override async Task<PrescriptionMedication?> GetOneAsync(Expression<Func<PrescriptionMedication, bool>> predicate)
        {
            return await _context.prescriptionMedications
                .Include(pm => pm.Medication)
                .Include(pm => pm.Prescription)
                    .ThenInclude(p => p.Patient)
                        .ThenInclude(p => p.Person)
                            .ThenInclude(p => p.User)
                .AsNoTracking()
                .SingleOrDefaultAsync(predicate);
        }

        public async Task<bool> IsForeignKeysRepeated(int fk1, int fk2)
        {
            return await _context.prescriptionMedications.AnyAsync(x => x.PrescriptionId == fk1 && x.MedicationId == fk2);
        }
    }
}
