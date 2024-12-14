using Application.Features.DTOs;
using Application.Features.Interfaces.IRepositores;
using Application.Features.Interfaces.IService;
using Domain.Entities;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;


namespace Application.Features.Queries.Services.Implementation
{
    public class UserService(UserManager<ApplicationUser> _userManager, IHttpContextAccessor _assessor, IUserRepository _user, IStringLocalizer<UserService> _localizer, IEmailService _emailService, IOptions<JWT> _jwt) : IUserService
    {
        public async Task<ApplicationUser> GetCurrentUser()
        {
            
            var userId = _assessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId == null ? null : await _userManager.FindByIdAsync(userId);
        }


        public async Task<BaseResponse<TokenResponse>> LoginAsync(LoginRequestModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new BaseResponse<TokenResponse> { IsSuccessful = false, Message = (_localizer["User Not Found."]) };
            }
            if (!user.IsActive)
            {
                return new BaseResponse<TokenResponse> { IsSuccessful = false, Message = (_localizer["User Not Active. Please contact the administrator."]) };
            }
            if (!user.EmailConfirmed)
            {
                return new BaseResponse<TokenResponse> { Message = (_localizer["E-Mail not confirmed."]), IsSuccessful = false };
            }
            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                return new BaseResponse<TokenResponse> { IsSuccessful = false, Message = (_localizer["Invalid Credentials."]) };
            }

            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            var token = await GenerateJwtAsync(user);
            var response = new TokenResponse { Token = token, RefreshToken = user.RefreshToken, UserName = user.FullName };
            return new BaseResponse<TokenResponse> { IsSuccessful = true, Result = response };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }


        public async Task<string> GenerateJwtAsync(ApplicationUser user)
        {
            var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
            return token;
        }
        private SigningCredentials GetSigningCredentials()
        {
            var secret = Encoding.UTF8.GetBytes(_jwt.Value.Key);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }
        public async Task<IEnumerable<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var role = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new("role", role.First()),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.FullName),
            };
            return claims;
        }

        /*private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
               claims: claims,
               expires: DateTime.UtcNow.AddDays(2),
               signingCredentials: signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            var encryptedToken = tokenHandler.WriteToken(token);
            return encryptedToken;
        }*/

        private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
                issuer: _jwt.Value.Issuer,           // Set the issuer
                audience: _jwt.Value.Audience,       // Set the audience
                claims: claims,
                expires: DateTime.UtcNow.AddDays(2),
                signingCredentials: signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var encryptedToken = tokenHandler.WriteToken(token);
            return encryptedToken;
        }


        public async Task<bool> ChangePassword(ChangePasswordRequestModel request, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null) throw new Exception("User Not Found");
            var newpassword = await _userManager.ChangePasswordAsync(user, request.Password, request.NewPassword);
            return newpassword.Succeeded ? true : false; 
        }

        public async Task<BaseResponse<string>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                user.IsActive = true;
                await _userManager.UpdateAsync(user);
                return new BaseResponse<string> { IsSuccessful = true, Message = "you can proceed to login" };
            }
            else
            {
                throw new Exception($"An error ocurred when confirming {user.Email} email");
            }
        }

        public async Task<BaseResponse<string>> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                throw new Exception("An error has occured");
            }
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "account/reset-password";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            var passwordResetURL = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", code);
            var mailRequest = new EmailRequestModel
            {
                HtmlContent = string.Format(_localizer["Please reset your password by <a href='{0}'>clicking here</a>."], HtmlEncoder.Default.Encode(passwordResetURL)),
                Subject = _localizer["Reset Password"],
                ToEmail = request.Email,
                ToName = user.FullName
            };
            BackgroundJob.Enqueue(() => _emailService.SendEmail(mailRequest));
            return new BaseResponse<string> { IsSuccessful = true, Message = (_localizer["Password Reset Mail has been sent to your authorized Email."]) };
        }

        public async Task<BaseResponse<string>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new BaseResponse<string> { IsSuccessful = false, Message = (_localizer["An Error has occured!"]) } ;
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (result.Succeeded)
            {
                return new BaseResponse<string> { IsSuccessful = true, Message = (_localizer["Password Reset Successful!"])
            };
                
            }
            else
            {
                return new BaseResponse<string> { IsSuccessful = false, Message = (_localizer["An Error has occured!"]) };
            }
        }


    }
}
