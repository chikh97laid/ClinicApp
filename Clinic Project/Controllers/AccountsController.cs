using Clinic_Project.Dtos.Account;
using Microsoft.AspNetCore.Mvc;
using Clinic_Project.Services.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace Clinic_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("patients/register")]
        public async Task<IActionResult> RegisterPatient(RegisterDto dto)
        {
            var result = await _accountService.RegisterPatientAsync(dto);
            return !result.Success ? BadRequest(result.ErrorMessage) : 
                Ok(new { Message = "Patient Registered Successfully, Copy the link to confirm your email", ConfirmLink = result.Data});
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dtoLogin)
        {
            var result = await _accountService.LoginAsync(dtoLogin);
            return !result.Success ? BadRequest(result.ErrorMessage) : Ok(result.Data);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _accountService.GetProfileAsync(userId);
            return !result.Success ? NotFound(result.ErrorMessage) : Ok(result.Data);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _accountService.ConfirmEmailAsync(userId, token);

            if (!result.Success)
            {
                return result.ErrorType switch
                {
                    Helpers.enErrorType.Conflict => Conflict(result.ErrorMessage),
                    Helpers.enErrorType.BadRequest => BadRequest(result.ErrorMessage),
                    _ => NotFound(result.ErrorMessage),
                };
            }

            return Ok(new { message = "Email confirmed successfully" });
        }

        [HttpGet("resend-confirmation")]
        public async Task<IActionResult> ResendConfirmation(string email)
        {
            var result = await _accountService.ResendConfirmationAsync(email);

            if (!result.Success)
            {
                return result.ErrorType switch
                {
                    Helpers.enErrorType.Conflict => Conflict(result.ErrorMessage),
                    Helpers.enErrorType.BadRequest => BadRequest(result.ErrorMessage),
                    _ => NotFound(result.ErrorMessage),
                };
            }

            return Ok(new { token = result.Data});
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var result = await _accountService.ForgotPasswordAsync(email);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok(new {token = result.Data});
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var result = await _accountService.ResetPasswordAsync(dto);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await _accountService.RefreshTokenAsync(request);
            if (!result.Success)
            {
                return result.ErrorType switch
                {
                    Helpers.enErrorType.Conflict => Conflict(result.ErrorMessage),
                    Helpers.enErrorType.BadRequest => BadRequest(result.ErrorMessage),
                    Helpers.enErrorType.Unauthorized => Unauthorized(result.ErrorMessage),
                    _ => NotFound(result.ErrorMessage),
                };
            }

            return Ok(result.Data);
        }

    }
}
