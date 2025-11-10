using Clinic_Project.Models;

namespace Clinic_Project.Repositories.Interfaces
{
    public interface IPersonRepo : IRepo<Person>
    {
        Task<bool> IsEmailExistAsync(string email); 
    }
}
