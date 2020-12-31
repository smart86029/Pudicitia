using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pudicitia.Common.Events;

namespace Pudicitia.Common.EntityFrameworkCore.Configurations
{
    public class EventPublishedConfiguration : IEntityTypeConfiguration<EventPublished>
    {
        public void Configure(EntityTypeBuilder<EventPublished> builder)
        {
            builder.ToTable("EventPublished", "Common");

            builder.HasKey(e => e.EventId);

            builder
                .Property(e => e.EventTypeNamespace)
                .IsRequired()
                .HasMaxLength(256);

            builder
                .Property(e => e.EventTypeName)
                .IsRequired()
                .HasMaxLength(256);

            builder
                .Property(e => e.EventContent)
                .IsRequired();

            builder
                .Property(e => e.CreatedOn)
                .IsRequired();

            builder.Ignore(e => e.Event);

            builder.HasData(GetSeedData());
        }

        private object[] GetSeedData()
        {
            var result = new object[]
            {
            };

            return result;
        }
    }
}