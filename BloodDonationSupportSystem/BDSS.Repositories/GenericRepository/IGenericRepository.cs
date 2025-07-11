using BDSS.Models.Entities;

namespace BDSS.Repositories.GenericRepository
{
    public interface IGenericRepository<T> where T : GenericModel
    {
        public Task<T> AddAsync(T entity);
        public Task UpdateAsync(T entity);
        public Task DeleteAsync(long id);
        public Task<IList<T>> GetAllAsync();
        public IQueryable<T> GetAll();
        public T Find(params object[] keyValues);
        public Task<T> FindAsync(params object[] keyValues);
        public Task AddRangeAsync(IEnumerable<T> entities);
        Task<T?> FirstOrDefaultAsync(System.Linq.Expressions.Expression<System.Func<T, bool>> predicate);
    }
}