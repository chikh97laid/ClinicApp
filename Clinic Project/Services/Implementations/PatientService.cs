using AutoMapper;
using Clinic_Project.Dtos.Patient;
using Clinic_Project.Helpers;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;
using Clinic_Project.Services.Interfaces;

namespace Clinic_Project.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PatientService(IUnitOfWork uniteOfWork, IMapper mapper)
        {
            _unitOfWork = uniteOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<PatientReadDto>?>> GetAllAsync()
        {
            var patients = (await _unitOfWork.Patients.GetAllAsync())!.ToList();
            if (!patients.Any()) return Result<IEnumerable<PatientReadDto>?>.Fail("No Patients!");

            var readDtoList = _mapper.Map<IEnumerable<PatientReadDto>>(patients);          
            return Result<IEnumerable<PatientReadDto>?>.Ok(readDtoList);
        }

        public async Task<Result<PatientReadDto>?> GetAsyncById(int id, string currentUserId, bool isAdmin)
        {
            var patient = await _unitOfWork.Patients.GetOneAsync(pa => pa.Id == id);
            if (patient == null) return Result<PatientReadDto>.Fail("Patient not found");

            var userId = patient?.Person?.User?.Id;
            var doctorId = patient.Appointments.FirstOrDefault(ap => ap.PatientId == id).Doctor.Person.User.Id;
            if (currentUserId != userId && currentUserId != doctorId && !isAdmin)
            {
                return Result<PatientReadDto>.Fail("You are not allowed to access this Patient.", enErrorType.Forbiden);
            }

            var patientReadDto = _mapper.Map<PatientReadDto>(patient);
            return Result<PatientReadDto>.Ok(patientReadDto);
        }

        public async Task<Result<PatientReadDto>?> CreateAsync(PatientWriteDto dto)
        {
            if (dto.Email != null)
            {
                if (await _unitOfWork.Persons.IsEmailExistAsync(dto.Email))
                {
                    return Result<PatientReadDto>.Fail("Email already exist!", enErrorType.Conflict);
                }
            }

            var person = _mapper.Map<Person>(dto);

            var patient = new Patient() { Person = person }; 

            await _unitOfWork.Patients.AddAsync(patient);
            await _unitOfWork.CommitChangesAsync();

            var patientReadDto = _mapper.Map<PatientReadDto>(patient);
            return Result<PatientReadDto>.Ok(patientReadDto);

        }

        public async Task<Result<PatientReadDto>?> UpdateAsync(int id, PatientWriteDto dto, string currentUserId, bool isAdmin)
        {

            var patient = await _unitOfWork.Patients.GetOneAsync(pa => pa.Id == id);
            if (patient == null)
            {
                return Result<PatientReadDto>.Fail("Patient not found");
            }
            
            if (dto.Email != null)
            {
                if (dto.Email != patient.Person.Email && await _unitOfWork.Persons.IsEmailExistAsync(dto.Email))
                {
                    return Result<PatientReadDto>.Fail("Email already exist!", enErrorType.Conflict);
                }
            }

            var userId = patient?.Person?.User?.Id;
            if (currentUserId != userId && !isAdmin)
            {
                return Result<PatientReadDto>.Fail("You are not allowed to access this Patient.", enErrorType.Forbiden);
            }

            _mapper.Map(dto, patient.Person);

            _unitOfWork.Patients.Update(patient);
            await _unitOfWork.CommitChangesAsync();

            var patientReadDto = _mapper.Map<PatientReadDto>(patient);
            return Result<PatientReadDto>.Ok(patientReadDto);
        }

        public async Task<Result<PatientReadDto>?> DeleteAsync(int id)
        {
            var patient = await _unitOfWork.Patients.GetOneAsync(pa => pa.Id == id);
            if (patient == null) return Result<PatientReadDto>.Fail("Patient not found");

            _unitOfWork.Patients.Delete(patient);

            if(patient.Person != null)
                _unitOfWork.Persons.Delete(patient.Person);

            await _unitOfWork.CommitChangesAsync();

            return Result<PatientReadDto>.Ok(_mapper.Map<PatientReadDto>(patient));
        }

        public Task<Result<PatientReadDto>?> GetAsyncById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<PatientReadDto>?> UpdateAsync(int id, PatientWriteDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
