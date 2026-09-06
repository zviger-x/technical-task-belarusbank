using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Repositories;
using Shared.Common;
using Shared.Entities;
using Users.Infrastructure.Contexts;

namespace Users.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T>
        where T : class, IEntity
    {
        protected UserDbContext _context { get; set; }

        protected BaseRepository(UserDbContext context)
        {
            _context = context;
        }

        public virtual async Task<Guid> CreateAsync(T entity, CancellationToken token = default)
        {
            await _context.AddAsync(entity, token);

            DetachEntity(entity);

            return entity.Id;
        }

        public virtual Task UpdateAsync(T entity, CancellationToken token = default)
        {
            _context.Update(entity);

            DetachEntity(entity);

            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(T entity, CancellationToken token = default)
        {
            _context.Set<T>().Remove(entity);

            return Task.CompletedTask;
        }

        public virtual Task<T> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, token);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken token = default)
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync(token);
        }

        public virtual async Task<PagedCollection<T>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken token = default)
        {
            var query = _context.Set<T>().AsNoTracking();

            var totalCount = await query.CountAsync(token);
            var totalPages = (int)Math.Ceiling(totalCount / (float)pageSize);

            var page = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(token);

            return new PagedCollection<T>
            {
                Items = page,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        public virtual Task CreateManyAsync(IEnumerable<T> entities, CancellationToken token = default)
        {
            return _context.BulkInsertAsync(entities, cancellationToken: token);
        }

        public virtual Task UpdateManyAsync(IEnumerable<T> entities, CancellationToken token = default)
        {
            return _context.BulkUpdateAsync(entities, cancellationToken: token);
        }

        public virtual Task DeleteManyAsync(IEnumerable<T> entities, CancellationToken token = default)
        {
            return _context.BulkDeleteAsync(entities, cancellationToken: token);
        }

        public virtual void Dispose()
        {
            _context.Dispose();
        }

        protected virtual void DetachEntity(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }
    }
}
