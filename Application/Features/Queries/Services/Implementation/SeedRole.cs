using Application.Features.Interfaces.IService;
using Domain.Entities;
using Domain.RoleConst;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Queries.Services.Implementation
{
    public  class SeedRole(UserManager<ApplicationUser> _userManager) : ISeedRole
    { 
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
            if (!await _userManager.IsInRoleAsync(user, RoleConst.Basic))
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
            if (!await _userManager.IsInRoleAsync(user, RoleConst.Standard))
            {
                await _userManager.AddToRoleAsync(user, RoleConst.Standard);
            }
        }
    }
}
