using Clinic_Project.Models;

namespace Clinic_Project.Repositories.Interfaces
{
    public interface IMedicationRepo : IRepo<Medication>
    {
        Task<bool> IsMedicationNameExistAsync(string Name);
    }
}
