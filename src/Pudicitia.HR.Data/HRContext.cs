using Pudicitia.HR.Data.Configurations;

namespace Pudicitia.HR.Data;

public class HRContext : DbContext
{
    public HRContext(DbContextOptions<HRContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("HR")
            .Ignore<DomainEvent>()
            .ApplyConfiguration(new EventPublishedConfiguration())
            .ApplyConfiguration(new EventSubscribedConfiguration())
            .ApplyConfiguration(new PersonConfiguration())
            .ApplyConfiguration(new EmployeeConfiguration())
            .ApplyConfiguration(new DepartmentConfiguration())
            .ApplyConfiguration(new JobTitleConfiguration())
            .ApplyConfiguration(new JobChangeConfiguration());

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
