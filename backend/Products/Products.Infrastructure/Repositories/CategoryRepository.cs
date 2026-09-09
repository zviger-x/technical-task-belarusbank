using Microsoft.EntityFrameworkCore;
using Products.Application.Repositories;
using Products.Domain;
using Products.Infrastructure.Contexts;

namespace Products.Infrastructure.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ProductsDbContext context)
            : base(context)
        {
        }

        public Task<bool> IsExistsAsync(Guid id, CancellationToken token = default)
        {
            return _context.Categories.AnyAsync(u => u.Id == id, token);
        }
    }
}
