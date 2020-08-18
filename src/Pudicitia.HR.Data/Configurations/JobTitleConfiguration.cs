using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pudicitia.Common.EntityFrameworkCore.Configurations;
using Pudicitia.HR.Domain.JobTitles;

namespace Pudicitia.HR.Data.Configurations
{
    public class JobTitleConfiguration : EntityConfiguration<JobTitle>
    {
        public override void Configure(EntityTypeBuilder<JobTitle> builder)
        {
            builder
               .Property(j => j.Name)
               .IsRequired()
               .HasMaxLength(32);
        }
    }
}