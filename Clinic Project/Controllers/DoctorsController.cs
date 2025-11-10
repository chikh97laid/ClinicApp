using Clinic_Project.Dtos.Doctor;
using Clinic_Project.Helpers;
using Clinic_Project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctors()
        {
            var result = await _doctorService.GetAllAsync();
            return !result.Success ? NotFound(result.ErrorMessage) : Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var result = await _doctorService.GetAsyncById(id);
            return !result!.Success ? NotFound(result.ErrorMessage) : Ok(result.Data);
        }

        [HttpPost]
        [Authorize(Roles = RoleName.Admin)]
        public async Task<IActionResult> CreateDoctor(DoctorWriteDto dto)
        {
            var result = await _doctorService.CreateAsync(dto);

            if (!result!.Success)
            {
                return result.ErrorType == Helpers.enErrorType.Conflict
                    ? Conflict(result.ErrorMessage) : result.ErrorType == Helpers.enErrorType.BadRequest
                    ? BadRequest(result.ErrorMessage) : NotFound(result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetDoctorById), new { result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = RoleName.Admin)]
        public async Task<IActionResult> UpdateDoctor(int id, DoctorWriteDto dto)
        {
            var result = await _doctorService.UpdateAsync(id, dto);

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
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var result = await _doctorService.DeleteAsync(id);

            return result!.Success ? NoContent() : NotFound(result.ErrorMessage);
        }
    }
}
