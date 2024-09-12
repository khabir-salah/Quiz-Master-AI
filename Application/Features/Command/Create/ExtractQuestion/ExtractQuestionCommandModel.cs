using Application.Features.DTOs;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.Create.ExtractQuestion
{
    public record class ExtractQuestionCommandModel : IRequest<BaseResponse<string>>
    {
    }

    
}
