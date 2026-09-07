using MediatR;
using Shared.Common;
using Shared.Common.Results;
using Users.Application.Contracts;

namespace Users.Application.UseCases.Queries
{
    public record UserGetPagedQuery(PageParameters PageParameters) : IRequest<Result<PagedCollection<UserDto>>>;
}
