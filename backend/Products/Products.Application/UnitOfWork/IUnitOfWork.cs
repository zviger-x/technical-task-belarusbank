using Products.Application.Repositories;
using Shared.Abstractions.UnitOfWork;

namespace Products.Application.UnitOfWork
{
    public interface IUnitOfWork : IBaseUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
    }
}
