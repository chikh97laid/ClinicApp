using Clinic_Project.Dtos.Account;
using Clinic_Project.Helpers;

namespace Clinic_Project.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Result<string>> RegisterPatientAsync(RegisterDto dto);
        Task<Result<TokenResponse?>> LoginAsync(LoginDto dto);
        Task<Result<bool>> ConfirmEmailAsync(string id, string token);
        Task<Result<string>> ResendConfirmationAsync(string email);
        Task<Result<string>> ForgotPasswordAsync(string email);
        Task<Result<string>> ResetPasswordAsync(ResetPasswordDto dto);
        Task<Result<AccountDto>> GetProfileAsync(string userId);
        Task<Result<TokenResponse>> RefreshTokenAsync(RefreshTokenRequestDto Request);
    }
}
