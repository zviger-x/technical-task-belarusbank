using Products.Domain;
using Shared.Abstractions.Repositories;

namespace Products.Application.Repositories
{
    public interface IProductRepository : IRepository<Product>;
}
