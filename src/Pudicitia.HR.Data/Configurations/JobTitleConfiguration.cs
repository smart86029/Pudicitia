using Pudicitia.HR.Domain.Jobs;

namespace Pudicitia.HR.Data.Configurations;

public class JobTitleConfiguration : EntityConfiguration<Job>
{
    public override void Configure(EntityTypeBuilder<Job> builder)
    {
        builder
           .Property(x => x.Title)
           .IsRequired()
           .HasMaxLength(32);
    }
}
