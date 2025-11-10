using Clinic_Project.Dtos.PrescriptionMedication;
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
   
    public class PrescriptionMedicationsController : ControllerBase
    {
        private readonly IPrescriptionMedicationService _prescriptionMedicationService;
        public PrescriptionMedicationsController(IPrescriptionMedicationService prescriptionMedicationService)
        {
            _prescriptionMedicationService = prescriptionMedicationService;
        }

        [Authorize(Roles = $"{RoleName.Doctor},{RoleName.Admin}")]
        [HttpGet]
        public async Task<IActionResult> GetPrescriptionMedications()
        {
            var result = await _prescriptionMedicationService.GetAllAsync();
            return !result.Success ? NotFound(result.ErrorMessage) : Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPrescriptionMedicationById(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole(RoleName.Admin);

            var result = await _prescriptionMedicationService.GetAsyncById(id, currentUserId, isAdmin);

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
        public async Task<IActionResult> CreatePrescriptionMedication(PrescriptionMedicationWriteDto dto)
        {
            var result = await _prescriptionMedicationService.CreateAsync(dto);

            if (!result!.Success)
            {
                return result.ErrorType == Helpers.enErrorType.Conflict
                    ? Conflict(result.ErrorMessage) : result.ErrorType == Helpers.enErrorType.BadRequest
                    ? BadRequest(result.ErrorMessage) : NotFound(result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetPrescriptionMedicationById), new { result.Data!.Id }, result.Data);
        }

        [Authorize(Roles = $"{RoleName.Admin},{RoleName.Doctor}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrescriptionMedication(int id, PrescriptionMedicationBaseDto dto)
        {
            var result = await _prescriptionMedicationService.UpdateAsync(id, dto);

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
        public async Task<IActionResult> DeletePrescriptionMedication(int id)
        {
            var result = await _prescriptionMedicationService.DeleteAsync(id);

            return result!.Success ? NoContent() : NotFound(result.ErrorMessage);
        }
    }
}
