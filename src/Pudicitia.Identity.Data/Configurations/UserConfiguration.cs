using Pudicitia.Identity.Domain.Users;

namespace Pudicitia.Identity.Data.Configurations;

public class UserConfiguration : EntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder
            .Property(x => x.UserName)
            .IsRequired()
            .HasMaxLength(32);

        builder
            .HasIndex(x => x.UserName)
            .IsUnique();

        builder
            .Property(x => x.Salt)
            .IsRequired()
            .HasMaxLength(64);

        builder
            .Property(x => x.PasswordHash)
            .IsRequired()
            .HasMaxLength(256);

        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(32);

        builder
            .Property(x => x.DisplayName)
            .IsRequired()
            .HasMaxLength(32);
    }
}
