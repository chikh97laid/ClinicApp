using Clinic_Project.Dtos.Account;
using Clinic_Project.Helpers;
using Microsoft.AspNetCore.Identity;

namespace Clinic_Project.Services.Interfaces
{
    public interface IRoleService
    {
        Task<Result<IdentityResult?>> AddRoleAsync(RoleDto roleDto);
        Task<Result<IdentityResult?>> SetUserRolesAsync(UserRoleDto usRolDto);
    }
}
