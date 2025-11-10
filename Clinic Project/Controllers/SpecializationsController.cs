using Clinic_Project.Dtos.Specialization;
using Clinic_Project.Helpers;
using Clinic_Project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class SpecializationsController : ControllerBase
    {
        private readonly ISpecializationService _specializationService;
        public SpecializationsController(ISpecializationService specializationService)
        {
            _specializationService = specializationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecializations()
        {
            var result = await _specializationService.GetAllAsync();
            return !result.Success ? NotFound(result.ErrorMessage) : Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecializationById(int id)
        {
            var result = await _specializationService.GetAsyncById(id);
            return !result!.Success ? NotFound(result.ErrorMessage) : Ok(result.Data);
        }

        [HttpPost]
        [Authorize(Roles = RoleName.Admin)]
        public async Task<IActionResult> CreateSpecialization(SpecializationWriteDto dto)
        {
            var result = await _specializationService.CreateAsync(dto);

            if (!result!.Success)
            {
                return result.ErrorType == Helpers.enErrorType.Conflict
                    ? Conflict(result.ErrorMessage) : result.ErrorType == Helpers.enErrorType.BadRequest
                    ? BadRequest(result.ErrorMessage) : NotFound(result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetSpecializationById), new { result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = RoleName.Admin)]
        public async Task<IActionResult> UpdateSpecialization(int id, SpecializationWriteDto dto)
        {
            var result = await _specializationService.UpdateAsync(id, dto);

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
        public async Task<IActionResult> DeleteSpecialization(int id)
        {
            var result = await _specializationService.DeleteAsync(id);

            return result!.Success ? NoContent() : NotFound(result.ErrorMessage);
        }
    }
}
