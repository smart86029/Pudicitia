using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Pudicitia.Common.EntityFrameworkCore.Converters;

public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    private static readonly Expression<Func<DateOnly, DateTime>> _convertTo = x => x.ToDateTime();
    private static readonly Expression<Func<DateTime, DateOnly>> _convertFrom = x => x.ToDateOnly();

    public DateOnlyConverter()
        : base(_convertTo, _convertFrom)
    {
    }
}
