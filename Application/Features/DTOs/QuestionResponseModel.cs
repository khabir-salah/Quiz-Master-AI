using Domain.Entities;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace Application.Features.DTOs
{
    public record class QuestionResponseModel
    {
        public string QuestionText { get; set; }
        public ICollection<OptionResponseModel> Options { get; set; }
        public string Answer { get; set; }
        public string Explanation { get; set; }
        public string UserAnswer { get; set; }
    }

    public record class AssessmentResponseModel
    {
        public ICollection<QuestionResponseModel> Question { get; set; }
        public QuestionType QuestionType { get; set; }
        public DateTime Date { get; set; }
    }

    public record class OptionResponseModel
    {
        public string Text { get; set; }
    }
}
