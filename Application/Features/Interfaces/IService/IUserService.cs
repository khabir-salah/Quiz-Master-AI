using Application.Features.DTOs;
using Domain.Entities;
using System.Security.Claims;


namespace Application.Features.Interfaces.IService
{
    public interface IUserService
    {
        Task<ApplicationUser> GetCurrentUser();
        
        Task<BaseResponse<TokenResponse>> LoginAsync(LoginRequestModel model);
        Task<bool> ChangePassword(ChangePasswordRequestModel request, string userId);
        Task<BaseResponse<string>> ConfirmEmailAsync(string userId, string code);
        Task<BaseResponse<string>> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
        Task<BaseResponse<string>> ResetPasswordAsync(ResetPasswordRequest request);
        Task<IEnumerable<Claim>> GetClaimsAsync(ApplicationUser user);
    }
}
