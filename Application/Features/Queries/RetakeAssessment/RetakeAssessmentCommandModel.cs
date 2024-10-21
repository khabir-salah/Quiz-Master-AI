using Application.Features.DTOs;
using Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Queries.RetakeAssessment
{
    public record class RetakeAssessmentCommandModel : IRequest<BaseResponse<RetakeAssesmentResponseModel>>
    {
        public Guid AssessmentId { get; set; }
    }

    public record class RetakeAssesmentResponseModel
    {
        public Guid AssessmentId { get; set; }

        public ICollection<QuestionModel> Question { get; set; }

    }

    
    public record class QuestionModel
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; }
        public ICollection<OptionModel> Options { get; set; }
    }

    public record class OptionModel
    {
        public Guid OptionId { get; set; }
        public string Text { get; set; }
    }
}
