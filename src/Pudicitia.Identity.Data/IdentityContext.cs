using Pudicitia.Identity.Data.Configurations;

namespace Pudicitia.Identity.Data;

public class IdentityContext : DbContext
{
    public IdentityContext(DbContextOptions<IdentityContext> options)
        : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Configure();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("Identity")
            .ApplyConfiguration(new EventPublishedConfiguration())
            .ApplyConfiguration(new EventSubscribedConfiguration())
            .ApplyConfiguration(new UserConfiguration())
            .ApplyConfiguration(new RoleConfiguration())
            .ApplyConfiguration(new PermissionConfiguration())
            .ApplyConfiguration(new UserRoleConfiguration())
            .ApplyConfiguration(new UserRefreshTokenConfiguration())
            .ApplyConfiguration(new RolePermissionConfiguration())
            .Ignore<DomainEvent>();
    }
}
