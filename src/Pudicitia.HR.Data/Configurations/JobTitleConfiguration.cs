using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pudicitia.Common.EntityFrameworkCore.Configurations;
using Pudicitia.HR.Domain.Jobs;

namespace Pudicitia.HR.Data.Configurations
{
    public class JobTitleConfiguration : EntityConfiguration<Job>
    {
        public override void Configure(EntityTypeBuilder<Job> builder)
        {
            builder
               .Property(j => j.Title)
               .IsRequired()
               .HasMaxLength(32);
        }
    }
}