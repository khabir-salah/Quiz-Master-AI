using Application.Features.Interfaces.IRepositores;
using Infrastructure.Persistent.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Infrastructure.Persistent.Repositories.Implementation
{
    public class GenericRepository<T>(QuizMasterAiDb _context) : IGenericRepository<T> where T : class
    {
        public async Task AddAsync(T item)
        {
            await _context.AddAsync(item);
        }

        public async Task AddRangeAsync(IEnumerable<T> documents)
        {
            await _context.AddRangeAsync(documents);
        }

        public void Delete(T item)
        {
            _context.Remove(item);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public void Update(T item)
        {
            _context.Update(item);
        }
        public async Task<bool> isExist(Expression<Func<T, bool>> predicate)
        {
            var exist =  _context.Set<T>().Any(predicate);
            return exist == true ? true : false;
        }

        public async Task SaveAsync()
        {
             await _context.SaveChangesAsync();
        }
        //public async Task<int> SaveAsync()
        //{
        //    var save = await _context.SaveChangesAsync();
        //    return save;
        //}
    }
}
