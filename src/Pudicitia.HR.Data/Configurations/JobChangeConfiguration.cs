using Pudicitia.HR.Domain.Employees;

namespace Pudicitia.HR.Data.Configurations;

public class JobChangeConfiguration : EntityConfiguration<JobChange>
{
    public override void Configure(EntityTypeBuilder<JobChange> builder)
    {
        builder.HasIndex(x => x.DepartmentId);

        builder.HasIndex(x => x.JobTitleId);
    }
}
