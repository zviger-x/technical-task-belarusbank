namespace Products.Application.Clients
{
    public interface IUserClient
    {
        /// <summary>
        /// Checks whether a user is blocked based on their email address.
        /// </summary>
        /// <param name="id">The id of the user to check.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains <see langword="true"/> if the user is blocked; otherwise, <see langword="false"/>.
        /// </returns>
        Task<bool> IsUserBlockedAsync(Guid id, CancellationToken cancellationToken);
    }
}
