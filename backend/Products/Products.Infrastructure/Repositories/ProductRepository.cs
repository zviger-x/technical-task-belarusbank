using Products.Application.Repositories;
using Products.Domain;
using Products.Infrastructure.Contexts;

namespace Products.Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ProductsDbContext context)
            : base(context)
        {
        }
    }
}
