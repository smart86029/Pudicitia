using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pudicitia.Common.EntityFrameworkCore.Configurations;
using Pudicitia.Identity.Domain.Users;

namespace Pudicitia.Identity.Data.Configurations
{
    public class UserRoleConfiguration : EntityConfiguration<UserRole>
    {
        public override void Configure(EntityTypeBuilder<UserRole> builder)
        {
            base.Configure(builder);
        }
    }
}