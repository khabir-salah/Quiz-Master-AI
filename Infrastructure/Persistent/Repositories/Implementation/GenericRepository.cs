using Application.Features.Interfaces.IRepositores;
using Infrastructure.Persistent.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Infrastructure.Persistent.Repositories.Implementation
{
    public class GenericRepository<T>(QuizMasterAiDb _context, DbSet<T> _dbSet) : IGenericRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;
        public async Task AddAsync(T item)
        {
            await _context.AddAsync(item);
        }

        public void Delete(T item)
        {
            _context.Remove(item);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public void Update(T item)
        {
            _context.Update(item);
        }
    }
}
