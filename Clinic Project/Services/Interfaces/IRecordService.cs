using Clinic_Project.Dtos.Record;
using Clinic_Project.Helpers;
using Clinic_Project.Models;

namespace Clinic_Project.Services.Interfaces
{
    public interface IRecordService : IService<RecordReadDto, RecordWriteDto>
    {
        Task<Result<RecordReadDto>?> UpdateAsync(int id, RecordUpdateWriteDto dto);
        Task<Result<RecordReadDto>?> GetAsyncById(int id, string currentUserId, bool isAdmin);


    }
}
