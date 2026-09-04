using MediatR;
using Shared.Common.Results;

namespace Users.Application.UseCases.Commands
{
    public record LogoutCommand(Guid UserId) : IRequest<Result>;
}
