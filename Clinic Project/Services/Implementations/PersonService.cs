using AutoMapper;
using Clinic_Project.Dtos.Person;
using Clinic_Project.Helpers;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;
using Clinic_Project.Services.Interfaces;

namespace Clinic_Project.Services.Implementations
{
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PersonService(IUnitOfWork unitOfWork, IMapper mapper)
        { 
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public async Task<Result<IEnumerable<PersonReadDto>?>> GetAllAsync()
        {
            var persons = await _unitOfWork.Persons.GetAllAsync();

            var PersonsReadDto = _mapper.Map<IEnumerable<PersonReadDto>>(persons);

            return !persons!.Any() ? Result<IEnumerable<PersonReadDto>?>.Fail("No Persons!", enErrorType.NotFound) 
                                   : Result<IEnumerable<PersonReadDto>?>.Ok(PersonsReadDto); 
        }

        public async Task<Result<PersonReadDto>?> GetAsyncById(int id)
        {
            var person = await _unitOfWork.Persons.GetOneAsync(p => p.Id == id);

            var personReadDto = _mapper.Map<PersonReadDto>(person);

            return person == null ? Result<PersonReadDto>.Fail("Person not found", enErrorType.NotFound) 
                                  : Result<PersonReadDto>.Ok(personReadDto);
        }

        public async Task<Result<PersonReadDto>?> CreateAsync(PersonWriteDto personDto)
        {
            if(personDto.Email != null)
            {
                if (await _unitOfWork.Persons.IsEmailExistAsync(personDto.Email))
                {
                    return Result<PersonReadDto>.Fail("Email already exist!", enErrorType.Conflict);
                }
            }

            if(personDto.DateOfBirth > DateTime.Now)
            {
                return Result<PersonReadDto>.Fail("Invalid Date!", enErrorType.BadRequest);
            }

            var person = _mapper.Map<Person>(personDto);

            await _unitOfWork.Persons.AddAsync(person);
            await _unitOfWork.CommitChangesAsync();

            var personReadDto = _mapper.Map<PersonReadDto>(person);

            return Result<PersonReadDto>.Ok(personReadDto);
        }

        public async Task<Result<PersonReadDto>?> UpdateAsync(int id, PersonWriteDto personDto)
        {

            var person = await _unitOfWork.Persons.GetOneAsync(p => p.Id == id);
            if(person == null)
            {
                return Result<PersonReadDto>.Fail("Person not found");
            }

            if (personDto.Email != null)
            {
                if (personDto.Email != person.Email && await _unitOfWork.Persons.IsEmailExistAsync(personDto.Email))
                {
                    return Result<PersonReadDto>.Fail("Email already exist!", enErrorType.Conflict);
                }
            }

            if (personDto.DateOfBirth > DateTime.Now)
            {
                return Result<PersonReadDto>.Fail("Invalid Date!", enErrorType.BadRequest);
            }

            _mapper.Map(personDto, person);

            _unitOfWork.Persons.Update(person);
            await _unitOfWork.CommitChangesAsync();

            var personReadDto = _mapper.Map<PersonReadDto>(person);

            return Result<PersonReadDto>.Ok(personReadDto);
        }


        public async Task<Result<PersonReadDto>?> DeleteAsync(int id)
        {
            var person = await _unitOfWork.Persons.GetOneAsync(p => p.Id == id);
            if(person == null) return Result<PersonReadDto>.Fail("Person not found");

            _unitOfWork.Persons.Delete(person);
            await _unitOfWork.CommitChangesAsync();

            return Result<PersonReadDto>.Ok(_mapper.Map<PersonReadDto>(person));
        }

    }
}
