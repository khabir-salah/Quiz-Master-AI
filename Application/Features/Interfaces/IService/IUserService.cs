using Application.Features.DTOs;
using Domain.Entities;


namespace Application.Features.Interfaces.IService
{
    public interface IUserService
    {
        Task<ApplicationUser> GetCurrentUser();
        Task AssignStandardRole(ApplicationUser user);
        Task AssignClassicRole(ApplicationUser user);
        Task AssignBasicRole(ApplicationUser user);
        Task<BaseResponse<LoginResponseModel>> LoginAsync(LoginRequestModel request);
    }
}
