using Domain.Entities;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.Create.Assesment
{
    public record class CreateAssessmentCommandModel
    {
        public QuestionType QuestionType { get; set; }
        public int NumberOfQuestion { get; set; }
        public Guid UserId { get; set; }
        public Document Document { get; set; }
    }

    public record class CreateAssessmentResponseModel
    {
        public ICollection<Question> Question { get; set; }
        public QuestionType QuestionType { get; set; }
    }
}
