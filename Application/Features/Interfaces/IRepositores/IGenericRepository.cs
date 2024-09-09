using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Interfaces.IRepositores
{
    public interface IGenericRepository<T>
    {
        Task AddAsync(T item);
        void Update(T item);
        void Delete(T item);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);

    }
}
