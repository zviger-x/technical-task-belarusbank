using MediatR;
using Shared.Common.Results;

namespace Users.Application.UseCases.Queries
{
    public record UserGetBlockStatusQuery(Guid Guid) : IRequest<Result<bool>>;
}
