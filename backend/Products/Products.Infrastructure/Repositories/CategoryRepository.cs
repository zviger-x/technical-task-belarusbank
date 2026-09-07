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
    }
}
