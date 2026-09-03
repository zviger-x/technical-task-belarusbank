using Shared.Common.Errors;

namespace Users.Application.Common.Errors
{
    internal static class UserErrors
    {
        public static readonly Error EmailAlreadyExists =
            new ConflictError(
                "User.EmailAlreadyExists",
                "A user with this email already exists.");
    }
}
