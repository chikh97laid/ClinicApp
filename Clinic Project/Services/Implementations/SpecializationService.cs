using AutoMapper;
using Clinic_Project.Dtos.Prescription;
using Clinic_Project.Dtos.Specialization;
using Clinic_Project.Helpers;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;
using Clinic_Project.Services.Interfaces;

namespace Clinic_Project.Services.Implementations
{
    public class SpecializationService : ISpecializationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SpecializationService(IUnitOfWork uniteOfWork, IMapper mapper)
        {
            _unitOfWork = uniteOfWork;
            _mapper = mapper;
        }



        public async Task<Result<IEnumerable<SpecializationReadDto>?>> GetAllAsync()
        {
            var specializations = (await _unitOfWork.Specializations.GetAllAsync())!.ToList();
            if (!specializations.Any())
                return Result<IEnumerable<SpecializationReadDto>?>.Fail("No PaymentMethods!", enErrorType.NotFound);

            var specializationsReadDto = _mapper.Map<IEnumerable<SpecializationReadDto>>(specializations);
            return Result<IEnumerable<SpecializationReadDto>?>.Ok(specializationsReadDto);
        }

        public async Task<Result<SpecializationReadDto>?> GetAsyncById(int id)
        {
            var specialization = await _unitOfWork.Specializations.GetOneAsync(s => s.Id == id);
            if (specialization == null)
                return Result<SpecializationReadDto>.Fail("Specialization not found", enErrorType.NotFound);

            var specializationReadDto = _mapper.Map<SpecializationReadDto>(specialization);
            return Result<SpecializationReadDto>.Ok(specializationReadDto);
        }

        public async Task<Result<SpecializationReadDto>?> CreateAsync(SpecializationWriteDto dto)
        {
            if (await _unitOfWork.Specializations.IsSpecializationExistAsync(dto.Name))
            {
                return Result<SpecializationReadDto>.Fail("Specialization already exist!", enErrorType.Conflict);
            }

            var specialization = _mapper.Map<Specialization>(dto);

            await _unitOfWork.Specializations.AddAsync(specialization);
            await _unitOfWork.CommitChangesAsync();

            var specializationReadDto = _mapper.Map<SpecializationReadDto>(specialization);
            return Result<SpecializationReadDto>.Ok(specializationReadDto);
        }

        public async Task<Result<SpecializationReadDto>?> UpdateAsync(int id, SpecializationWriteDto dto)
        {

            var specialization = await _unitOfWork.Specializations.GetOneAsync(s => s.Id == id);
            if (specialization == null)
            {
                return Result<SpecializationReadDto>.Fail("Specialization not found");
            }

            if (specialization.Name != dto.Name && await _unitOfWork.Specializations.IsSpecializationExistAsync(dto.Name))
            {
                return Result<SpecializationReadDto>.Fail("Specialization already exist!", enErrorType.Conflict);
            }

            _mapper.Map(dto, specialization);

            _unitOfWork.Specializations.Update(specialization);
            await _unitOfWork.CommitChangesAsync();

            var specializationReadDto = _mapper.Map<SpecializationReadDto>(specialization);
            return Result<SpecializationReadDto>.Ok(specializationReadDto);
        }

        public async Task<Result<SpecializationReadDto>?> DeleteAsync(int id)
        {
            var specialization = await _unitOfWork.Specializations.GetOneAsync(s => s.Id == id);
            if (specialization == null)
                return Result<SpecializationReadDto>.Fail("Specialization not found");

            _unitOfWork.Specializations.Delete(specialization);
            await _unitOfWork.CommitChangesAsync();

            return Result<SpecializationReadDto>.Ok(_mapper.Map<SpecializationReadDto>(specialization));
        }

    }
}
