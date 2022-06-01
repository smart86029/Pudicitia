using Pudicitia.Identity.Data.Configurations;

namespace Pudicitia.Identity.Data;

public class IdentityContext : DbContext
{
    public IdentityContext(DbContextOptions<IdentityContext> options)
        : base(options)
    {
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

        var utcDateTimeConverter = new UtcDateTimeConverter();
        var dateTimeProperties = modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(x => x.GetProperties())
            .Where(x => x.ClrType == typeof(DateTime));
        foreach (var property in dateTimeProperties)
        {
            property.SetValueConverter(utcDateTimeConverter);
        }
    }
}
