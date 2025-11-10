using Clinic_Project.Dtos.Payment;
using Clinic_Project.Helpers;

namespace Clinic_Project.Services.Interfaces
{
    public interface IPaymentService : IService<PaymentReadDto, PaymentWriteDto>
    {
        Task<Result<PaymentReadDto>?> ConfirmPaymentAsync(int id);

        Task<Result<PaymentReadDto>?> UpdateAsync(int id, PaymentUpdateWriteDto dto);

        Task<Result<PaymentReadDto>?> GetAsyncById(int id, string currentUserId, bool isAdmin);
    }
}

