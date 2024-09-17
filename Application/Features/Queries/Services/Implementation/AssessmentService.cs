using Application.Features.Interfaces.IRepositores;
using Application.Features.Interfaces.IService;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Queries.Services.Implementation
{
    public class AssessmentService(IGenericRepository<Assesment> _assessment) : IAssessmentService
    {
        public async  Task<int> GetAssessmentGeneratedToday(string userId, DateTime date)
        {
            var today = date.Date;
            var record = await _assessment.GetAsync(q => q.ApplicationUserId == userId && q.Date == today);

            return record?.AssessmentCount ?? 0;
        }

        public async Task StoreQuizGeneration(string userId, DateTime date)
        {
            var today = date.Date;
            var record = await _assessment.GetAsync(q => q.ApplicationUserId == userId && q.Date == today);
            record.AssessmentCount += 1;
        }
    }
}
