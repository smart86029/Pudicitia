using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pudicitia.Common.EntityFrameworkCore.Configurations;
using Pudicitia.Identity.Domain.Permissions;

namespace Pudicitia.Identity.Data.Configurations
{
    public class PermissionConfiguration : EntityConfiguration<Permission>
    {
        public override void Configure(EntityTypeBuilder<Permission> builder)
        {
            base.Configure(builder);

            builder
                .Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(32);

            builder
                .HasIndex(x => x.Code)
                .IsUnique();

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(32);

            builder
                .Property(x => x.Description)
                .HasMaxLength(128);
        }
    }
}