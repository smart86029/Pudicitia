using Pudicitia.HR.Data.Configurations;

namespace Pudicitia.HR.Data;

public class HRContext : DbContext
{
    public HRContext(DbContextOptions<HRContext> options)
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
            .HasDefaultSchema("HR")
            .ApplyConfiguration(new EventPublishedConfiguration())
            .ApplyConfiguration(new EventSubscribedConfiguration())
            .ApplyConfiguration(new PersonConfiguration())
            .ApplyConfiguration(new EmployeeConfiguration())
            .ApplyConfiguration(new DepartmentConfiguration())
            .ApplyConfiguration(new JobTitleConfiguration())
            .ApplyConfiguration(new JobChangeConfiguration())
            .ApplyConfiguration(new LeaveConfiguration())
            .Ignore<DomainEvent>();
    }
}
