using Shared.Enums;

namespace Shared.Common
{
    public sealed record UserContext(Guid Id, UserRoles Role)
    {
        public bool IsAdmin => Role == UserRoles.Admin;
    }
}
