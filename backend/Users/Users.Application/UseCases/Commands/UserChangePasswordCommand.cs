using MediatR;
using Shared.Common;
using Shared.Common.Results;
using Users.Application.Contracts;

namespace Users.Application.UseCases.Commands
{
    public record UserChangePasswordCommand(
        Guid UserId,
        ChangeUserPasswordDto UserPasswordDto,
        UserContext UserContext
        ) : IRequest<Result>;
}
