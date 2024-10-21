using Application.Features.DTOs;
using Domain.Entities;
using MediatR;


namespace Application.Features.Queries.AssessmentResult
{
    public record class GetAssesmentResultCommandModel : IRequest<BaseResponse<AssessmentResultResponseModel>>
    {
        public Dictionary<Guid, string> Answer { get; set; } 
        public Guid AssesmentId { get; set; }
    }

    public record class AssessmentResultResponseModel
    {
        public double Score { get; set; }
        public ICollection<QuestionResponseModel> Questions { get; set; }
        public Guid AssesmentId { get; set; }
    }
}
