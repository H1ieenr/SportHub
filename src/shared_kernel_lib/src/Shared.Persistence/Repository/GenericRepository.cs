using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Shared.Persistence
{
    public class GenericRepository<T, TContext> : IGenericRepository<T>
        where T : class
        where TContext : DbContext
    {
        protected readonly TContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(TContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
                                 => await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default) => await _dbSet.AsNoTracking().ToListAsync(cancellationToken);

        public virtual async Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "",
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _dbSet;

            // 1. Filter logic
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // 2. Eager loading logic (Include các bảng liên quan)
            var properties = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var includeProperty in properties)
            {
                query = query.Include(includeProperty.Trim());
            }

            // 3. OrderBy logic
            if (orderBy != null)
            {
                return await orderBy(query).AsNoTracking().ToListAsync();
            }

            return await query.AsNoTracking().ToListAsync(cancellationToken);
        }

        public virtual async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public virtual async Task AddRangeAsync(IEnumerable<T> entities) => await _dbSet.AddRangeAsync(entities);

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
            // Hoặc: _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void UpdateRange(IEnumerable<T> entities) => _dbSet.UpdateRange(entities);

        public virtual void Delete(T entity) => _dbSet.Remove(entity);

        public virtual void DeleteRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);

        public virtual async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}