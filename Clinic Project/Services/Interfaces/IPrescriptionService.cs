using Clinic_Project.Dtos.Prescription;
using Clinic_Project.Helpers;

namespace Clinic_Project.Services.Interfaces
{
    public interface IPrescriptionService : IService<PrescriptionReadDto, PrescriptionWriteDto>
    {
        Task<Result<PrescriptionReadDto>?> UpdateAsync(int id, PrescriptionUpdateWriteDto dto);
        Task<Result<PrescriptionReadDto>?> GetAsyncById(int id, string currentUserId, bool isAdmin);
    }
}
