using Shared.Abstractions.Repositories;
using Shared.Common;
using Users.Domain;

namespace Users.Application.Repositories
{
    public interface IAuditRepository : IRepository<AuditLog>
    {
        /// <summary>
        /// Returns a paged collection of entities, optionally filtered by user ID.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation if needed.</param>
        /// <returns>A paged collection of entities.</returns>
        Task<PagedCollection<AuditLog>> GetPagedByUserAsync(
            Guid? userId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
    }
}
