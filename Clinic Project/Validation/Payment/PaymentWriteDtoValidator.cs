using Clinic_Project.Dtos.Payment;
using Clinic_Project.Repositories.Interfaces;
using FluentValidation;

namespace Clinic_Project.Validation.Payment
{
    public class PaymentWriteDtoValidator : AbstractValidator<PaymentWriteDto>
    {
        public PaymentWriteDtoValidator(IUnitOfWork unit)
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than zero");

        }
    }
}
