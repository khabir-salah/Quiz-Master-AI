using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Interfaces.IService
{
    public interface IExtractQuestionService
    {
        Task<ICollection<Question>> ExtractMultipleChoiceQuestions(string questionText, Guid assesmentId);
        Task<ICollection<Question>> ExtractTrueFalseQuestions(string rawResponse, Guid assesmentId);
    }
}
