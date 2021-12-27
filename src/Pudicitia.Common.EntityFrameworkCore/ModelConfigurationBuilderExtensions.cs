using Pudicitia.Common.EntityFrameworkCore.Converters;

namespace Pudicitia.Common.EntityFrameworkCore;

public static class ModelConfigurationBuilderExtensions
{
    public static void Configure(this ModelConfigurationBuilder builder)
    {
        builder
            .Properties<DateTime>()
            .HaveConversion<UtcDateTimeConverter>();
    }
}
