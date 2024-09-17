using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Interfaces.IService
{
    public interface IAssessmentService
    {
        Task StoreQuizGeneration(string userId, DateTime date);
        Task<int> GetAssessmentGeneratedToday(string userId, DateTime date);
    }
}
