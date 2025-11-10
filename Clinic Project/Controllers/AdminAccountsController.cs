using Clinic_Project.Dtos.Account;
using Clinic_Project.Helpers;
using Clinic_Project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Clinic_Project.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    [Route("api/admin/accounts")]
    [ApiController]
    public class AdminAccountsController : ControllerBase
    {
        private readonly IAdminAccountService _adminAccountService;
        public AdminAccountsController(IAdminAccountService adminAccountService)
        {
            _adminAccountService = adminAccountService; 
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            var result = await _adminAccountService.GetAllAccountsAsync();
            return !result.Success ? BadRequest(result.ErrorMessage) : Ok(result.Data);
        }

        [HttpPost("admins")]
        public async Task<IActionResult> AddAdmin(AddAdminDto dto)
        {
            var result = await _adminAccountService.AddAdminAsync(dto);
            return !result.Success ? BadRequest(result.ErrorMessage) : Ok(new { Message = "Admin Added Successfully" });
        }

        [HttpPost("doctors")]
        public async Task<IActionResult> AddDoctor(RegisterDto dto)
        {
            var result = await _adminAccountService.AddDoctorAsync(dto);
            return !result.Success ? BadRequest(result.ErrorMessage) : Ok(new { Message = "Doctor Added Successfully" });
        }

        [HttpPost("{id}/toggle-status")]
        public async Task<IActionResult> ToggleAccountStatus(string id)
        {
            var result = await _adminAccountService.ToggleAccountStatusAsync(id);
            return !result.Success ? BadRequest(result.ErrorMessage) : Ok(result.Success);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var result = await _adminAccountService.DeleteAccountAsync(id);
            return !result.Success ? BadRequest(result.ErrorMessage) : Ok(result.Success);
        }

    }
}
