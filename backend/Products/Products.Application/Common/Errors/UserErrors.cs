using Shared.Common.Errors;

namespace Products.Application.Common.Errors
{
    internal static class UserErrors
    {
        public static readonly Error UserBlocked =
            new ForbiddenError(
                "User.Blocked",
                "User is blocked.");
    }
}
