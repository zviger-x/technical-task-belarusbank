using MediatR;
using Shared.Common;
using Shared.Common.Results;
using Shared.Enums;

namespace Users.Application.UseCases.Commands
{
    public record UserChangeRoleCommand(Guid UserId, UserRoles UserRole, UserContext UserContext) : IRequest<Result>;
}
