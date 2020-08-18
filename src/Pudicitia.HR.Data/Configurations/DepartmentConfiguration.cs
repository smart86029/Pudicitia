using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pudicitia.Common.EntityFrameworkCore.Configurations;
using Pudicitia.HR.Domain.Departments;

namespace Pudicitia.HR.Data.Configurations
{
    public class DepartmentConfiguration : EntityConfiguration<Department>
    {
        public override void Configure(EntityTypeBuilder<Department> builder)
        {
            builder
                .Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(32);

            builder.HasIndex(d => d.ParentId);
        }
    }
}