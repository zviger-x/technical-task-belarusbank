using Shared.Repositories;
using Users.Domain;

namespace Users.Application.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Checks whether a user is blocked based on their email address.
        /// </summary>
        /// <param name="email">The email address of the user to check.</param>
        /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains <see langword="true"/> if the user is blocked; otherwise, <see langword="false"/>.
        /// </returns>
        Task<bool> IsUserBlockedAsync(string email, CancellationToken token = default);
    }
}
