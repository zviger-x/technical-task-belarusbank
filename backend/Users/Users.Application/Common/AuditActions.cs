namespace Users.Application.Common
{
    internal static class AuditActions
    {
        public const string UserLoggedIn = "User.LoggedIn";
        public const string UserLoggedOut = "User.LoggedOut";
        public const string UserTokenRefreshed = "User.TokenRefreshed";
        public const string UserCreated = "User.Created";
        public const string UserDeleted = "User.Deleted";
        public const string UserBlockChanged = "User.BlockChanged";
        public const string UserPasswordChanged = "User.PasswordChanged";
        public const string UserRoleChanged = "User.RoleChanged";
    }
}
