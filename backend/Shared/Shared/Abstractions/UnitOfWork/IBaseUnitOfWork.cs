using Shared.Abstractions.Repositories;
using Shared.Entities;

namespace Shared.Abstractions.UnitOfWork
{
    public interface IBaseUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets a repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <returns>The repository for the specified entity type.</returns>
        IRepository<T> Repository<T>() where T : class, IEntity;

        /// <summary>
        /// Executes an action within a transaction.
        /// </summary>
        /// <param name="action">The action to execute within the transaction.</param>
        /// <param name="token">Cancellation token to cancel the operation if needed.</param>
        Task InvokeWithTransactionAsync(Func<CancellationToken, Task> action, CancellationToken token = default);

        /// <summary>
        /// Executes an action within a transaction.
        /// </summary>
        /// <param name="action">The action to execute within the transaction.</param>
        /// <param name="token">Cancellation token to cancel the operation if needed.</param>
        /// <returns>An object that can be returned within a transaction</returns>
        Task<T> InvokeWithTransactionAsync<T>(Func<CancellationToken, Task<T>> action, CancellationToken token = default);

        /// <summary>
        /// Asynchronously saves all changes made in this unit of work to the database.
        /// </summary>
        /// <param name="token">Cancellation token to cancel the operation if needed.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
