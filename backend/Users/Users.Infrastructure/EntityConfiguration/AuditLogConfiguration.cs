using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain;

namespace Users.Infrastructure.EntityConfiguration
{
    internal class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Action).IsRequired();
            builder.Property(x => x.EntityId).IsRequired(false);
            builder.Property(x => x.CreatedAt).IsRequired();

            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.CreatedAt);
        }
    }
}
