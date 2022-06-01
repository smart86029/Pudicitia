using Pudicitia.HR.Domain.Leaves;

namespace Pudicitia.HR.Data.Configurations;

public class LeaveConfiguration : EntityConfiguration<Leave>
{
    public override void Configure(EntityTypeBuilder<Leave> builder)
    {
        builder.HasIndex(x => x.EmployeeId);
    }
}
