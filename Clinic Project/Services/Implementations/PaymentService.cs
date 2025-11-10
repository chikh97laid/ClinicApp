using AutoMapper;
using Clinic_Project.Dtos.Payment;
using Clinic_Project.Helpers;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;
using Clinic_Project.Services.Interfaces;

namespace Clinic_Project.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<PaymentReadDto>?>> GetAllAsync()
        {
            var payments = await _unitOfWork.Payments.GetAllAsync();

            if (payments == null || !payments.Any())
                return Result<IEnumerable<PaymentReadDto>?>.Fail("No payments!");

            var paymentsReadDto = _mapper.Map<IEnumerable<PaymentReadDto>>(payments);

            return Result<IEnumerable<PaymentReadDto>?>.Ok(paymentsReadDto);
        }

        public async Task<Result<PaymentReadDto>?> GetAsyncById(int id, string currentUserId, bool isAdmin)
        {
            var payment = await _unitOfWork.Payments.GetOneAsync(pay => pay.Id == id);
            if(payment == null)
            {
                return Result<PaymentReadDto>.Fail("Payment not found", enErrorType.NotFound);
            }

            var userId = payment?.Appointment?.Patient?.Person?.User?.Id;
            if (currentUserId != userId && !isAdmin)
            {
                return Result<PaymentReadDto>.Fail("You are not allowed to access this payment.", enErrorType.Forbiden);
            }

            var PaymentReadDto = _mapper.Map<PaymentReadDto>(payment);

            return Result<PaymentReadDto>.Ok(PaymentReadDto);
        }

        private async Task<ValidationResult> ValidationData(PaymentWriteDto dto)
        {

            if (!await _unitOfWork.PaymentMethods.IsExistAsync(pm => pm.Id == dto.PaymentMethodId))
            {
                return ValidationResult.Fail($"PaymentMethodId {dto.PaymentMethodId} doesn't exist", enErrorType.NotFound);
            }

            if (await _unitOfWork.Payments.IsExistAsync(p => p.AppointmentId == dto.AppointmentId))
            {
                return ValidationResult.Fail("Payment already exists for this appointment", enErrorType.Conflict);
            }

            return ValidationResult.Success();
        } 

        public async Task<Result<PaymentReadDto>?> CreateAsync(PaymentWriteDto dto)
        {
            var validation = await ValidationData(dto);
            if (!validation.IsValid)
            {
                return Result<PaymentReadDto>.Fail(validation.Message!, validation.ErrorType);
            }

            var appointment = await _unitOfWork.Appointments.GetOneAsync(ap => ap.Id == dto.AppointmentId);

            if (appointment == null)
                return Result<PaymentReadDto>.Fail("Appointment not found", enErrorType.NotFound);

            if (appointment.Status != enAppointmentStatus.Completed)
            {
                return Result<PaymentReadDto>.Fail("Payment can only be created for completed appointments", enErrorType.NotFound);
            }

            var payment = _mapper.Map<Payment>(dto);
            payment.IsPaid = false;
            payment.InvoiceNumber = $"INV-{DateTime.Now:yyyy}-{Guid.NewGuid().ToString()[..6].ToUpper()}";

            await _unitOfWork.Payments.AddAsync(payment);
            await _unitOfWork.CommitChangesAsync();

            var paymentReadDto = _mapper.Map<PaymentReadDto>(payment);
            return Result<PaymentReadDto>.Ok(paymentReadDto);
        }

        public async Task<Result<PaymentReadDto>?> UpdateAsync(int id, PaymentUpdateWriteDto dto)
        {

            var payment = await _unitOfWork.Payments.GetOneAsync(pay => pay.Id == id);

            if (payment == null)
                return Result<PaymentReadDto>.Fail("Payment not found");

            if(payment.IsPaid == true)
                return Result<PaymentReadDto>.Fail("You can't update a paid payment!");

            if (!await _unitOfWork.PaymentMethods.IsExistAsync(pm => pm.Id == dto.PaymentMethodId))
            {
                return Result<PaymentReadDto>.Fail($"PaymentMethodId {dto.PaymentMethodId} doesn't exist", enErrorType.BadRequest);
            }

            _mapper.Map(dto, payment);

            _unitOfWork.Payments.Update(payment);
            await _unitOfWork.CommitChangesAsync();

            var paymentReadDto = _mapper.Map<PaymentReadDto>(payment);
            return Result<PaymentReadDto>.Ok(paymentReadDto);
        }

        public async Task<Result<PaymentReadDto>?> ConfirmPaymentAsync(int id)
        {
            var payment = await _unitOfWork.Payments.GetOneAsync(p => p.Id == id);
            if (payment == null)
                return Result<PaymentReadDto>.Fail("Payment not found");

            if (!await _unitOfWork.PaymentMethods.IsExistAsync(pm => pm.Id == payment.PaymentMethodId))
            {
                return Result<PaymentReadDto>.Fail($"PaymentMethodId {id} doesn't exist", enErrorType.NotFound);
            }

            if (payment.IsPaid)
                return Result<PaymentReadDto>.Fail("Payment already confirmed", enErrorType.BadRequest);

            if(payment.Appointment.Status != enAppointmentStatus.Completed)
                return Result<PaymentReadDto>.Fail("Appointment not completed yet", enErrorType.BadRequest);

            payment.IsPaid = true;
            await _unitOfWork.CommitChangesAsync();

            return Result<PaymentReadDto>.Ok(_mapper.Map<PaymentReadDto>(payment));
        }

        public async Task<Result<PaymentReadDto>?> DeleteAsync(int id)
        {
            var payment = await _unitOfWork.Payments.GetOneAsync(p => p.Id == id);
            if (payment == null) 
                return Result<PaymentReadDto>.Fail("Payment not found");

            _unitOfWork.Payments.Delete(payment);
            await _unitOfWork.CommitChangesAsync();

            return Result<PaymentReadDto>.Ok(_mapper.Map<PaymentReadDto>(payment));
        }

        public Task<Result<PaymentReadDto>?> UpdateAsync(int id, PaymentWriteDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<Result<PaymentReadDto>?> GetAsyncById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
