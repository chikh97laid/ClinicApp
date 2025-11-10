using Clinic_Project.Dtos.PaymentMethod;

namespace Clinic_Project.Services.Interfaces
{
    public interface IPaymentMethodService : IService<PaymentMethodReadDto, PaymentMethodWriteDto>
    {
    }
}
