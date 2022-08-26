using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pudicitia.Common.EntityFrameworkCore.Configurations;

public class EventSubscribedConfiguration : IEntityTypeConfiguration<EventSubscribed>
{
    public void Configure(EntityTypeBuilder<EventSubscribed> builder)
    {
        builder.ToTable("EventSubscribed", "Common");

        builder.HasKey(x => x.EventId);

        builder
            .Property(x => x.EventTypeName)
            .IsRequired()
            .HasMaxLength(256);

        builder
            .Property(x => x.EventContent)
            .IsRequired();

        builder
            .Property(x => x.CreatedOn)
            .IsRequired();

        builder.Ignore(x => x.Event);
    }
}
