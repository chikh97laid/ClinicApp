using AutoMapper;
using Clinic_Project.Dtos.Prescription;
using Clinic_Project.Helpers;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;
using Clinic_Project.Services.Interfaces;

namespace Clinic_Project.Services.Implementations
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PrescriptionService(IUnitOfWork uniteOfWork, IMapper mapper)
        {
            _unitOfWork = uniteOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<PrescriptionReadDto>?>> GetAllAsync()
        {
            var prescriptions = await _unitOfWork.Prescriptions.GetAllAsync();
            if (prescriptions == null || !prescriptions.Any())
            {
                return Result<IEnumerable<PrescriptionReadDto>?>.Fail("No prescriptions");
            }

            var readDtoList = _mapper.Map<IEnumerable<PrescriptionReadDto>>(prescriptions);

            return Result<IEnumerable<PrescriptionReadDto>?>.Ok(readDtoList);
        }

        public async Task<Result<PrescriptionReadDto>?> GetAsyncById(int id, string currentUserId, bool isAdmin)
        {
            var prescription = await _unitOfWork.Prescriptions.GetOneAsync(p => p.Id == id);

            if (prescription == null) 
                return Result<PrescriptionReadDto>.Fail("Prescription not found", enErrorType.NotFound);

            var userId = prescription?.Patient?.Person?.User?.Id;
            if (currentUserId != userId && !isAdmin)
            {
                return Result<PrescriptionReadDto>.Fail("You are not allowed to access this prescription.", enErrorType.Forbiden);
            }

            var prescriptionReadDto = _mapper.Map<PrescriptionReadDto>(prescription);

            return Result<PrescriptionReadDto>.Ok(prescriptionReadDto);
        }
        public async Task<ValidationResult> ValidationData(PrescriptionWriteDto dto)
        {
            if (!await _unitOfWork.Patients.IsExistAsync(p => p.Id == dto.PatientId))
            {
                return ValidationResult.Fail("Patient not found", enErrorType.NotFound);
            }

            if (!await _unitOfWork.Doctors.IsExistAsync(d => d.Id == dto.DoctorId))
            {
                return ValidationResult.Fail("Doctor not found", enErrorType.NotFound);
            }
            var medIds = dto.Medications.Select(m => m.MedicationId).ToList();
            var existIds = (await _unitOfWork.Medications.FindAsync(x => medIds.Contains(x.Id))).Select(x => x.Id).ToHashSet();

            var missedIds = medIds.Except(existIds);

            if (missedIds.Any())
            {
                return ValidationResult.Fail($"Medication Id {string.Join(", ", missedIds)} doesn't exist", enErrorType.NotFound);
            }


            return ValidationResult.Success();
        }

        public async Task<Result<PrescriptionReadDto>?> CreateAsync(PrescriptionWriteDto dto)
        {
            var validation = await ValidationData(dto);
            if (!validation.IsValid)
            {
                return Result<PrescriptionReadDto>.Fail(validation.Message, validation.ErrorType);
            }

            var prescription = _mapper.Map<Prescription>(dto);

            await _unitOfWork.Prescriptions.AddAsync(prescription);
            await _unitOfWork.CommitChangesAsync();

            prescription = await _unitOfWork.Prescriptions.GetOneAsync(p => p.Id == prescription.Id);

            var prescriptionReadDto = _mapper.Map<PrescriptionReadDto>(prescription);
            return Result<PrescriptionReadDto>.Ok(prescriptionReadDto);
        }

        public async Task<Result<PrescriptionReadDto>?> UpdateAsync(int id, PrescriptionUpdateWriteDto dto)
        {
            var prescription = await _unitOfWork.Prescriptions.GetOneAsync(p => p.Id == id);

            if (prescription == null)
                return Result<PrescriptionReadDto>.Fail("Prescription not found");

            _mapper.Map(dto, prescription);

            _unitOfWork.Prescriptions.Update(prescription);
            await _unitOfWork.CommitChangesAsync();

            var prescriptionUpdateReadDto = _mapper.Map<PrescriptionReadDto>(prescription);
            return Result<PrescriptionReadDto>.Ok(prescriptionUpdateReadDto);
        }

        public async Task<Result<PrescriptionReadDto>?> DeleteAsync(int id)
        {
            var prescription = await _unitOfWork.Prescriptions.GetOneAsync(p => p.Id == id);
            if (prescription == null) 
                return Result<PrescriptionReadDto>.Fail("Prescription not found");

            _unitOfWork.Prescriptions.Delete(prescription);
                
            await _unitOfWork.CommitChangesAsync();

            return Result<PrescriptionReadDto>.Ok(_mapper.Map<PrescriptionReadDto>(prescription));
        }

        public Task<Result<PrescriptionReadDto>?> UpdateAsync(int id, PrescriptionWriteDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<Result<PrescriptionReadDto>?> GetAsyncById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
