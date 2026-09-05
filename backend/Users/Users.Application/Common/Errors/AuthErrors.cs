using Shared.Common.Errors;

namespace Users.Application.Common.Errors
{
    internal static class AuthErrors
    {
        public static readonly Error InvalidEmailOrPassword =
            new ValidationError(
                "Auth.InvalidEmailOrPassword",
                "Invalid email or password.");

        public static readonly Error InvalidRefreshToken =
            new ValidationError(
                "Auth.InvalidRefreshToken",
                "Refresh token expired or invalid.");
    }
}
