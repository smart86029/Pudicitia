using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pudicitia.Common.EntityFrameworkCore.Configurations;
using Pudicitia.HR.Domain.Employees;

namespace Pudicitia.HR.Data.Configurations
{
    public class JobChangeConfiguration : EntityConfiguration<JobChange>
    {
        public override void Configure(EntityTypeBuilder<JobChange> builder)
        {
            builder.HasIndex(j => j.DepartmentId);

            builder.HasIndex(j => j.JobTitleId);
        }
    }
}