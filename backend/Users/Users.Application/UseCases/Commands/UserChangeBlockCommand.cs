using MediatR;
using Shared.Common;
using Shared.Common.Results;

namespace Users.Application.UseCases.Commands
{
    public record UserChangeBlockCommand(Guid UserId, bool IsBlocked, UserContext UserContext) : IRequest<Result>;
}
