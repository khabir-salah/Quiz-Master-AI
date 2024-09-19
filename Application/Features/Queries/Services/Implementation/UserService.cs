using Application.Features.DTOs;
using Application.Features.Interfaces.IRepositores;
using Application.Features.Interfaces.IService;
using Domain.Entities;
using Domain.RoleConst;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Queries.Services.Implementation
{
    public class UserService(UserManager<ApplicationUser> _userManager, IHttpContextAccessor _assessor, IUserRepository _user ) : IUserService
    {
        public async Task<ApplicationUser> GetCurrentUser()
        {
            var userId = _assessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId == null ? null : await _userManager.FindByIdAsync(userId);
        }

        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(RoleConst.Basic))
            {
                await roleManager.CreateAsync(new IdentityRole(RoleConst.Basic));
            }
            if (!await roleManager.RoleExistsAsync(RoleConst.Standard))
            {
                await roleManager.CreateAsync(new IdentityRole(RoleConst.Standard));
            }
            if (!await roleManager.RoleExistsAsync(RoleConst.Classic))
            {
                await roleManager.CreateAsync(new IdentityRole(RoleConst.Classic));
            }
        }

        public async Task AssignBasicRole(ApplicationUser user)
        {
            if(!await _userManager.IsInRoleAsync(user, RoleConst.Basic))
            {
                await _userManager.AddToRoleAsync(user, RoleConst.Basic);
            }
        }

        public async Task AssignClassicRole(ApplicationUser user)
        {
            if (!await _userManager.IsInRoleAsync(user, RoleConst.Classic))
            {
                await _userManager.AddToRoleAsync(user, RoleConst.Classic);
            }
        }

        public async Task AssignStandardRole(ApplicationUser user)
        {
            if(!await _userManager.IsInRoleAsync(user, RoleConst.Standard))
            {
                await _userManager.AddToRoleAsync(user,RoleConst.Standard);
            }
        }

        public async Task<BaseResponse<LoginResponseModel>> LoginAsync(LoginRequestModel request)
        {
            var user = await _user.GetAsync(u => u.Email == request.Email) ?? throw new ArgumentNullException("Email doesnt exist");
            if(BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return new BaseResponse<LoginResponseModel>
                {
                    IsSuccessful = true,
                    Result = new LoginResponseModel
                    {
                        Email = request.Email,
                        FullName = user.FullName,
                    },
                    Message = "Login Successfull"
                };
            }
            return new BaseResponse<LoginResponseModel>
            {
                IsSuccessful = false,
                Result = null,
                Message = "Login Failed"
            };
        }
    }
}
