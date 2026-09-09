using Shared.Common;
using Shared.Enums;
using System.Security.Claims;

namespace Shared.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Creates a <see cref="UserContext"/> from the authenticated user's claims.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the user ID or role claim is missing or has an invalid value.
        /// </exception>
        public static UserContext GetUserContext(this ClaimsPrincipal principal)
        {
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var id))
                throw new InvalidOperationException("Authenticated user does not have a valid user ID claim.");

            var role = principal.FindFirstValue(ClaimTypes.Role);
            if (!Enum.TryParse<UserRoles>(role, out var userRole))
                throw new InvalidOperationException("Authenticated user does not have a valid role claim.");

            return new UserContext(id, userRole);
        }
    }
}
