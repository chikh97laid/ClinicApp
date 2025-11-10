using Clinic_Project.Dtos.Person;
using Clinic_Project.Helpers;

namespace Clinic_Project.Services.Interfaces
{
    public interface IService<TReadDto, TWriteDto>
        where TReadDto : class
        where TWriteDto : class
    {
        Task<Result<IEnumerable<TReadDto>?>> GetAllAsync();
        Task<Result<TReadDto>?> GetAsyncById(int id);
        Task<Result<TReadDto>?> CreateAsync(TWriteDto dto);
        Task<Result<TReadDto>?> UpdateAsync(int id, TWriteDto dto);
        Task<Result<TReadDto>?> DeleteAsync(int id);
    }
}
