using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Domain;

namespace Products.Infrastructure.EntityConfiguration
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.CategoryId).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Price).HasPrecision(18, 2).IsRequired();
            builder.Property(x => x.GeneralNote).IsRequired();
            builder.Property(x => x.SpecialNote).IsRequired();

            builder.HasOne<Category>()
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
