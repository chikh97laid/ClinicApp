using Clinic_Project.Dtos.Payment;
using Clinic_Project.Repositories.Interfaces;
using FluentValidation;

namespace Clinic_Project.Validation.Payment
{
    public class PaymentUpdateWriteDtoValidator : AbstractValidator<PaymentUpdateWriteDto>
    {
        public PaymentUpdateWriteDtoValidator(IUnitOfWork unit)
        {

            RuleFor(x => x.Amount)
                    .GreaterThan(0)
                    .WithMessage("Amount must be greater than zero");

        }
    }
}

