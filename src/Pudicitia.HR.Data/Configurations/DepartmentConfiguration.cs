using Pudicitia.HR.Domain.Departments;

namespace Pudicitia.HR.Data.Configurations;

public class DepartmentConfiguration : EntityConfiguration<Department>
{
    public override void Configure(EntityTypeBuilder<Department> builder)
    {
        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(32);

        builder.HasIndex(x => x.ParentId);
    }
}
