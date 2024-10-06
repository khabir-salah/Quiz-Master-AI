using Domain.Entities;
using Domain.Enum;
using MediatR;

namespace Application.Features.DTOs
{
    public class  TextAssessmentModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string AssessmentLink { get; set; } = default!;
        public DateTime DateCreated { get; set; }
    }

    


}
