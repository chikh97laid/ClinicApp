using Clinic_Project.Dtos.Patient;
using Clinic_Project.Dtos.Payment;
using Clinic_Project.Helpers;

namespace Clinic_Project.Services.Interfaces
{
    public interface IPatientService : IService<PatientReadDto, PatientWriteDto>
    {
        Task<Result<PatientReadDto>?> GetAsyncById(int id, string currentUserId, bool isAdmin);
        Task<Result<PatientReadDto>?> UpdateAsync(int id, PatientWriteDto dto, string currentUserId, bool isAdmin);
    }
}

