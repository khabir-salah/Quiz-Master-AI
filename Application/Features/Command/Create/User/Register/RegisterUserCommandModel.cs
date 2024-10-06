using Application.Features.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.Create.User.Register
{
    public record class  RegisterUserCommandModel(string Password, string Email, string LastName, string FirstName) : IRequest<BaseResponse<string>>
    {
        //public RegistrationRequest RegisterRequest {  get; set; }
        //public string origin { get; set; }
    }

    //public record class RegistrationRequest(string Password, string Email, string LastName, string FirstName, bool AutoActivateEmail, bool ActivateUser);
}
