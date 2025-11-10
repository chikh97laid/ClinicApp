using AutoMapper;
using Clinic_Project.Dtos.PrescriptionMedication;
using Clinic_Project.Helpers;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;
using Clinic_Project.Services.Interfaces;

namespace Clinic_Project.Services.Implementations
{
    public class PrescriptionMedicationService : IPrescriptionMedicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PrescriptionMedicationService(IUnitOfWork uniteOfWork, IMapper mapper)
        {
            _unitOfWork = uniteOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<PrescriptionMedicationReadDto>?>> GetAllAsync()
        {
            var prescriptionMedications = await _unitOfWork.PrescriptionMedications.GetAllAsync();
            if (prescriptionMedications == null || !prescriptionMedications.Any())
            {
                return Result<IEnumerable<PrescriptionMedicationReadDto>?>.Fail("No PrescriptionMedications!", enErrorType.NotFound);
            }

            var prescMedsReadDto = _mapper.Map<IEnumerable<PrescriptionMedicationReadDto>>(prescriptionMedications);
            return Result<IEnumerable<PrescriptionMedicationReadDto>?>.Ok(prescMedsReadDto);
        }

        public async Task<Result<PrescriptionMedicationReadDto>?> GetAsyncById(int id, string currentUserId, bool isAdmin)
        {
            var prescriptionMedication = await _unitOfWork.PrescriptionMedications.GetOneAsync(pm => pm.Id == id);
            
            if (prescriptionMedication == null) 
                return Result<PrescriptionMedicationReadDto>.Fail("PrescriptionMedication not found", enErrorType.NotFound);
            
            var userId = prescriptionMedication?.Prescription?.Patient?.Person?.User?.Id;
            if (currentUserId != userId && !isAdmin)
            {
                return Result<PrescriptionMedicationReadDto>.Fail("You are not allowed to access this prescription.", enErrorType.Forbiden);
            }

            var prescMedReadDto = _mapper.Map<PrescriptionMedicationReadDto>(prescriptionMedication);
            return Result<PrescriptionMedicationReadDto>.Ok(prescMedReadDto);
        }

        public async Task<ValidationResult> ValidationData(PrescriptionMedicationWriteDto dto)
        {
            if (!await _unitOfWork.Prescriptions.IsExistAsync(p => p.Id == dto.PrescriptionId))
            {
                return ValidationResult.Fail("Prescription not found", enErrorType.NotFound);
            }

            if (!await _unitOfWork.Medications.IsExistAsync(m => m.Id == dto.MedicationId))
            {
                return ValidationResult.Fail("Medication not found", enErrorType.NotFound);
            }

            if (await _unitOfWork.PrescriptionMedications.IsForeignKeysRepeated(dto.PrescriptionId, dto.MedicationId))
            {
                return ValidationResult.Fail($"fk1 = {dto.PrescriptionId} and fk2 = {dto.MedicationId} already exist!", enErrorType.Conflict);

            }

            return ValidationResult.Success();
        }

        public async Task<Result<PrescriptionMedicationReadDto>?> CreateAsync(PrescriptionMedicationWriteDto dto)
        {
            var validation = await ValidationData(dto);
            if(!validation.IsValid)
            {
                return Result<PrescriptionMedicationReadDto>.Fail(validation.Message, validation.ErrorType);
            }

            var prescriptionMedication = _mapper.Map<PrescriptionMedication>(dto);

            await _unitOfWork.PrescriptionMedications.AddAsync(prescriptionMedication);
            await _unitOfWork.CommitChangesAsync();

            prescriptionMedication = await _unitOfWork.PrescriptionMedications.GetOneAsync(pm => pm.Id == prescriptionMedication.Id);

            var prescMedReadDto = _mapper.Map<PrescriptionMedicationReadDto>(prescriptionMedication);
            return Result<PrescriptionMedicationReadDto>.Ok(prescMedReadDto);
        }

        public async Task<Result<PrescriptionMedicationReadDto>?> UpdateAsync(int id, PrescriptionMedicationBaseDto dto)
        {
            var prescriptionMedication = await _unitOfWork.PrescriptionMedications.GetOneAsync(pm => pm.Id == id);  
            if (prescriptionMedication == null)
            {
                return Result<PrescriptionMedicationReadDto>.Fail("PrescriptionMedication not found", enErrorType.NotFound);
            }

            _mapper.Map(dto, prescriptionMedication);

            _unitOfWork.PrescriptionMedications.Update(prescriptionMedication);
            await _unitOfWork.CommitChangesAsync();

            var prescMedReadDto = _mapper.Map<PrescriptionMedicationReadDto>(prescriptionMedication);
            return Result<PrescriptionMedicationReadDto>.Ok(prescMedReadDto);
        }

        public async Task<Result<PrescriptionMedicationReadDto>?> DeleteAsync(int id)
        {
            var prescriptionMedication = await _unitOfWork.PrescriptionMedications.GetOneAsync(pm => pm.Id == id);
            if (prescriptionMedication == null)
            {
                return Result<PrescriptionMedicationReadDto>.Fail("PrescriptionMedication not found", enErrorType.NotFound);
            }
            _unitOfWork.PrescriptionMedications.Delete(prescriptionMedication);

            await _unitOfWork.CommitChangesAsync();

            return Result<PrescriptionMedicationReadDto>.Ok(_mapper.Map<PrescriptionMedicationReadDto>(prescriptionMedication));
        }

        public Task<Result<PrescriptionMedicationReadDto>?> UpdateAsync(int id, PrescriptionMedicationWriteDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<Result<PrescriptionMedicationReadDto>?> GetAsyncById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
