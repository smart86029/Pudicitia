namespace Pudicitia.Common.Extensions;

public static class ComparableExtensions
{
    public static bool IsBetween<TValue>(this TValue value, TValue minInclusive, TValue maxInclusive)
        where TValue : IComparable<TValue>
    {
        return value.CompareTo(minInclusive) >= 0 &&
            value.CompareTo(maxInclusive) <= 0;
    }

    public static bool IsBetween<TValue>(this TValue value, TValue minInclusive, TValue? maxInclusive)
        where TValue : struct, IComparable<TValue>
    {
        return value.CompareTo(minInclusive) >= 0 &&
            (!maxInclusive.HasValue || value.CompareTo(maxInclusive.Value) <= 0);
    }

    public static bool IsBetween<TValue>(this TValue value, TValue? minInclusive, TValue maxInclusive)
        where TValue : struct, IComparable<TValue>
    {
        return (!minInclusive.HasValue || value.CompareTo(minInclusive.Value) >= 0) &&
            value.CompareTo(maxInclusive) <= 0;
    }
}
