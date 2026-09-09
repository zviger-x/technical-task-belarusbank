using Products.Application.Contracts;
using Products.Domain;
using Shared.Abstractions.Repositories;
using Shared.Common;

namespace Products.Application.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        /// <summary>
        /// Returns a paged collection of products matching the specified filter.
        /// </summary>
        /// <param name="name">Product name filter.</param>
        /// <param name="description">Product description filter.</param>
        /// <param name="generalNote">General note filter.</param>
        /// <param name="specialNote">Special note filter.</param>
        /// <param name="categoryId">Category identifier filter.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="token">Cancellation token to cancel the operation if needed.</param>
        /// <returns>A paged collection of products matching the specified filter.</returns>
        Task<PagedCollection<Product>> GetPagedWithFilterAsync(
            string name,
            string description,
            string generalNote,
            string specialNote,
            Guid? categoryId,
            int pageNumber,
            int pageSize,
            CancellationToken token = default);


        /// <summary>
        /// Returns a collection of products matching the specified filter.
        /// </summary>
        /// <param name="name">Product name filter.</param>
        /// <param name="description">Product description filter.</param>
        /// <param name="generalNote">General note filter.</param>
        /// <param name="specialNote">Special note filter.</param>
        /// <param name="categoryId">Category identifier filter.</param>
        /// <param name="token">Cancellation token to cancel the operation if needed.</param>
        /// <returns>A paged collection of products matching the specified filter.</returns>
        Task<IEnumerable<ProductCatalogItemDto>> GetForCatalogAsync(
            string name,
            string description,
            string generalNote,
            string specialNote,
            Guid? categoryId,
            CancellationToken token = default);
    }
}
