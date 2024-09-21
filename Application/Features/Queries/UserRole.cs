using Domain.Enum;
using Domain.RoleConst;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Queries
{
    public class UserRole
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if(!await roleManager.RoleExistsAsync(RoleConst.Basic))
            {
                await roleManager.CreateAsync( new IdentityRole(RoleConst.Basic));
            }
            if(! await roleManager.RoleExistsAsync(RoleConst.Standard))
            {
                await roleManager.CreateAsync(new IdentityRole(RoleConst.Standard));
            }
            if (!await roleManager.RoleExistsAsync(RoleConst.Classic))
            {
                await roleManager.CreateAsync(new IdentityRole(RoleConst.Classic));
            }
        }
    }
}
