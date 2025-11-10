using System.Linq.Expressions;

namespace Clinic_Project.Repositories.Interfaces
{
    public interface IRepo<T> where T : class
    {
        Task<IEnumerable<T>?> GetAllAsync();
        Task<T?> GetOneAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>?> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> IsExistAsync(Expression<Func<T, bool>> match);
        //Task<bool> IsExistAsync(int id);
    }
}
