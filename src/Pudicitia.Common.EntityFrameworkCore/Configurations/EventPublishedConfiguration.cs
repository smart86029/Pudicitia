using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pudicitia.Common.EntityFrameworkCore.Configurations;

public class EventPublishedConfiguration : IEntityTypeConfiguration<EventPublished>
{
    public void Configure(EntityTypeBuilder<EventPublished> builder)
    {
        builder.ToTable("EventPublished", "Common");

        builder.HasKey(x => x.EventId);

        builder
            .Property(x => x.EventTypeNamespace)
            .IsRequired()
            .HasMaxLength(256);

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
