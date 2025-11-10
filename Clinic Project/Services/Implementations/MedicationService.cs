using AutoMapper;
using Clinic_Project.Dtos.Medication;
using Clinic_Project.Dtos.Person;
using Clinic_Project.Helpers;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;
using Clinic_Project.Services.Interfaces;

namespace Clinic_Project.Services.Implementations
{
    public class MedicationService : IMedicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MedicationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<MedicationReadDto>?>> GetAllAsync()
        {
            var medications = await _unitOfWork.Medications.GetAllAsync();

            if (medications == null || !medications.Any()) 
                return Result<IEnumerable<MedicationReadDto>?>.Fail("No medications!");

            var medicationsReadDto = _mapper.Map<IEnumerable<MedicationReadDto>>(medications);

            return Result<IEnumerable<MedicationReadDto>?>.Ok(medicationsReadDto);
        }

        public async Task<Result<MedicationReadDto>?> GetAsyncById(int id)
        {
            var medication = await _unitOfWork.Medications.GetOneAsync(m => m.Id == id);

            if (medication == null)
                return Result<MedicationReadDto>.Fail("Medication not found");

            var medicationReadDto = _mapper.Map<MedicationReadDto>(medication);

            return Result<MedicationReadDto>.Ok(medicationReadDto);
        }

        public async Task<Result<MedicationReadDto>?> CreateAsync(MedicationWriteDto dto)
        {

            if (await _unitOfWork.Medications.IsMedicationNameExistAsync(dto.Name))
            {
                return Result<MedicationReadDto>.Fail("Medication Name already exists", enErrorType.Conflict);
            }

            var medication = _mapper.Map<Medication>(dto);

            await _unitOfWork.Medications.AddAsync(medication);
            await _unitOfWork.CommitChangesAsync();

            var medicationReadDto = _mapper.Map<MedicationReadDto>(medication);

            return Result<MedicationReadDto>.Ok(medicationReadDto);
        }

        public async Task<Result<MedicationReadDto>?> UpdateAsync(int id, MedicationWriteDto dto)
        {

            var medication = await _unitOfWork.Medications.GetOneAsync(m => m.Id == id);
            if (medication == null)
            {
                return Result<MedicationReadDto>.Fail("Medication not found");
            }

            if (medication.Name != dto.Name && await _unitOfWork.Medications.IsMedicationNameExistAsync(dto.Name))
            {
                return Result<MedicationReadDto>.Fail("Medication Name already exists", enErrorType.Conflict);
            }

            _mapper.Map(dto, medication);

            _unitOfWork.Medications.Update(medication);
            await _unitOfWork.CommitChangesAsync();

            var medicationReadDto = _mapper.Map<MedicationReadDto>(medication);
            return Result<MedicationReadDto>.Ok(medicationReadDto);
        }

        public async Task<Result<MedicationReadDto>?> DeleteAsync(int id)
        {
            var medication = await _unitOfWork.Medications.GetOneAsync(m => m.Id == id);
            if (medication == null) return Result<MedicationReadDto>.Fail("Medication not found");

            _unitOfWork.Medications.Delete(medication);
            await _unitOfWork.CommitChangesAsync();

            return Result<MedicationReadDto>.Ok(_mapper.Map<MedicationReadDto>(medication));
        }
    }
}
