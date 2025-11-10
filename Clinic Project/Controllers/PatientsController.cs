using Clinic_Project.Dtos.Patient;
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
   
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        [Authorize(Roles = RoleName.Admin)]
        public async Task<IActionResult> GetPatients()
        {
            var result = await _patientService.GetAllAsync();
            return !result.Success ? NotFound(result.ErrorMessage) : Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole(RoleName.Admin);

            var result = await _patientService.GetAsyncById(id, currentUserId, isAdmin);

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

        [HttpPost]
        [Authorize(Roles = RoleName.Admin)]
        public async Task<IActionResult> CreatePatient(PatientWriteDto dto)
        {
            var result = await _patientService.CreateAsync(dto);

            if (!result!.Success)
            {
                return result.ErrorType == Helpers.enErrorType.Conflict
                    ? Conflict(result.ErrorMessage) : result.ErrorType == Helpers.enErrorType.BadRequest
                    ? BadRequest(result.ErrorMessage) : NotFound(result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetPatientById), new { result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{RoleName.Admin}, {RoleName.Patient}")]
        public async Task<IActionResult> UpdatePatient(int id, PatientWriteDto dto)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole(RoleName.Admin);

            var result = await _patientService.UpdateAsync(id, dto, currentUserId, isAdmin);

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

        [HttpDelete("{id}")]
        [Authorize(Roles = RoleName.Admin)]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var result = await _patientService.DeleteAsync(id);

            return result!.Success ? NoContent() : NotFound(result.ErrorMessage);
        }

    }
}
