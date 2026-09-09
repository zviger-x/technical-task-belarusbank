using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Products.Infrastructure.Contexts;
using Shared.Abstractions.Repositories;
using Shared.Abstractions.UnitOfWork;
using Shared.Entities;

namespace Products.Infrastructure.UnitOfWork
{
    public abstract class BaseUnitOfWork : IBaseUnitOfWork
    {
        protected readonly ProductsDbContext _context;
        protected readonly IServiceProvider _serviceProvider;

        protected IDbContextTransaction _transaction;

        private readonly Dictionary<Type, object> _repositories;

        public BaseUnitOfWork(ProductsDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;

            _repositories = new();
        }

        public IRepository<T> Repository<T>()
            where T : class, IEntity
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = _serviceProvider.GetRequiredService<IRepository<T>>();
                _repositories[type] = repositoryInstance;
            }

            return (IRepository<T>)_repositories[type];
        }

        public virtual Task InvokeWithTransactionAsync(Func<CancellationToken, Task> action, CancellationToken token = default)
        {
            return InvokeWithTransactionAsync(
                async ct =>
                {
                    await action(ct);
                    return true;
                },
                token);
        }

        public virtual async Task<T> InvokeWithTransactionAsync<T>(Func<CancellationToken, Task<T>> action, CancellationToken token = default)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(token);

            try
            {
                var result = await action(token);

                await transaction.CommitAsync(token);

                return result;
            }
            catch
            {
                await transaction.RollbackAsync(token);
                throw;
            }
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync();
        }

        public virtual void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
    }
}
