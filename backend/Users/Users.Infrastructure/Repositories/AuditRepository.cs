using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Users.Application.Repositories;
using Users.Domain;
using Users.Infrastructure.Contexts;

namespace Users.Infrastructure.Repositories
{
    public class AuditRepository : BaseRepository<AuditLog>, IAuditRepository
    {
        public AuditRepository(UserDbContext context) : base(context)
        {
        }

        public async Task<PagedCollection<AuditLog>> GetPagedByUserAsync(
            Guid? userId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _context.AuditLogs.AsNoTracking();

            if (userId.HasValue)
                query = query.Where(x => x.UserId == userId.Value);

            var totalCount = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(totalCount / (float)pageSize);

            var page = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedCollection<AuditLog>
            {
                Items = page,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
