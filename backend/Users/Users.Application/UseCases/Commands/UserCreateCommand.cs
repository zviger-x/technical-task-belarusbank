using MediatR;
using Shared.Common.Results;
using Users.Application.Contracts;

namespace Users.Application.UseCases.Commands
{
    public class UserCreateCommand : IRequest<Result<Guid>>
    {
        public required CreateUserDto User { get; set; }
    }
}
