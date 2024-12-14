using Application.Features.DTOs;
using Application.Features.Interfaces.IRepositores;
using Application.Features.Interfaces.IService;
using Domain.Entities;
using Domain.RoleConst;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel;
using System.Text;


namespace Application.Features.Command.Create.User.Register
{
    public class RegisterUserHandler(IGenericRepository<ApplicationUser> _userRepo, UserManager<ApplicationUser> _userManager, IUserService _userService, IEmailService _emailService, ISeedRole _role, IBackgroundJobClient _backgroundJobClient) : IRequestHandler<RegisterUserCommandModel, BaseResponse<string>>
    {
        public async Task<BaseResponse<string>> Handle(RegisterUserCommandModel request, CancellationToken cancellationToken)
        {
           var isUserExist = await _userManager.FindByEmailAsync(request.Email);

           if(isUserExist != null)
           {
                return new BaseResponse<string>
                {
                    IsSuccessful = false,
                    Message = "User Already Exist"
                };
            }

            var origin = Url.Uri;

            var newUser = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.FirstName
            }; 
            
            var result = await _userManager.CreateAsync(newUser, request.Password);
            if(result.Succeeded)
            {
                await _role.AssignBasicRole(newUser);
                var verificationUri = await SendEmailVerifaction(newUser, origin);
                _backgroundJobClient.Enqueue(() => _emailService.SendWelcomeMessage(request.Email, newUser.FullName, verificationUri));
                return new BaseResponse<string> { IsSuccessful = true, Message = "User Registered please check your mailbox to verify" };
                
            }
            return result.Succeeded == true ? new BaseResponse<string> { IsSuccessful = true } : new BaseResponse<string> { IsSuccessful = false, Message = string.Join(",   ", result.Errors.Select(e => e.Description)) };
        }


        private async Task<string> SendEmailVerifaction(ApplicationUser user,  string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/Account/confirm-email/";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            return verificationUri;
        }
    }
}
