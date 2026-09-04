using MediatR;
using Shared.Common.Results;
using Shared.Enums;

namespace Users.Application.UseCases.Commands
{
    public record UserChangeRoleCommand(Guid UserId, UserRoles UserRole) : IRequest<Result>;
}
