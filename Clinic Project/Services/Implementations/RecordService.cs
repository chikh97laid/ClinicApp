using AutoMapper;
using Clinic_Project.Dtos.Record;
using Clinic_Project.Helpers;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;
using Clinic_Project.Services.Interfaces;

namespace Clinic_Project.Services.Implementations
{
    public class RecordService : IRecordService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RecordService(IUnitOfWork uniteOfWork, IMapper mapper)
        {
            _unitOfWork = uniteOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<RecordReadDto>?>> GetAllAsync()
        {
            var records = await _unitOfWork.Records.GetAllAsync();
            if (records == null || !records.Any())
            {
                return Result<IEnumerable<RecordReadDto>?>.Fail("No records");
            }

            var recordsReadDto = _mapper.Map<IEnumerable<RecordReadDto>>(records);

            return Result<IEnumerable<RecordReadDto>?>.Ok(recordsReadDto);
        }

        public async Task<Result<RecordReadDto>?> GetAsyncById(int id, string currentUserId, bool isAdmin)
        {

            var record = await _unitOfWork.Records.GetOneAsync(r => r.Id == id);

            if (record == null)
                return Result<RecordReadDto>.Fail("Record not found", enErrorType.NotFound);

            var userId = record?.Patient?.Person?.User?.Id;
            if (currentUserId != userId && !isAdmin)
            {
                return Result<RecordReadDto>.Fail("You are not allowed to access this record.", enErrorType.Forbiden);
            }

            var recordReadDto = _mapper.Map<RecordReadDto>(record);

            return Result<RecordReadDto>.Ok(recordReadDto);
        }
        public async Task<ValidationResult> ValidationData(RecordWriteDto dto)
        {
            if (!await _unitOfWork.Patients.IsExistAsync(p => p.Id == dto.PatientId))
            {
                return ValidationResult.Fail("Patient not found", enErrorType.NotFound);
            }

            if (!await _unitOfWork.Doctors.IsExistAsync(d => d.Id == dto.DoctorId))
            {
                return ValidationResult.Fail("Doctor not found", enErrorType.NotFound);
            }


            return ValidationResult.Success();
        }

        public async Task<Result<RecordReadDto>?> CreateAsync(RecordWriteDto dto)
        {
            var validation = await ValidationData(dto);
            if (!validation.IsValid)
            {
                return Result<RecordReadDto>.Fail(validation.Message, validation.ErrorType);
            }

            var record = _mapper.Map<Record>(dto);

            await _unitOfWork.Records.AddAsync(record);
            await _unitOfWork.CommitChangesAsync();

            record = await _unitOfWork.Records.GetOneAsync(r => r.Id == record.Id);

            var recordReadDto = _mapper.Map<RecordReadDto>(record);
            return Result<RecordReadDto>.Ok(recordReadDto);
        }

        public async Task<Result<RecordReadDto>?> UpdateAsync(int id, RecordUpdateWriteDto dto)
        {
            var record = await _unitOfWork.Records.GetOneAsync(r => r.Id == id);

            if (record == null)
                return Result<RecordReadDto>.Fail("Record not found");

            _mapper.Map(dto, record);

            _unitOfWork.Records.Update(record);
            await _unitOfWork.CommitChangesAsync();

            var recordReadDto = _mapper.Map<RecordReadDto>(record);
            return Result<RecordReadDto>.Ok(recordReadDto);
        }

        public async Task<Result<RecordReadDto>?> DeleteAsync(int id)
        {
            var record = await _unitOfWork.Records.GetOneAsync(r => r.Id == id);
            if (record == null)
                return Result<RecordReadDto>.Fail("Record not found");

            _unitOfWork.Records.Delete(record);
            await _unitOfWork.CommitChangesAsync();

            return Result<RecordReadDto>.Ok(_mapper.Map<RecordReadDto>(record));
        }

        public Task<Result<RecordReadDto>?> UpdateAsync(int id, RecordWriteDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<Result<RecordReadDto>?> GetAsyncById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
