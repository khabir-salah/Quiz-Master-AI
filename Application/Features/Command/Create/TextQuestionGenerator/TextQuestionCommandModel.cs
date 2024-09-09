using Application.Features.DTOs;
using Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.Features.Command.Create.Assesment
{
    public record class TextQuestionCommandModel() : IRequest<BaseResponse<string>>
    {
        public string Title { get; set; }
        public UploadType DocumentType { get; set; }
        public int NumberOfQuestion { get; set; }
        public string Content { get; set; }
        public QuestionType Type { get; set; }
    }
}
