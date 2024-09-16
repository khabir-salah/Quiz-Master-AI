using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Interfaces.IRepositores
{
    public interface IAssessmentRepository : IGenericRepository<Assesment>
    {
        Task<Assesment?> GetAssesmentAsync(Expression<Func<Assesment, bool>> expression);
    }
}
