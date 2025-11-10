using Clinic_Project.Models;

namespace Clinic_Project.Repositories.Interfaces
{
    public interface IPrescriptionMedicationRepo : IRepo<PrescriptionMedication>
    {
        Task<bool> IsForeignKeysRepeated(int fk1, int fk2);
    }
}
