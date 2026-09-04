using Shared.Common.Errors;

namespace Users.Application.Common.Errors
{
    internal static class UserErrors
    {
        public static readonly Error EmailAlreadyExists =
            new ConflictError(
                "User.EmailAlreadyExists",
                "A user with this email already exists.");

        public static readonly Error UserToDeleteNotFound =
            new NotFoundError(
                "User.Delete.NotFound",
                "User is already deleted or not found.");

        public static readonly Error UserNotFound =
            new NotFoundError(
                "User.NotFound",
                "User not found.");

        public static readonly Error InvalidCurrentPassword =
            new ValidationError(
                "User.InvalidCurrentPassword",
                "The current password is incorrect.");
    }
}
