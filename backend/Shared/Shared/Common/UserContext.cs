using Shared.Enums;

namespace Shared.Common
{
    public sealed record UserContext(Guid Id, UserRoles Role);
}
