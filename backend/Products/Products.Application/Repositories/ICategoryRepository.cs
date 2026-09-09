using Products.Domain;
using Shared.Abstractions.Repositories;

namespace Products.Application.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// Checks whether a category is exists.
        /// </summary>
        /// <param name="id">The id of the category to check.</param>
        /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains <see langword="true"/> if the category is exists; otherwise, <see langword="false"/>.
        /// </returns>
        Task<bool> IsExistsAsync(Guid id, CancellationToken token = default);
    }
}
