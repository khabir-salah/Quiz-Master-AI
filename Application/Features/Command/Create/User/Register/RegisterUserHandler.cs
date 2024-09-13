using Application.Features.DTOs;
using Application.Features.Interfaces.IRepositores;
using Application.Features.Interfaces.IService;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Application.Features.Command.Create.User.Register
{
    public class RegisterUserHandler(IGenericRepository<ApplicationUser> _userRepo, UserManager<ApplicationUser> _userManager, IUserService _userService) : IRequestHandler<RegisterUserCommandModel, BaseResponse<string>>
    {
        public async Task<BaseResponse<string>> Handle(RegisterUserCommandModel request, CancellationToken cancellationToken)
        {
           var isUserExist = await _userRepo.isExist(U => U.Email == request.Email);

           if(!isUserExist)
           {
                return new BaseResponse<string>
                {
                    IsSuccessful = false,
                    Result = "User Already Exist"
                };
           } 

            var newUser = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.UserName,
                FullName = request.FullName,
            };
            
            var result = await _userManager.CreateAsync(newUser, request.Password);

            await _userService.AssignBasicRole(newUser);

            return result.Succeeded == true ? new BaseResponse<string> { IsSuccessful = true } : new BaseResponse<string> { IsSuccessful = false };
        }
    }
}
