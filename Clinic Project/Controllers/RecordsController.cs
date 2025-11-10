using Clinic_Project.Dtos.Record;
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
 
    public class RecordsController : ControllerBase
    {
        private readonly IRecordService _recordService;
        public RecordsController(IRecordService recordService)
        {
            _recordService = recordService;
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleName.Doctor},{RoleName.Admin}")]
        public async Task<IActionResult> GetRecords()
        {
            var result = await _recordService.GetAllAsync();
            return !result.Success ? NotFound(result.ErrorMessage) : Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecordById(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole(RoleName.Admin);

            var result = await _recordService.GetAsyncById(id, currentUserId, isAdmin);

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
        [Authorize(Roles = $"{RoleName.Admin},{RoleName.Doctor}")]
        public async Task<IActionResult> CreateRecord(RecordWriteDto dto)
        {
            var result = await _recordService.CreateAsync(dto);

            if (!result!.Success)
            {
                return result.ErrorType == enErrorType.Conflict
                    ? Conflict(result.ErrorMessage) : result.ErrorType == enErrorType.BadRequest
                    ? BadRequest(result.ErrorMessage) : NotFound(result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetRecordById), new { result.Data!.Id }, result.Data);
        }

        [Authorize(Roles = $"{RoleName.Admin},{RoleName.Doctor}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecord(int id, RecordUpdateWriteDto dto)
        {
            var result = await _recordService.UpdateAsync(id, dto);

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
        public async Task<IActionResult> DeleteRecord(int id)
        {
            var result = await _recordService.DeleteAsync(id);

            return result!.Success ? NoContent() : NotFound(result.ErrorMessage);
        }
    }
}
