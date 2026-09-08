using Microsoft.EntityFrameworkCore;
using Products.Infrastructure.Contexts;

namespace Products.Infrastructure.Initialization
{
    public sealed class DatabaseInitializer
    {
        private readonly ProductsDbContext _context;

        public DatabaseInitializer(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.MigrateAsync(cancellationToken);
        }
    }
}
