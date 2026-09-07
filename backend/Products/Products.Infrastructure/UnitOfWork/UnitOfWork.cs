using Microsoft.Extensions.DependencyInjection;
using Products.Application.Repositories;
using Products.Application.UnitOfWork;
using Products.Infrastructure.Contexts;

namespace Products.Infrastructure.UnitOfWork
{
    public class UnitOfWork : BaseUnitOfWork, IUnitOfWork
    {
        public IProductRepository ProductRepository => _productRepository.Value;
        public ICategoryRepository CategoryRepository => _categoryRepository.Value;

        private readonly Lazy<IProductRepository> _productRepository;
        private readonly Lazy<ICategoryRepository> _categoryRepository;

        public UnitOfWork(ProductsDbContext context, IServiceProvider serviceProvider)
            : base(context, serviceProvider)
        {
            _productRepository = new Lazy<IProductRepository>(_serviceProvider.GetRequiredService<IProductRepository>);
            _categoryRepository = new Lazy<ICategoryRepository>(_serviceProvider.GetRequiredService<ICategoryRepository>);
        }

        public override void Dispose()
        {
            base.Dispose();

            if (_productRepository.IsValueCreated)
                _productRepository.Value.Dispose();

            if (_categoryRepository.IsValueCreated)
                _categoryRepository.Value.Dispose();
        }
    }
}
