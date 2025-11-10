using Clinic_Project.Dtos.Account;
using Clinic_Project.Helpers;
using Clinic_Project.Models;
using Clinic_Project.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clinic_Project.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        private ValidationResult? ValidateResult(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ValidationResult.Fail(errors, enErrorType.BadRequest);
            }

            return ValidationResult.Success();
        }

        public async Task<Result<IdentityResult?>> AddRoleAsync(RoleDto roleDto)
        {

            if (await _roleManager.Roles.AnyAsync(r => r.Name == roleDto.Name))
            {
                return Result<IdentityResult?>.Fail($"Role '{roleDto.Name}' already exists!", enErrorType.Conflict);
            }

            var role = new IdentityRole()
            {
                Name = roleDto.Name
            };

            var result = await _roleManager.CreateAsync(role);

            var validation = ValidateResult(result);

            if (!validation.IsValid) 
                return Result<IdentityResult?>.Fail(validation.Message, validation.ErrorType);

            return Result<IdentityResult?>.Ok(result);
        }

        public async Task<Result<IdentityResult?>> SetUserRolesAsync(UserRoleDto userRolDto)
        {

            var user = await _userManager.FindByIdAsync(userRolDto.UserId);

            if (user == null)
            {
                return Result<IdentityResult?>.Fail("User not found", enErrorType.NotFound);
            }

            var stringRolesList = userRolDto.Roles.Select(x => x.ToString()).ToList();

            var existingRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            var invalidRoles = stringRolesList.Where(r => !existingRoles.Contains(r)).ToList(); // because this is link roles method

            if (invalidRoles.Any())
            {
                return Result<IdentityResult?>.Fail($"Invalid Role: {string.Join(", ", invalidRoles)}", enErrorType.BadRequest);
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var newRoles = stringRolesList.Except(currentRoles).ToList();
            if (!newRoles.Any())
            {
                return Result<IdentityResult?>.Fail($"User already has all specified roles", enErrorType.BadRequest);
            }

            var result = await _userManager.AddToRolesAsync(user, newRoles);

            var validation = ValidateResult(result);
            if (!validation.IsValid) 
                return Result<IdentityResult?>.Fail(validation.Message, validation.ErrorType);

            return Result<IdentityResult?>.Ok(result);
        }
    }
}
