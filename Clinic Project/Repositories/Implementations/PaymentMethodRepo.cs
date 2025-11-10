using Clinic_Project.Data;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Clinic_Project.Repositories.Implementations
{
    public class PaymentMethodRepo : MainRepo<PaymentMethod>, IPaymentMethodRepo
    {
        private readonly AppDbContext _context;
        public PaymentMethodRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsPaymentMethodExistAsync(string name)
        {
            return await _context.PaymentMethods.AnyAsync(pm => pm.Name == name);
        }
    }
}
