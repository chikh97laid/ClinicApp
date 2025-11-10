using Clinic_Project.Data;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Clinic_Project.Repositories.Implementations
{
    public class PaymentRepo : MainRepo<Payment>, IPaymentRepo
    {
        private readonly AppDbContext _context;
        public PaymentRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Payment>?> GetAllAsync()
        {
            return await _context.Payments
                .Include(pay => pay.Appointment)
                    .ThenInclude(ap => ap.Patient)
                        .ThenInclude(p => p.Person)
                .Include(pay => pay.Appointment)
                    .ThenInclude(ap => ap.Doctor)
                        .ThenInclude(d => d.Specialization)
                .Include(p => p.PaymentMethod)
                .AsNoTracking()
                .ToListAsync();

        }

        public override async Task<Payment?> GetOneAsync(Expression<Func<Payment, bool>> predicate)
        {
            return await _context.Payments
                .Include(pay => pay.Appointment)
                    .ThenInclude(ap => ap.Patient)
                        .ThenInclude(p => p.Person)
                        .ThenInclude(p => p.User)
                .Include(pay => pay.Appointment)
                    .ThenInclude(ap => ap.Doctor)
                        .ThenInclude(d => d.Specialization)
                .Include(p => p.PaymentMethod)
                .AsNoTracking()
                .SingleOrDefaultAsync(predicate);
        }
    }
}
