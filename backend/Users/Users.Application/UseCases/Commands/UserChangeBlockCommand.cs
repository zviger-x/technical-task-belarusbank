using MediatR;
using Shared.Common.Results;

namespace Users.Application.UseCases.Commands
{
    public record UserChangeBlockCommand(Guid UserId, bool IsBlocked) : IRequest<Result>;
}
