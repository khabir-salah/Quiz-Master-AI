using Application.Features.Command.Create.User.Register;
using Application.Features.DTOs;
using Application.Features.Interfaces.IService;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AuthenticationController(IMediator _mediator, IUserService _userService, SignInManager<ApplicationUser> _signInManager, IHttpContextAccessor _assessor) : ControllerBase
    { 
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserCommandModel request)
        {
            if(ModelState.IsValid)
            {
                var registerResponse = await _mediator.Send(request);

                if (!registerResponse.IsSuccessful) return BadRequest(registerResponse.Message);
                return Ok(registerResponse.Message);
            }
            return BadRequest();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestModel request)
        {
            var user = await _userService.LoginAsync(request);
            if(!user.IsSuccessful) return BadRequest(user.Message);
            return Ok(user.Result);
        }

        [HttpGet("LogOut")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost("Forgot-Password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _userService.ForgotPasswordAsync(request, origin));
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            return Ok(await _userService.ResetPasswordAsync(request));
        }

        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        {
            var result = await _userService.ConfirmEmailAsync(userId, code);

            if (result.IsSuccessful)
            {
                var origin = Request.Headers["Origin"].ToString();
                var redirectUrl = string.IsNullOrEmpty(origin) ? "https://localhost:7164/" : origin;
                return Redirect($"{redirectUrl}"); 
            }
            return BadRequest("Email confirmation failed.");
        }

        [Authorize]
        [HttpGet("currentuserinfo")]
        public async Task<CurrentUser> CurrentUserInfo()
        {
            var user = await _userService.GetCurrentUser();
            var claims = await _userService.GetClaimsAsync(user);

            if (user == null)
            {
                return new CurrentUser
                {
                    IsAuthenticated = false,
                    Claims = new Dictionary<string, string>()
                };
            }

            return new CurrentUser
            {
                IsAuthenticated = user.IsActive, 
                UserName = user.UserName,
                Claims = claims.ToDictionary(c => c.Type, c => c.Value)
            };
        }
    }
}
