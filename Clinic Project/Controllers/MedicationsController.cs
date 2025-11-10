using Clinic_Project.Dtos.Medication;
using Clinic_Project.Helpers;
using Clinic_Project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MedicationsController : ControllerBase
    {
        private readonly IMedicationService _medicationService;
        public MedicationsController(IMedicationService medicationService)
        {
            _medicationService = medicationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMedications()
        {
            var result = await _medicationService.GetAllAsync();
            return !result.Success ? NotFound(result.ErrorMessage) : Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedicationById(int id)
        {
            var result = await _medicationService.GetAsyncById(id);
            return !result!.Success ? NotFound(result.ErrorMessage) : Ok(result.Data);
        }

        [HttpPost]
        [Authorize(Roles = RoleName.Admin)]
        public async Task<IActionResult> CreateMedication(MedicationWriteDto dto)
        {
            var result = await _medicationService.CreateAsync(dto);

            if (!result!.Success)
            {
                return result.ErrorType == Helpers.enErrorType.Conflict
                    ? Conflict(result.ErrorMessage) : result.ErrorType == Helpers.enErrorType.BadRequest
                    ? BadRequest(result.ErrorMessage) : NotFound(result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetMedicationById), new { result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = RoleName.Admin)]
        public async Task<IActionResult> UpdateMedication(int id, MedicationWriteDto dto)
        {
            var result = await _medicationService.UpdateAsync(id, dto);

            if (!result!.Success)
            {
                return result.ErrorType == Helpers.enErrorType.Conflict
                    ? Conflict(result.ErrorMessage) : result.ErrorType == Helpers.enErrorType.BadRequest
                    ? BadRequest(result.ErrorMessage) : NotFound(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = RoleName.Admin)]
        public async Task<IActionResult> DeleteMedication(int id)
        {
            var result = await _medicationService.DeleteAsync(id);

            return result!.Success ? NoContent() : NotFound(result.ErrorMessage);
        }
    }
}
