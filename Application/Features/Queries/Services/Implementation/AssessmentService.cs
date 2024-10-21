using Application.Features.Interfaces.IRepositores;
using Application.Features.Interfaces.IService;
using Domain.Entities;

namespace Application.Features.Queries.Services.Implementation
{
    public class AssessmentService(IGenericRepository<Assesment> _assessment) : IAssessmentService
    {
        public async  Task<int> GetAssessmentGeneratedToday(string userId, DateTime date)
        {
            var record = await _assessment.GetAsync(q => q.ApplicationUserId == userId && q.Date == date);

            return record?.AssessmentCount ?? 0;
        }
         
        public async Task StoreQuizGeneration(string userId, DateTime date)
        {
            var record = await _assessment.GetAsync(q => q.ApplicationUserId == userId && q.Date == date);

            record.AssessmentCount += 1;
            await _assessment.SaveAsync();
        }
    }
}
