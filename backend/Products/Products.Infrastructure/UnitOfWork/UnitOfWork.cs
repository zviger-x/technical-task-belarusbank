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
        public IAuditRepository AuditRepository => _auditRepository.Value;

        private readonly Lazy<IProductRepository> _productRepository;
        private readonly Lazy<ICategoryRepository> _categoryRepository;
        private readonly Lazy<IAuditRepository> _auditRepository;

        public UnitOfWork(ProductsDbContext context, IServiceProvider serviceProvider)
            : base(context, serviceProvider)
        {
            _productRepository = new Lazy<IProductRepository>(_serviceProvider.GetRequiredService<IProductRepository>);
            _categoryRepository = new Lazy<ICategoryRepository>(_serviceProvider.GetRequiredService<ICategoryRepository>);
            _auditRepository = new Lazy<IAuditRepository>(_serviceProvider.GetRequiredService<IAuditRepository>);
        }

        public override void Dispose()
        {
            base.Dispose();

            if (_productRepository.IsValueCreated)
                _productRepository.Value.Dispose();

            if (_categoryRepository.IsValueCreated)
                _categoryRepository.Value.Dispose();

            if (_auditRepository.IsValueCreated)
                _auditRepository.Value.Dispose();
        }
    }
}
