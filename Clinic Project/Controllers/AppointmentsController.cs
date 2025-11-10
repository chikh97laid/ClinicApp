using Clinic_Project.Dtos.Appointment;
using Clinic_Project.Helpers;
using Clinic_Project.Services.Implementations;
using Clinic_Project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Clinic_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        [Authorize(RoleName.Admin)]
        public async Task<IActionResult> GetAppointments()
        {
            var result = await _appointmentService.GetAllAsync();
            return !result.Success ? NotFound(result.ErrorMessage) : Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole(RoleName.Admin);

            var result = await _appointmentService.GetAsyncById(id, currentUserId, isAdmin);

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
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetByDoctor(int doctorId)
        {
            var result = await _appointmentService.GetAppointmentsAsyncByDoctorId(doctorId);
            return !result.Success ? NotFound(result.ErrorMessage) : Ok(result.Data);
        }

        [Authorize(Roles = $"{RoleName.Admin},{RoleName.Patient}")]
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var result = await _appointmentService.GetAppointmentsAsyncByPatientId(patientId);
            return !result.Success ? NotFound(result.ErrorMessage) : Ok(result.Data);
        }


        [HttpPost]
        [Authorize(Roles = $"{RoleName.Admin},{RoleName.Patient}")]
        public async Task<IActionResult> CreateAppointment(AppointmentWriteDto dto)
        {
            var result = await _appointmentService.CreateAsync(dto);

            if (!result!.Success)
            {
                return result.ErrorType == Helpers.enErrorType.Conflict
                    ? Conflict(result.ErrorMessage) : result.ErrorType == Helpers.enErrorType.BadRequest
                    ? BadRequest(result.ErrorMessage) : NotFound(result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetAppointmentById), new { result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, AppointmentWriteDto dto)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole(RoleName.Admin);
         
            var result = await _appointmentService.UpdateAsync(id, dto, currentUserId, isAdmin);

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

        [Authorize(Roles = RoleName.Admin)]
        [HttpPut("{id}/Confirm")]
        public async Task<IActionResult> ConfirmAppointment(int id, [FromBody] DateTime scheduleDate)
        {
            var result = await _appointmentService.ConfirmAppointmentAsync(id, scheduleDate);

            if (!result!.Success)
            {
                return result.ErrorType switch
                {
                    enErrorType.Conflict => Conflict(result.ErrorMessage),
                    enErrorType.BadRequest => BadRequest(result.ErrorMessage),
                    _ => NotFound(result.ErrorMessage)
                };
            }

            return Ok(result.Data);
        }

        [Authorize(Roles = $"{RoleName.Admin},{RoleName.Doctor}")]
        [HttpPut("{id}/Complete")]
        public async Task<IActionResult> CompleteAppointment(int id)
        {
            var result = await _appointmentService.CompleteAppointmentAsync(id);

            if (!result!.Success)
            {
                return result.ErrorType switch
                {
                    enErrorType.Conflict => Conflict(result.ErrorMessage),
                    enErrorType.BadRequest => BadRequest(result.ErrorMessage),
                    _ => NotFound(result.ErrorMessage)
                };
            }

            return Ok(result.Data);
        }

        [HttpPut("{id}/NoShow")]
        [Authorize(Roles = $"{RoleName.Admin},{RoleName.Doctor}")]
        public async Task<IActionResult> NoShowAppointment(int id)
        {
            var result = await _appointmentService.NoShowAppointmentAsync(id);

            if (!result!.Success)
            {
                return result.ErrorType switch
                {
                    enErrorType.Conflict => Conflict(result.ErrorMessage),
                    enErrorType.BadRequest => BadRequest(result.ErrorMessage),
                    _ => NotFound(result.ErrorMessage)
                };
            }

            return Ok(result.Data);
        }

        [HttpPut("{id}/Reschedule")]
        [Authorize(Roles = $"{RoleName.Admin},{RoleName.Doctor}")]
        public async Task<IActionResult> RescheduleAppointment(int id, [FromBody] DateTime newDate)
        {
            var result = await _appointmentService.RescheduleAppointmentAsync(id, newDate);

            if (!result!.Success)
            {
                return result.ErrorType switch
                {
                    enErrorType.Conflict => Conflict(result.ErrorMessage),
                    enErrorType.BadRequest => BadRequest(result.ErrorMessage),
                    _ => NotFound(result.ErrorMessage)
                };
            }

            return Ok(result.Data);
        }

        [HttpPut("{id}/Cancel")]
        [Authorize(Roles = $"{RoleName.Admin},{RoleName.Doctor}")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var result = await _appointmentService.CancelAppointmentAsync(id);

            if (!result!.Success)
            {
                return result.ErrorType switch
                {
                    enErrorType.Conflict => Conflict(result.ErrorMessage),
                    enErrorType.BadRequest => BadRequest(result.ErrorMessage),
                    _ => NotFound(result.ErrorMessage)
                };
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = RoleName.Admin)]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var result = await _appointmentService.DeleteAsync(id);

            return result!.Success ? NoContent() : NotFound(result.ErrorMessage);
        }
    }
}
