using Clinic_Project.Models;

namespace Clinic_Project.Repositories.Interfaces
{
    public interface ISpecializationRepo : IRepo<Specialization>
    {
        Task<bool> IsSpecializationExistAsync(string name);
    }
}
