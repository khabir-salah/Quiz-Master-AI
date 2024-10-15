using Application.Features.DTOs;
using Application.Features.Interfaces.IRepositores;
using Application.Features.Interfaces.IService;
using Domain.Entities;
using Domain.RoleConst;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Application.Features.Queries.Services.Implementation
{
    public  class SeedRole(UserManager<ApplicationUser> _userManager, HttpClient _httpClient, IConfiguration _configuration) : ISeedRole
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

        public async Task<ApplicationUser?> GetUserByEmail(string Email)
        {
            return await _userManager.FindByEmailAsync(Email);
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
                await _userManager.RemoveFromRoleAsync(user, RoleConst.Basic);
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

        public async Task<PaystackVerifyResponse?> VerifyPaystackPayment(string reference)
        {
            string? apiKey = _configuration["PayStack:SecretKey"];
            var paystackUrl = $"https://api.paystack.co/transaction/verify/{reference}";
            var request = new HttpRequestMessage(HttpMethod.Get, paystackUrl);
            request.Headers.Add("Authorization", $"Bearer {apiKey}");

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var paystackResponse = await response.Content.ReadFromJsonAsync<PaystackVerifyResponse>();
                return paystackResponse;
            }
            return null;
        }

       
    }
}
