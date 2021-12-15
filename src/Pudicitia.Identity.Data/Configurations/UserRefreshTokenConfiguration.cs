using Pudicitia.Identity.Domain.Users;

namespace Pudicitia.Identity.Data.Configurations;

public class UserRefreshTokenConfiguration : EntityConfiguration<UserRefreshToken>
{
    public override void Configure(EntityTypeBuilder<UserRefreshToken> builder)
    {
        base.Configure(builder);

        builder
            .Property(x => x.RefreshToken)
            .IsRequired()
            .HasColumnType("char(24)");

        builder
            .HasIndex(x => x.RefreshToken)
            .IsUnique();
    }
}
