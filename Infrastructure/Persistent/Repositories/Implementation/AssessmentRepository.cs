using Application.Features.Interfaces.IRepositores;
using Domain.Entities;
using Infrastructure.Persistent.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistent.Repositories.Implementation
{
    public class AssessmentRepository : GenericRepository<Assesment>, IAssessmentRepository
    {
        private readonly QuizMasterAiDb _context;
        public AssessmentRepository(QuizMasterAiDb context) : base(context)
        {
            _context = context;
        }
        public async Task<Assesment?> GetAssesmentAsync(Expression<Func<Assesment, bool>> expression)
        {
            var assessment = _context.Assesment.Include(u => u.Question).FirstOrDefaultAsync();
            return await assessment;
        }
    }
}
