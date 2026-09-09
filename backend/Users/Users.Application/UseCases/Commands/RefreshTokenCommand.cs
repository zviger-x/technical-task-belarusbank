using MediatR;
using Shared.Common.Results;
using Users.Application.Contracts;

namespace Users.Application.UseCases.Commands
{
    public record RefreshTokenCommand(string RefreshToken) : IRequest<Result<TokenResponseDto>>;
}
