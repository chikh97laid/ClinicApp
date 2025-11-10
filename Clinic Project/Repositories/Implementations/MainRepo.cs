using Clinic_Project.Data;
using Clinic_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Clinic_Project.Repositories.Implementations
{
    public class MainRepo<T> : IRepo<T> where T : class
    {
        private readonly AppDbContext _context;
        public MainRepo(AppDbContext context)
        {
            this._context = context;
        }

        public virtual async Task<IEnumerable<T>?> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public virtual async Task<T?> GetOneAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(predicate);
        }

        // this method is for bringing list of data such as all patient with age > 30 
        public async Task<IEnumerable<T>?> FindAsync(Expression<Func<T, bool>> predicate) 
        {
            return await _context.Set<T>().AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<bool> IsExistAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().AnyAsync(match);
        }
    }
}
