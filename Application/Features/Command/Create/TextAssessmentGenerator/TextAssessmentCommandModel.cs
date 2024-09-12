using Application.Features.DTOs;
using Domain.Entities;
using Domain.Enum;
using MediatR;


namespace Application.Features.Command.Create.Assesment
{
    public record class TextAssessmentCommandModel() : IRequest<BaseResponse<AssessmentResponseModel>>
    {
        public string Title { get; set; }
        public UploadType DocumentType { get; set; }
        public int NumberOfQuestion { get; set; }
        public string Content { get; set; }
        public QuestionType Type { get; set; }
    }
}
