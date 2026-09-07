using MediatR;
using Shared.Common;
using Shared.Common.Results;

namespace Users.Application.UseCases.Commands
{
    public record UserDeleteCommand(Guid UserId, UserContext UserContext) : IRequest<Result>;
}
