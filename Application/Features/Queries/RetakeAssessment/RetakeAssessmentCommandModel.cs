﻿using Application.Features.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Queries.RetakeAssessment
{
    public record class RetakeAssessmentCommandModel : IRequest<BaseResponse<ICollection<RetakeAssesmentResponseModel>>>
    {
        public Guid AssessmentId { get; set; }
    }

    public record class RetakeAssesmentResponseModel
    {
        public string QuestionText { get; set; }
        public ICollection<OptionResponseModel> Options { get; set; }
    }
}
