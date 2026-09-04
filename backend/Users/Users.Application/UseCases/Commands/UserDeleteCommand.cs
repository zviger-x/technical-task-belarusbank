using MediatR;
using Shared.Common.Results;

namespace Users.Application.UseCases.Commands
{
    public record UserDeleteCommand(Guid UserId) : IRequest<Result>;
}
