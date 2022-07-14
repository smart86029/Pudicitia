namespace Pudicitia.Common.Extensions;

public static class ConvertExtensions
{
    // From:  To: Bol Chr SBy Byt I16 U16 I32 U32 I64 U64 Sgl Dbl Dec Dat Str
    // ----------------------------------------------------------------------
    // Boolean     x       x   x   x   x   x   x   x   x   x   x   x       x
    // Char            x   x   x   x   x   x   x   x   x                   x
    // SByte       x   x   x   x   x   x   x   x   x   x   x   x   x       x
    // Byte        x   x   x   x   x   x   x   x   x   x   x   x   x       x
    // Int16       x   x   x   x   x   x   x   x   x   x   x   x   x       x
    // UInt16      x   x   x   x   x   x   x   x   x   x   x   x   x       x
    // Int32       x   x   x   x   x   x   x   x   x   x   x   x   x       x
    // UInt32      x   x   x   x   x   x   x   x   x   x   x   x   x       x
    // Int64       x   x   x   x   x   x   x   x   x   x   x   x   x       x
    // UInt64      x   x   x   x   x   x   x   x   x   x   x   x   x       x
    // Single      x       x   x   x   x   x   x   x   x   x   x   x       x
    // Double      x       x   x   x   x   x   x   x   x   x   x   x       x
    // Decimal     x       x   x   x   x   x   x   x   x   x   x   x       x
    // DateTime                                                        x   x
    // String      x   x   x   x   x   x   x   x   x   x   x   x   x   x   x
    // ----------------------------------------------------------------------

    public static int ToInt(this bool value)
    {
        return Convert.ToInt32(value);
    }

    public static int ToInt(this char value)
    {
        return Convert.ToInt32(value);
    }

    public static int ToInt(this sbyte value)
    {
        return Convert.ToInt32(value);
    }

    public static int ToInt(this int value)
    {
        return Convert.ToInt32(value);
    }

    public static int ToInt(this uint value, int defaultValue = default)
    {
        return value <= int.MaxValue ? Convert.ToInt32(value) : defaultValue;
    }

    public static int ToInt(this long value, int defaultValue = default)
    {
        return value.IsBetween(int.MinValue, int.MaxValue) ? Convert.ToInt32(value) : defaultValue;
    }

    public static int ToInt(this ulong value, int defaultValue = default)
    {
        return value <= int.MaxValue ? Convert.ToInt32(value) : defaultValue;
    }

    public static int ToInt(this float value, int defaultValue = default)
    {
        return TryConvert(() => Convert.ToInt32(value), defaultValue);
    }

    public static int ToInt(this double value, int defaultValue = default)
    {
        return TryConvert(() => Convert.ToInt32(value), defaultValue);
    }

    public static int ToInt(this decimal value, int defaultValue = default)
    {
        return TryConvert(() => Convert.ToInt32(value), defaultValue);
    }

    public static int ToInt(this string value, int defaultValue = default)
    {
        return int.TryParse(value, out var result) ? result : defaultValue;
    }

    public static int ToInt(this object value, int defaultValue = default)
    {
        return TryConvert(() => Convert.ToInt32(value), defaultValue);
    }

    public static decimal ToDecimal(this bool value)
    {
        return Convert.ToDecimal(value);
    }

    public static decimal ToDecimal(this char value)
    {
        return Convert.ToInt32(value);
    }

    public static decimal ToDecimal(this sbyte value)
    {
        return Convert.ToDecimal(value);
    }

    public static decimal ToDecimal(this int value)
    {
        return Convert.ToDecimal(value);
    }

    public static decimal ToDecimal(this uint value)
    {
        return Convert.ToDecimal(value);
    }

    public static decimal ToDecimal(this long value)
    {
        return Convert.ToDecimal(value);
    }

    public static decimal ToDecimal(this ulong value)
    {
        return Convert.ToDecimal(value);
    }

    public static decimal ToDecimal(this float value, int defaultValue = default)
    {
        return TryConvert(() => Convert.ToDecimal(value), defaultValue);
    }

    public static decimal ToDecimal(this double value, int defaultValue = default)
    {
        return TryConvert(() => Convert.ToDecimal(value), defaultValue);
    }

    public static decimal ToDecimal(this decimal value)
    {
        return Convert.ToDecimal(value);
    }

    public static decimal ToDecimal(this string value, int defaultValue = default)
    {
        return decimal.TryParse(value, out var result) ? result : defaultValue;
    }

    public static decimal ToDecimal(this object value, int defaultValue = default)
    {
        return TryConvert(() => Convert.ToDecimal(value), defaultValue);
    }

    public static Guid ToGuid(this string value, Guid defaultValue = default)
    {
        return Guid.TryParse(value, out var result) ? result : defaultValue;
    }

    private static TResult TryConvert<TResult>(Func<TResult> func, TResult defaultValue)
    {
        try
        {
            return func.Invoke();
        }
        catch
        {
            return defaultValue;
        }
    }
}
