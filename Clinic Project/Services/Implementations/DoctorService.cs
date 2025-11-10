using AutoMapper;
using Clinic_Project.Dtos.Doctor;
using Clinic_Project.Helpers;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;
using Clinic_Project.Services.Interfaces;

namespace Clinic_Project.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DoctorService(IUnitOfWork uniteOfWork, IMapper mapper)
        {
            _unitOfWork = uniteOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<DoctorReadDto>?>> GetAllAsync()
        {
            var doctors = (await _unitOfWork.Doctors.GetAllAsync())!.ToList();
            if (!doctors.Any()) return Result<IEnumerable<DoctorReadDto>?>.Fail("No Doctors!");

            var readDtoList = _mapper.Map<IEnumerable<DoctorReadDto>>(doctors);
            return Result<IEnumerable<DoctorReadDto>?>.Ok(readDtoList);
        }

        public async Task<Result<DoctorReadDto>?> GetAsyncById(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetOneAsync(d => d.Id == id);
            if (doctor == null) return Result<DoctorReadDto>.Fail("Doctor not found");

            var doctorReadDto = _mapper.Map<DoctorReadDto>(doctor);
            return Result<DoctorReadDto>.Ok(doctorReadDto);
        }

        public async Task<Result<DoctorReadDto>?> CreateAsync(DoctorWriteDto dto)
        {
            if (dto.Email != null)
            {
                if (await _unitOfWork.Persons.IsEmailExistAsync(dto.Email))
                {
                    return Result<DoctorReadDto>.Fail("Email already exist!", enErrorType.Conflict);
                }
            }

            if (!await _unitOfWork.Specializations.IsExistAsync(s => s.Id == dto.SpecializationId))
            {
                return Result<DoctorReadDto>.Fail($"SpecializationId {dto.SpecializationId} doesn't exist");
            }

            if (dto.DateOfBirth > DateTime.Now)
            {
                return Result<DoctorReadDto>.Fail("Invalid Date!", enErrorType.BadRequest);
            }

            var person = _mapper.Map<Person>(dto);

            var doctor = new Doctor() { SpecializationId = dto.SpecializationId, Person = person };

            await _unitOfWork.Doctors.AddAsync(doctor);
            await _unitOfWork.CommitChangesAsync();

            doctor = await _unitOfWork.Doctors.GetOneAsync(d => d.Id == doctor.Id);

            var doctorReadDto = _mapper.Map<DoctorReadDto>(doctor);
            return Result<DoctorReadDto>.Ok(doctorReadDto);

        }

        public async Task<Result<DoctorReadDto>?> UpdateAsync(int id, DoctorWriteDto dto)
        {

            var doctor = await _unitOfWork.Doctors.GetOneAsync(d => d.Id == id);  // means doctor is tracked
            if (doctor == null)
            {
                return Result<DoctorReadDto>.Fail("Doctor not found");
            }

            if (!await _unitOfWork.Specializations.IsExistAsync(s => s.Id == dto.SpecializationId))
            {
                return Result<DoctorReadDto>.Fail($"SpecializationId {dto.SpecializationId} doesn't exist");
            }

            if (dto.Email != null)
            {
                if (dto.Email != doctor.Person.Email && await _unitOfWork.Persons.IsEmailExistAsync(dto.Email))
                {
                    return Result<DoctorReadDto>.Fail("Email already exist!", enErrorType.Conflict);
                }
            }

            if (dto.DateOfBirth > DateTime.Now)
            {
                return Result<DoctorReadDto>.Fail("Invalid Date!", enErrorType.BadRequest);
            }

            _mapper.Map(dto, doctor.Person);
            doctor.SpecializationId = dto.SpecializationId;

            _unitOfWork.Doctors.Update(doctor);
            await _unitOfWork.CommitChangesAsync();

            var doctorReadDto = _mapper.Map<DoctorReadDto>(doctor);
            return Result<DoctorReadDto>.Ok(doctorReadDto);
        }

        public async Task<Result<DoctorReadDto>?> DeleteAsync(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetOneAsync(d => d.Id == id);
            if (doctor == null) return Result<DoctorReadDto>.Fail("Doctor not found");

            _unitOfWork.Doctors.Delete(doctor);
            _unitOfWork.Persons.Delete(doctor.Person);

            await _unitOfWork.CommitChangesAsync();

            return Result<DoctorReadDto>.Ok(_mapper.Map<DoctorReadDto>(doctor));
        }
    }
}
