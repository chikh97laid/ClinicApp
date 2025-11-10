using Clinic_Project.Dtos.Account;
using Clinic_Project.Helpers;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;
using Clinic_Project.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Clinic_Project.Services.Implementations
{
    public class AccountService : IAccountService, IAdminAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(UserManager<AppUser> userManager, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IdentityResult?>> AddAdminAsync(AddAdminDto dto)
        {
            var appUser = new AppUser()
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(appUser, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description).ToList());
                return Result<IdentityResult?>.Fail(errors, enErrorType.BadRequest);
            }

            await _userManager.AddToRoleAsync(appUser, RoleName.Admin);

            return Result<IdentityResult?>.Ok(result);

        }

        public async Task<Result<string>> RegisterPatientAsync(RegisterDto dto)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var person = new Person()
                {
                    FirstName = dto.Person.FirstName,
                    LastName = dto.Person.LastName,
                    DateOfBirth = dto.Person.DateOfBirth,
                    Gender = dto.Person.Gender,
                    Email = dto.Email,
                    Phone = dto.PhoneNumber,
                    Address = dto.Person.Address
                };

                var patient = new Patient() { Person = person };

                await _unitOfWork.Patients.AddAsync(patient);
                await _unitOfWork.CommitChangesAsync();

                var appUser = new AppUser()
                {
                    UserName = dto.UserName,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    PersonId = person.Id
                };

                var result = await _userManager.CreateAsync(appUser, dto.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description).ToList());
                    await transaction.RollbackAsync();
                    return Result<string>.Fail(errors, enErrorType.BadRequest);
                }

                await _userManager.AddToRoleAsync(appUser, RoleName.Patient);

                // create confirm token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);

                var confirmLink = $"{_configuration["AppUrl"]}/api/account/confirm-email?userId={appUser.Id}&token={Uri.EscapeDataString(token)}";

                await transaction.CommitAsync();

                return Result<string>.Ok(confirmLink);

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result<string>.Fail(ex.Message, enErrorType.BadRequest);
            }

        }

        public async Task<Result<IdentityResult?>> AddDoctorAsync(RegisterDto dto)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var person = new Person()
                {
                    FirstName = dto.Person.FirstName,
                    LastName = dto.Person.LastName,
                    DateOfBirth = dto.Person.DateOfBirth,
                    Gender = dto.Person.Gender,
                    Email = dto.Email,
                    Phone = dto.PhoneNumber,
                    Address = dto.Person.Address
                };

                var doctor = new Doctor() { Person = person };

                await _unitOfWork.Doctors.AddAsync(doctor);
                await _unitOfWork.CommitChangesAsync();

                var appUser = new AppUser()
                {
                    UserName = dto.UserName,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    PersonId = person.Id,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(appUser, dto.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description).ToList());
                    await transaction.RollbackAsync();
                    return Result<IdentityResult?>.Fail(errors, enErrorType.BadRequest);
                }

                await _userManager.AddToRoleAsync(appUser, RoleName.Doctor);

                await transaction.CommitAsync();

                return Result<IdentityResult?>.Ok(result);

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result<IdentityResult?>.Fail(ex.Message, enErrorType.BadRequest);
            }

        }

        private async Task<JwtSecurityToken> CreateJwtToken(AppUser user)
        {
            // ===== Access Token =====
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user!.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]!));

            var token = new JwtSecurityToken(
                claims: claims,    // Named Arguments Syntax
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

        private async Task<RefreshToken> GenerateRefreshToken(AppUser user)
        {
            // ===== Refresh Token =====
            var refreshToken = new RefreshToken()
            {
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7), // expiration period
                IsRevoked = false,
                IsUsed = false,
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
            };

            await _unitOfWork.RefreshTokens.AddAsync(refreshToken);
            await _unitOfWork.CommitChangesAsync();

            return refreshToken;
        }

        public async Task<Result<TokenResponse?>> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserNameOrEmail);
            user ??= await _userManager.FindByEmailAsync(dto.UserNameOrEmail);

            if (user == null)
            {
                return Result<TokenResponse?>.Fail("UserName/Email is invalid!", enErrorType.BadRequest);
            }

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return Result<TokenResponse?>.Fail("Incorrect password", enErrorType.BadRequest);
            }

            // ===== Access Token ===== 
            var jwtToken = await CreateJwtToken(user);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            // ===== Refresh Token =====
            var refreshToken = await GenerateRefreshToken(user);

            return Result<TokenResponse?>.Ok(new TokenResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                Expiration = jwtToken.ValidTo 
            });

        }

        public async Task<Result<IEnumerable<AccountDto>>> GetAllAccountsAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var accounts = new List<AccountDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                accounts.Add(new AccountDto()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    Roles = roles.ToList()
                });
            }

            return Result<IEnumerable<AccountDto>>.Ok(accounts);
        }

        public async Task<Result<bool>> ToggleAccountStatusAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) 
                return Result<bool>.Fail("User not found", enErrorType.NotFound);
        
            if(user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
            {
                user.LockoutEnd = null;
            }
            else
            {
                user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(1);
            }

            await _userManager.UpdateAsync(user);
            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> DeleteAccountAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return Result<bool>.Fail("User not found", enErrorType.NotFound);

            await _userManager.DeleteAsync(user);
            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> ConfirmEmailAsync(string id, string token)
        {
            var user = await _userManager.FindByIdAsync(id);
            
            if (user == null)
                return Result<bool>.Fail("User not found", enErrorType.NotFound);

            if (user.EmailConfirmed)
                return Result<bool>.Fail("Email already confirmed", enErrorType.Conflict);

            token = Uri.UnescapeDataString(token);

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                return Result<bool>.Fail("Invalid token", enErrorType.BadRequest);
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<string>> ResendConfirmationAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Result<string>.Fail("User not found", enErrorType.NotFound);

            if (user.EmailConfirmed)
                return Result<string>.Fail("Email already confirmed", enErrorType.Conflict);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmLink = $"{_configuration["AppUrl"]}/api/account/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            return Result<string>.Ok(confirmLink);
        }

        public async Task<Result<string>> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result<string>.Fail("User not found", enErrorType.NotFound);

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"{_configuration["AppUrl"]}/api/account/forgot-password?userId={user.Id}&token={Uri.EscapeDataString(resetToken)}";

            return Result<string>.Ok(resetLink);
        }

        public async Task<Result<string>> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
                return Result<string>.Fail("User not found", enErrorType.NotFound);

            dto.Token = Uri.UnescapeDataString(dto.Token);

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);

            if (!result.Succeeded)
            {
                return Result<string>.Fail("Invalid token", enErrorType.BadRequest);
            }

            return Result<string>.Ok("Password has been reset successfully.");
        }

        public async Task<Result<AccountDto>> GetProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Result<AccountDto>.Fail("User not found", enErrorType.NotFound);

            var roles = await _userManager.GetRolesAsync(user);

            var accountDto = new AccountDto()
            {
                Id = userId,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Roles = roles.ToList()
            };

            return Result<AccountDto>.Ok(accountDto);
        }        

        public async Task<Result<TokenResponse>> RefreshTokenAsync(RefreshTokenRequestDto Request)
        {
            var refreshToken = await _unitOfWork.RefreshTokens
                              .GetOneAsync(rt => rt.Token == Request.RefreshToken);

            if(refreshToken == null || refreshToken.IsUsed || refreshToken.IsRevoked)
            {
                return Result<TokenResponse>.Fail("Invalid refresh token", enErrorType.Unauthorized);
            }

            if(refreshToken.ExpiresAt < DateTime.UtcNow)
            {
                return Result<TokenResponse>.Fail("Refresh token expired", enErrorType.Unauthorized);
            }

            var user = await _userManager.FindByIdAsync(refreshToken.UserId);

            if(user is null)
                return Result<TokenResponse>.Fail("User not found", enErrorType.NotFound);

            // mark old token as used
            refreshToken.IsUsed = true;
            _unitOfWork.RefreshTokens.Update(refreshToken);

            // generate new access token and new refresh token
            var jwtToken = await CreateJwtToken(user);
            var newAccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            
            var newRefreshToken = new RefreshToken()
            {
                UserId = user.Id,
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                IsUsed = false
            };

            await _unitOfWork.RefreshTokens.AddAsync(newRefreshToken);
            await _unitOfWork.CommitChangesAsync();

            var respons = new TokenResponse()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token,
                Expiration = jwtToken.ValidTo,
            };

            return Result<TokenResponse>.Ok(respons);
        }

    }
}
