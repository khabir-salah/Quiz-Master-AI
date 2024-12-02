using Application.Features.DTOs;
using Domain.Entities;
using Domain.Enum;
using MediatR;


namespace Application.Features.Command.Create.Assesment
{
    public record class TextAssessmentCommandModel() : IRequest<BaseResponse<AssessmentResponse>>
    {
        public string Title { get; set; }
        //public int NumberOfQuestion { get; set; }
        public string Content { get; set; }
        //public QuestionType Type { get; set; }
        public Choice Choice { get; set; }
    }

    public class Choice
    {
        public int NumberOfQuestion { get; set; }
        public QuestionType Type { get; set; }
    }

    public record class AssessmentResponse
    {
        public Guid AssessmentId { get; set; }
        public ICollection<QuestionModel> Question { get; set; }
        public QuestionType QuestionType { get; set; }
        public DateTime Date { get; set; }
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
