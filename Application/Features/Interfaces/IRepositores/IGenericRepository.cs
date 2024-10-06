
using System.Linq.Expressions;

namespace Application.Features.Interfaces.IRepositores
{
    public interface IGenericRepository<T>
    {
        Task AddAsync(T item);
        void Update(T item);
        void Delete(T item);
        Task SaveAsync();
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        Task<bool> isExist(Expression<Func<T, bool>> predicate);
    }
}
