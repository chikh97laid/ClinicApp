using Clinic_Project.Dtos.PaymentMethod;
using Clinic_Project.Helpers;
using Clinic_Project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class PaymentMethodsController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;
        public PaymentMethodsController(IPaymentMethodService aymentMethodService)
        {
            _paymentMethodService = aymentMethodService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaymentMethods()
        {
            var result = await _paymentMethodService.GetAllAsync();
            return !result.Success ? NotFound(result.ErrorMessage) : Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentMethodById(int id)
        {
            var result = await _paymentMethodService.GetAsyncById(id);
            return !result!.Success ? NotFound(result.ErrorMessage) : Ok(result.Data);
        }

        [HttpPost]
        [Authorize(Roles = RoleName.Admin)]
        public async Task<IActionResult> CreatePaymentMethod(PaymentMethodWriteDto dto)
        {
            var result = await _paymentMethodService.CreateAsync(dto);

            if (!result!.Success)
            {
                return result.ErrorType == Helpers.enErrorType.Conflict
                    ? Conflict(result.ErrorMessage) : result.ErrorType == Helpers.enErrorType.BadRequest
                    ? BadRequest(result.ErrorMessage) : NotFound(result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetPaymentMethodById), new { result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = RoleName.Admin)]
        public async Task<IActionResult> UpdatePaymentMethod(int id, PaymentMethodWriteDto dto)
        {
            var result = await _paymentMethodService.UpdateAsync(id, dto);

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
        public async Task<IActionResult> DeletePaymentMethod(int id)
        {
            var result = await _paymentMethodService.DeleteAsync(id);

            return result!.Success ? NoContent() : NotFound(result.ErrorMessage);
        }
    }
}
