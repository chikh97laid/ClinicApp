using Clinic_Project.Dtos.Account;
using Clinic_Project.Helpers;
using Clinic_Project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_Project.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole(RoleDto roleDto)
        {
            var result = await _roleService.AddRoleAsync(roleDto);
            return !result.Success ? BadRequest(result.ErrorMessage) : Ok(new { Message = "Role added successfully", roleDto.Name });
        }

        [HttpPost("SetUserRoles")]
        //[Authorize(Roles = RoleName.Admin)]
        public async Task<IActionResult> SetUserRoles(UserRoleDto usRolDto)
        {
            var result = await _roleService.SetUserRolesAsync(usRolDto);
            if (!result.Success)
            {
                return result.ErrorType switch
                {
                    enErrorType.Conflict => Conflict(result.ErrorMessage),
                    enErrorType.BadRequest => BadRequest(result.ErrorMessage),
                    _ => NotFound(result.ErrorMessage)
                };
            }

            return Ok("Role(s) Added to user successfully");

        }
    }
}
