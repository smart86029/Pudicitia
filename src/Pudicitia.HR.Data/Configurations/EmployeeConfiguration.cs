using Pudicitia.HR.Domain.Employees;

namespace Pudicitia.HR.Data.Configurations;

public class EmployeeConfiguration : EntityConfiguration<Employee>
{
    public override void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasIndex(x => x.DepartmentId);

        builder.HasIndex(x => x.JobId);
    }
}
