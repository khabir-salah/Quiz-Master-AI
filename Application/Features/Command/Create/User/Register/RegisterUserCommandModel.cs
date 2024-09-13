using Application.Features.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.Create.User.Register
{
    public record class  RegisterUserCommandModel(string UserName,  string Password, string Email, string FullName) : IRequest<BaseResponse<string>>
    {
    }
}
