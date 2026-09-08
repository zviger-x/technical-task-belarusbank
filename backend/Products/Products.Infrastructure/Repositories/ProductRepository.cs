using Microsoft.EntityFrameworkCore;
using Products.Application.Repositories;
using Products.Domain;
using Products.Infrastructure.Contexts;
using Shared.Common;

namespace Products.Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ProductsDbContext context)
            : base(context)
        {
        }

        public async Task<PagedCollection<Product>> GetPagedWithFilterAsync(
            string name,
            string description,
            string generalNote,
            string specialNote,
            Guid? categoryId,
            int pageNumber,
            int pageSize,
            CancellationToken token = default)
        {
            var query = _context.Products.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => x.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(description))
                query = query.Where(x => x.Description.Contains(description));

            if (!string.IsNullOrWhiteSpace(generalNote))
                query = query.Where(x => x.GeneralNote.Contains(generalNote));

            if (!string.IsNullOrWhiteSpace(specialNote))
                query = query.Where(x => x.SpecialNote.Contains(specialNote));

            if (categoryId.HasValue)
                query = query.Where(x => x.CategoryId == categoryId.Value);

            var totalCount = await query.CountAsync(token);
            var totalPages = (int)Math.Ceiling(totalCount / (float)pageSize);

            var page = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(token);

            return new PagedCollection<Product>
            {
                Items = page,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
