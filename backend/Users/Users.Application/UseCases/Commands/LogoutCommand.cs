using MediatR;
using Shared.Common;
using Shared.Common.Results;

namespace Users.Application.UseCases.Commands
{
    public record LogoutCommand(UserContext UserContext) : IRequest<Result>;
}
