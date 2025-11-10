using Clinic_Project.Dtos.Account;
using Clinic_Project.Helpers;
using Microsoft.AspNetCore.Identity;

namespace Clinic_Project.Services.Interfaces
{
    public interface IAdminAccountService
    {
        Task<Result<IdentityResult?>> AddAdminAsync(AddAdminDto dto);
        Task<Result<IdentityResult?>> AddDoctorAsync(RegisterDto dto);
        Task<Result<IEnumerable<AccountDto>>> GetAllAccountsAsync();
        Task<Result<bool>> ToggleAccountStatusAsync(string id);
        Task<Result<bool>> DeleteAccountAsync(string id);
    }
}
