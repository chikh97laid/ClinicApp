using Clinic_Project.Models;

namespace Clinic_Project.Repositories.Interfaces
{
    public interface IPaymentMethodRepo : IRepo<PaymentMethod>
    {
        Task<bool> IsPaymentMethodExistAsync(string name);
    }
}
