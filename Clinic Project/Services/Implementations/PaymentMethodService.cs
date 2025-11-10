using AutoMapper;
using Clinic_Project.Dtos.PaymentMethod;
using Clinic_Project.Helpers;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Implementations;
using Clinic_Project.Repositories.Interfaces;
using Clinic_Project.Services.Interfaces;

namespace Clinic_Project.Services.Implementations
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PaymentMethodService(IUnitOfWork uniteOfWork, IMapper mapper)
        {
            _unitOfWork = uniteOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<PaymentMethodReadDto>?>> GetAllAsync()
        {
            var paymentMethods = (await _unitOfWork.PaymentMethods.GetAllAsync())!.ToList();
            if (!paymentMethods.Any()) 
                return Result<IEnumerable<PaymentMethodReadDto>?>.Fail("No PaymentMethods!", enErrorType.NotFound);

            var paymentMethodsReadDto = _mapper.Map<IEnumerable<PaymentMethodReadDto>>(paymentMethods);
            return Result<IEnumerable<PaymentMethodReadDto>?>.Ok(paymentMethodsReadDto);
        }

        public async Task<Result<PaymentMethodReadDto>?> GetAsyncById(int id)
        {
            var paymentMethod = await _unitOfWork.PaymentMethods.GetOneAsync(pm => pm.Id == id);
            if (paymentMethod == null) 
                return Result<PaymentMethodReadDto>.Fail("PaymentMethod not found", enErrorType.NotFound);

            var paymentMethodReadDto = _mapper.Map<PaymentMethodReadDto>(paymentMethod);
            return Result<PaymentMethodReadDto>.Ok(paymentMethodReadDto);
        }

        public async Task<Result<PaymentMethodReadDto>?> CreateAsync(PaymentMethodWriteDto dto)
        {
            if (await _unitOfWork.PaymentMethods.IsPaymentMethodExistAsync(dto.Name))
            {
                return Result<PaymentMethodReadDto>.Fail("PaymentMethod already exist!", enErrorType.Conflict);
            }

            var paymentMethod = _mapper.Map<PaymentMethod>(dto);

            await _unitOfWork.PaymentMethods.AddAsync(paymentMethod);
            await _unitOfWork.CommitChangesAsync();

            var paymentMethodReadDto = _mapper.Map<PaymentMethodReadDto>(paymentMethod);
            return Result<PaymentMethodReadDto>.Ok(paymentMethodReadDto);
        }

        public async Task<Result<PaymentMethodReadDto>?> UpdateAsync(int id, PaymentMethodWriteDto dto)
        {

            var paymentMethod = await _unitOfWork.PaymentMethods.GetOneAsync(pm => pm.Id == id);  
            if (paymentMethod == null)
            {
                return Result<PaymentMethodReadDto>.Fail("PaymentMethod not found");
            }

            if (dto.Name != paymentMethod.Name && await _unitOfWork.PaymentMethods.IsPaymentMethodExistAsync(dto.Name))
            {
                return Result<PaymentMethodReadDto>.Fail("PaymentMethod already exist!", enErrorType.Conflict);
            }

            _mapper.Map(dto, paymentMethod);
            
            _unitOfWork.PaymentMethods.Update(paymentMethod);
            await _unitOfWork.CommitChangesAsync();

            var paymentMethodReadDto = _mapper.Map<PaymentMethodReadDto>(paymentMethod);
            return Result<PaymentMethodReadDto>.Ok(paymentMethodReadDto);
        }

        public async Task<Result<PaymentMethodReadDto>?> DeleteAsync(int id)
        {
            var paymentMethod = await _unitOfWork.PaymentMethods.GetOneAsync(pm => pm.Id == id);
            if (paymentMethod == null) 
                return Result<PaymentMethodReadDto>.Fail("PaymentMethod not found");

            _unitOfWork.PaymentMethods.Delete(paymentMethod);
            await _unitOfWork.CommitChangesAsync(); 

            return Result<PaymentMethodReadDto>.Ok(_mapper.Map<PaymentMethodReadDto>(paymentMethod));
        }
    }
}
