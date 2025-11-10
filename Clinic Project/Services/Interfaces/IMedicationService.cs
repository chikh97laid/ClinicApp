using Clinic_Project.Dtos.Medication;

namespace Clinic_Project.Services.Interfaces
{
    public interface IMedicationService : IService<MedicationReadDto, MedicationWriteDto>
    {
    }
}
