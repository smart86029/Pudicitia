using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Pudicitia.Common.EntityFrameworkCore.Converters;

public class UtcDateTimeConverter : ValueConverter<DateTime, DateTime>
{
    private static readonly Expression<Func<DateTime, DateTime>> _convertTo = x => x.ToUniversalTime();
    private static readonly Expression<Func<DateTime, DateTime>> _convertFrom = x => DateTime.SpecifyKind(x, DateTimeKind.Utc);

    public UtcDateTimeConverter()
        : base(_convertTo, _convertFrom)
    {
    }
}
