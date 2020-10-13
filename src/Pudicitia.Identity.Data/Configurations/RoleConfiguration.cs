using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pudicitia.Common.EntityFrameworkCore.Configurations;
using Pudicitia.Identity.Domain.Roles;

namespace Pudicitia.Identity.Data.Configurations
{
    public class RoleConfiguration : EntityConfiguration<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            base.Configure(builder);

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(32);

            builder.Metadata
                .FindNavigation(nameof(Role.UserRoles))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Metadata
                .FindNavigation(nameof(Role.RolePermissions))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}