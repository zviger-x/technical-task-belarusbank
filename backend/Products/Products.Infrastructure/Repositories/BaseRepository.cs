using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Products.Infrastructure.Contexts;
using Shared.Abstractions.Repositories;
using Shared.Common;
using Shared.Entities;

namespace Products.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T>
        where T : class, IEntity
    {
        protected ProductsDbContext _context { get; set; }

        protected BaseRepository(ProductsDbContext context)
        {
            _context = context;
        }

        public virtual async Task<Guid> CreateAsync(T entity, CancellationToken token = default)
        {
            await _context.AddAsync(entity, token);

            return entity.Id;
        }

        public virtual Task UpdateAsync(T entity, CancellationToken token = default)
        {
            _context.Update(entity);

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
    }
}
