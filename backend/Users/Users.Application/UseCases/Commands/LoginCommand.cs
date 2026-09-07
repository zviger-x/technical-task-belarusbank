using MediatR;
using Shared.Common.Results;
using Users.Application.Contracts;

namespace Users.Application.UseCases.Commands
{
    public record LoginCommand(LoginDto LoginDto) : IRequest<Result<TokenResponseDto>>;
}
