using Clinic_Project.Dtos.Prescription;
using Clinic_Project.Helpers;
using Clinic_Project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Clinic_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
   
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;
        public PrescriptionsController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        [Authorize(Roles = $"{RoleName.Doctor},{RoleName.Admin}")]
        [HttpGet]
        public async Task<IActionResult> GetPrescriptions()
        {
            var result = await _prescriptionService.GetAllAsync();
            return !result.Success ? NotFound(result.ErrorMessage) : Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPrescriptionById(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole(RoleName.Admin);

            var result = await _prescriptionService.GetAsyncById(id, currentUserId, isAdmin);

            if (!result!.Success)
            {
                return result.ErrorType == enErrorType.Conflict
                    ? Conflict(result.ErrorMessage)
                    : result.ErrorType == enErrorType.BadRequest
                    ? BadRequest(result.ErrorMessage)
                    : result.ErrorType == enErrorType.Forbiden
                    ? Forbid(result.ErrorMessage)
                    : NotFound(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [Authorize(Roles = $"{RoleName.Admin},{RoleName.Doctor}")]
        [HttpPost]
        public async Task<IActionResult> CreatePrescription(PrescriptionWriteDto dto)
        {
            var result = await _prescriptionService.CreateAsync(dto);

            if (!result!.Success)
            {
                return result.ErrorType == Helpers.enErrorType.Conflict
                    ? Conflict(result.ErrorMessage) : result.ErrorType == Helpers.enErrorType.BadRequest
                    ? BadRequest(result.ErrorMessage) : NotFound(result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetPrescriptionById), new { result.Data!.Id }, result.Data);
        }

        [Authorize(Roles = $"{RoleName.Admin},{RoleName.Doctor}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrescription(int id, PrescriptionUpdateWriteDto dto)
        {
            var result = await _prescriptionService.UpdateAsync(id, dto);

            if (!result!.Success)
            {
                return result.ErrorType == Helpers.enErrorType.Conflict
                    ? Conflict(result.ErrorMessage) : result.ErrorType == Helpers.enErrorType.BadRequest
                    ? BadRequest(result.ErrorMessage) : NotFound(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [Authorize(Roles = $"{RoleName.Admin},{RoleName.Doctor}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescription(int id)
        {
            var result = await _prescriptionService.DeleteAsync(id);

            return result!.Success ? NoContent() : NotFound(result.ErrorMessage);
        }
    }
}
 