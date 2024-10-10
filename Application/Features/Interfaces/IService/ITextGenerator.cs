using Application.Features.DTOs;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Interfaces.IService
{
    public interface ITextGenerator
    {
        Task<BaseResponse<string>> GenerateTextAssessmnet(TextGeneratorRequestModel request );
       
    }
}
