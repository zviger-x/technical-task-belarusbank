using Microsoft.EntityFrameworkCore;
using Products.Domain;

namespace Products.Infrastructure.Contexts
{
    public class ProductsDbContext : DbContext
    {
        public virtual DbSet<Product> Users { get; set; }
        public virtual DbSet<Category> RefreshTokens { get; set; }

        public ProductsDbContext(DbContextOptions<ProductsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductsDbContext).Assembly);
        }
    }
}
