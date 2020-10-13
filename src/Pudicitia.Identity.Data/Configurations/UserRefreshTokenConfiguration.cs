using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pudicitia.Common.EntityFrameworkCore.Configurations;
using Pudicitia.Identity.Domain.Users;

namespace Pudicitia.Identity.Data.Configurations
{
    public class UserRefreshTokenConfiguration : EntityConfiguration<UserRefreshToken>
    {
        public override void Configure(EntityTypeBuilder<UserRefreshToken> builder)
        {
            base.Configure(builder);

            builder
                .Property(t => t.RefreshToken)
                .IsRequired()
                .HasColumnType("char(24)");

            builder
                .HasIndex(t => t.RefreshToken)
                .IsUnique();
        }
    }
}