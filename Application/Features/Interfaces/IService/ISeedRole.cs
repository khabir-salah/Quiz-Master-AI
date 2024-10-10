using Application.Features.DTOs;
using Domain.Entities;

namespace Application.Features.Interfaces.IService
{
    public interface ISeedRole
    {
        Task AssignStandardRole(ApplicationUser user);
        Task AssignClassicRole(ApplicationUser user);
        Task AssignBasicRole(ApplicationUser user);
        Task<ApplicationUser?> GetUserByEmail(string Email);
        Task<PaystackVerifyResponse?> VerifyPaystackPayment(string reference);
    }
}
