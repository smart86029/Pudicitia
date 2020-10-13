using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pudicitia.Common.EntityFrameworkCore.Configurations;
using Pudicitia.Identity.Domain.Roles;

namespace Pudicitia.Identity.Data.Configurations
{
    public class RolePermissionConfiguration : EntityConfiguration<RolePermission>
    {
        public override void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            base.Configure(builder);
        }
    }
}