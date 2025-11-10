using Clinic_Project.Dtos.PrescriptionMedication;
using Clinic_Project.Helpers;

namespace Clinic_Project.Services.Interfaces
{
    public interface IPrescriptionMedicationService : IService<PrescriptionMedicationReadDto, PrescriptionMedicationWriteDto>
    {
        Task<Result<PrescriptionMedicationReadDto>?> UpdateAsync(int id, PrescriptionMedicationBaseDto dto);
        Task<Result<PrescriptionMedicationReadDto>?> GetAsyncById(int id, string currentUserId, bool isAdmin);
    }
}
