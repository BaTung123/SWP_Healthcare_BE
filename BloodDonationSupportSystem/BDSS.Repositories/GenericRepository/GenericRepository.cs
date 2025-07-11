using BDSS.Common.Utils;
using BDSS.Models.Context;
using BDSS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BDSS.Repositories.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : GenericModel
    {
        protected readonly BdssDbContext _context;
        public DbSet<T> Entities => _context.Set<T>();

        public GenericRepository(BdssDbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            T newEntity = _context.Add(entity).Entity;
            await _context.SaveChangesAsync();

            return newEntity;
        }

        public async Task UpdateAsync(T entity)
        {
            entity.UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7();
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await Entities.FindAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                entity.UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7();
                Entities.Update(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await Entities
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        public IQueryable<T> GetAll()
            => Entities.Where(x => !x.IsDeleted).AsQueryable();

        public T Find(params object[] keyValues)
        {
            return Entities.Find(keyValues);
        }

        public async Task<T> FindAsync(params object[] keyValues)
        {
            return await Entities.FindAsync(keyValues);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<T?> FirstOrDefaultAsync(System.Linq.Expressions.Expression<System.Func<T, bool>> predicate)
        {
            return await Entities.FirstOrDefaultAsync(predicate);
        }
    }
}