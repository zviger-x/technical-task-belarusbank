using MediatR;
using Shared.Common;
using Shared.Common.Results;
using Users.Application.Contracts;

namespace Users.Application.UseCases.Commands
{
    public record UserCreateCommand(CreateUserDto User, UserContext UserContext) : IRequest<Result<Guid>>;
}
