using Pudicitia.HR.Domain;

namespace Pudicitia.HR.Data.Configurations;

public class PersonConfiguration : EntityConfiguration<Person>
{
    public override void Configure(EntityTypeBuilder<Person> builder)
    {
        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(32);

        builder
            .Property(x => x.DisplayName)
            .IsRequired()
            .HasMaxLength(32);

        builder
            .Property(x => x.BirthDate)
            .HasColumnType("date");
    }
}
