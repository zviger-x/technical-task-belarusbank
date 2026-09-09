using MediatR;
using Shared.Common;
using Shared.Common.Results;
using Users.Domain;

namespace Users.Application.UseCases.Queries
{
    public record AuditGetPagedQuery(Guid? UserId, PageParameters PageParameters) : IRequest<Result<PagedCollection<AuditLog>>>;
}
