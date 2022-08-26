namespace Pudicitia.Common.Extensions;

public static class ConvertExtensions
{
    public static bool ToBool(this bool value)
    {
        return value;
    }

    public static bool ToBool(this char value)
    {
        return value == '0';
    }

    public static bool ToBool(this sbyte value)
    {
        return Convert.ToBoolean(value);
    }

    public static bool ToBool(this byte value)
    {
        return Convert.ToBoolean(value);
    }

    public static bool ToBool(this short value)
    {
        return Convert.ToBoolean(value);
    }

    public static bool ToBool(this ushort value)
    {
        return Convert.ToBoolean(value);
    }

    public static bool ToBool(this int value)
    {
        return Convert.ToBoolean(value);
    }

    public static bool ToBool(this uint value)
    {
        return Convert.ToBoolean(value);
    }

    public static bool ToBool(this long value)
    {
        return Convert.ToBoolean(value);
    }

    public static bool ToBool(this ulong value)
    {
        return Convert.ToBoolean(value);
    }

    public static bool ToBool(this float value)
    {
        return Convert.ToBoolean(value);
    }

    public static bool ToBool(this double value)
    {
        return Convert.ToBoolean(value);
    }

    public static bool ToBool(this decimal value)
    {
        return Convert.ToBoolean(value);
    }

    public static bool ToBool(this string value, bool defaultValue = default)
    {
        return bool.TryParse(value, out var result) ? result : defaultValue;
    }

    public static bool ToBool(this object value, bool defaultValue = default)
    {
        return TryConvert(() => Convert.ToBoolean(value), defaultValue);
    }

    public static char ToChar(this bool value)
    {
        return value ? '1' : '0';
    }

    public static char ToChar(this char value)
    {
        return Convert.ToChar(value);
    }

    public static char ToChar(this sbyte value, char defaultValue = default)
    {
        return TryConvert(() => Convert.ToChar(value), defaultValue);
    }

    public static char ToChar(this byte value)
    {
        return Convert.ToChar(value);
    }

    public static char ToChar(this short value, char defaultValue = default)
    {
        return TryConvert(() => Convert.ToChar(value), defaultValue);
    }

    public static char ToChar(this ushort value)
    {
        return Convert.ToChar(value);
    }

    public static char ToChar(this int value, char defaultValue = default)
    {
        return TryConvert(() => Convert.ToChar(value), defaultValue);
    }

    public static char ToChar(this uint value, char defaultValue = default)
    {
        return TryConvert(() => Convert.ToChar(value), defaultValue);
    }

    public static char ToChar(this long value, char defaultValue = default)
    {
        return TryConvert(() => Convert.ToChar(value), defaultValue);
    }

    public static char ToChar(this ulong value, char defaultValue = default)
    {
        return TryConvert(() => Convert.ToChar(value), defaultValue);
    }

    public static char ToChar(this float value, char defaultValue = default)
    {
        return TryConvert(() => Convert.ToChar(Convert.ToUInt16(value)), defaultValue);
    }

    public static char ToChar(this double value, char defaultValue = default)
    {
        return TryConvert(() => Convert.ToChar(Convert.ToUInt16(value)), defaultValue);
    }

    public static char ToChar(this decimal value, char defaultValue = default)
    {
        return TryConvert(() => Convert.ToChar(Convert.ToUInt16(value)), defaultValue);
    }

    public static char ToChar(this string value, char defaultValue = default)
    {
        return char.TryParse(value, out var result) ? result : defaultValue;
    }

    public static char ToChar(this object value, char defaultValue = default)
    {
        return TryConvert(() => Convert.ToChar(value), defaultValue);
    }

    public static sbyte ToSByte(this bool value)
    {
        return Convert.ToSByte(value);
    }

    public static sbyte ToSByte(this char value, sbyte defaultValue = default)
    {
        return TryConvert(() => Convert.ToSByte(value), defaultValue);
    }

    public static sbyte ToSByte(this sbyte value)
    {
        return Convert.ToSByte(value);
    }

    public static sbyte ToSByte(this byte value, sbyte defaultValue = default)
    {
        return value <= sbyte.MaxValue ? Convert.ToSByte(value) : defaultValue;
    }

    public static sbyte ToSByte(this short value, sbyte defaultValue = default)
    {
        return value.IsBetween(sbyte.MinValue, sbyte.MaxValue) ? Convert.ToSByte(value) : defaultValue;
    }

    public static sbyte ToSByte(this ushort value, sbyte defaultValue = default)
    {
        return value <= sbyte.MaxValue ? Convert.ToSByte(value) : defaultValue;
    }

    public static sbyte ToSByte(this int value, sbyte defaultValue = default)
    {
        return value.IsBetween(sbyte.MinValue, sbyte.MaxValue) ? Convert.ToSByte(value) : defaultValue;
    }

    public static sbyte ToSByte(this uint value, sbyte defaultValue = default)
    {
        return value <= sbyte.MaxValue ? Convert.ToSByte(value) : defaultValue;
    }

    public static sbyte ToSByte(this long value, sbyte defaultValue = default)
    {
        return value.IsBetween(sbyte.MinValue, sbyte.MaxValue) ? Convert.ToSByte(value) : defaultValue;
    }

    public static sbyte ToSByte(this ulong value, sbyte defaultValue = default)
    {
        return value <= (ulong)sbyte.MaxValue ? Convert.ToSByte(value) : defaultValue;
    }

    public static sbyte ToSByte(this float value, sbyte defaultValue = default)
    {
        return TryConvert(() => Convert.ToSByte(value), defaultValue);
    }

    public static sbyte ToSByte(this double value, sbyte defaultValue = default)
    {
        return TryConvert(() => Convert.ToSByte(value), defaultValue);
    }

    public static sbyte ToSByte(this decimal value, sbyte defaultValue = default)
    {
        return TryConvert(() => Convert.ToSByte(value), defaultValue);
    }

    public static sbyte ToSByte(this string value, sbyte defaultValue = default)
    {
        return sbyte.TryParse(value, out var result) ? result : defaultValue;
    }

    public static sbyte ToSByte(this object value, sbyte defaultValue = default)
    {
        return TryConvert(() => Convert.ToSByte(value), defaultValue);
    }

    public static byte ToByte(this bool value)
    {
        return Convert.ToByte(value);
    }

    public static byte ToByte(this char value, byte defaultValue = default)
    {
        return TryConvert(() => Convert.ToByte(value), defaultValue);
    }

    public static byte ToByte(this sbyte value, byte defaultValue = default)
    {
        return value >= byte.MinValue ? Convert.ToByte(value) : defaultValue;
    }

    public static byte ToByte(this byte value)
    {
        return value;
    }

    public static byte ToByte(this short value, byte defaultValue = default)
    {
        return value.IsBetween(byte.MinValue, byte.MaxValue) ? Convert.ToByte(value) : defaultValue;
    }

    public static byte ToByte(this ushort value, byte defaultValue = default)
    {
        return value.IsBetween(byte.MinValue, byte.MaxValue) ? Convert.ToByte(value) : defaultValue;
    }

    public static byte ToByte(this int value, byte defaultValue = default)
    {
        return value.IsBetween(byte.MinValue, byte.MaxValue) ? Convert.ToByte(value) : defaultValue;
    }

    public static byte ToByte(this uint value, byte defaultValue = default)
    {
        return value <= byte.MaxValue ? Convert.ToByte(value) : defaultValue;
    }

    public static byte ToByte(this long value, byte defaultValue = default)
    {
        return value.IsBetween(byte.MinValue, byte.MaxValue) ? Convert.ToByte(value) : defaultValue;
    }

    public static byte ToByte(this ulong value, byte defaultValue = default)
    {
        return value <= byte.MaxValue ? Convert.ToByte(value) : defaultValue;
    }

    public static byte ToByte(this float value, byte defaultValue = default)
    {
        return TryConvert(() => Convert.ToByte(value), defaultValue);
    }

    public static byte ToByte(this double value, byte defaultValue = default)
    {
        return TryConvert(() => Convert.ToByte(value), defaultValue);
    }

    public static byte ToByte(this decimal value, byte defaultValue = default)
    {
        return TryConvert(() => Convert.ToByte(value), defaultValue);
    }

    public static byte ToByte(this string value, byte defaultValue = default)
    {
        return byte.TryParse(value, out var result) ? result : defaultValue;
    }

    public static byte ToByte(this object value, byte defaultValue = default)
    {
        return TryConvert(() => Convert.ToByte(value), defaultValue);
    }

    public static short ToShort(this bool value)
    {
        return Convert.ToInt16(value);
    }

    public static short ToShort(this char value, sbyte defaultValue = default)
    {
        return TryConvert(() => Convert.ToInt16(value), defaultValue);
    }

    public static short ToShort(this sbyte value)
    {
        return Convert.ToInt16(value);
    }

    public static short ToShort(this byte value)
    {
        return Convert.ToInt16(value);
    }

    public static short ToShort(this short value)
    {
        return value;
    }

    public static short ToShort(this ushort value, short defaultValue = default)
    {
        return value <= short.MaxValue ? Convert.ToInt16(value) : defaultValue;
    }

    public static short ToShort(this int value, short defaultValue = default)
    {
        return value.IsBetween(short.MinValue, short.MaxValue) ? Convert.ToInt16(value) : defaultValue;
    }

    public static short ToShort(this uint value, short defaultValue = default)
    {
        return value <= short.MaxValue ? Convert.ToInt16(value) : defaultValue;
    }

    public static short ToShort(this long value, short defaultValue = default)
    {
        return value.IsBetween(short.MinValue, short.MaxValue) ? Convert.ToInt16(value) : defaultValue;
    }

    public static short ToShort(this ulong value, short defaultValue = default)
    {
        return value <= (ulong)short.MaxValue ? Convert.ToInt16(value) : defaultValue;
    }

    public static short ToShort(this float value, short defaultValue = default)
    {
        return TryConvert(() => Convert.ToInt16(value), defaultValue);
    }

    public static short ToShort(this double value, short defaultValue = default)
    {
        return TryConvert(() => Convert.ToInt16(value), defaultValue);
    }

    public static short ToShort(this decimal value, short defaultValue = default)
    {
        return TryConvert(() => Convert.ToInt16(value), defaultValue);
    }

    public static short ToShort(this string value, short defaultValue = default)
    {
        return short.TryParse(value, out var result) ? result : defaultValue;
    }

    public static short ToShort(this object value, short defaultValue = default)
    {
        return TryConvert(() => Convert.ToInt16(value), defaultValue);
    }

    public static ushort ToUShort(this bool value)
    {
        return Convert.ToUInt16(value);
    }

    public static ushort ToUShort(this char value)
    {
        return Convert.ToUInt16(value);
    }

    public static ushort ToUShort(this sbyte value, ushort defaultValue = default)
    {
        return value >= ushort.MinValue ? Convert.ToUInt16(value) : defaultValue;
    }

    public static ushort ToUShort(this byte value)
    {
        return Convert.ToUInt16(value);
    }

    public static ushort ToUShort(this short value, ushort defaultValue = default)
    {
        return value >= ushort.MinValue ? Convert.ToUInt16(value) : defaultValue;
    }

    public static ushort ToUShort(this ushort value)
    {
        return value;
    }

    public static ushort ToUShort(this int value, ushort defaultValue = default)
    {
        return value.IsBetween(ushort.MinValue, ushort.MaxValue) ? Convert.ToUInt16(value) : defaultValue;
    }

    public static ushort ToUShort(this uint value, ushort defaultValue = default)
    {
        return value <= ushort.MaxValue ? Convert.ToUInt16(value) : defaultValue;
    }

    public static ushort ToUShort(this long value, ushort defaultValue = default)
    {
        return value.IsBetween(ushort.MinValue, ushort.MaxValue) ? Convert.ToUInt16(value) : defaultValue;
    }

    public static ushort ToUShort(this ulong value, ushort defaultValue = default)
    {
        return value <= ushort.MaxValue ? Convert.ToUInt16(value) : defaultValue;
    }

    public static ushort ToUShort(this float value, ushort defaultValue = default)
    {
        return TryConvert(() => Convert.ToUInt16(value), defaultValue);
    }

    public static ushort ToUShort(this double value, ushort defaultValue = default)
    {
        return TryConvert(() => Convert.ToUInt16(value), defaultValue);
    }

    public static ushort ToUShort(this decimal value, ushort defaultValue = default)
    {
        return TryConvert(() => Convert.ToUInt16(value), defaultValue);
    }

    public static ushort ToUShort(this string value, ushort defaultValue = default)
    {
        return ushort.TryParse(value, out var result) ? result : defaultValue;
    }

    public static ushort ToUShort(this object value, ushort defaultValue = default)
    {
        return TryConvert(() => Convert.ToUInt16(value), defaultValue);
    }

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

    public static int ToInt(this byte value)
    {
        return Convert.ToInt32(value);
    }

    public static int ToInt(this short value)
    {
        return Convert.ToInt32(value);
    }

    public static int ToInt(this ushort value)
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

    public static uint ToUInt(this bool value)
    {
        return Convert.ToUInt32(value);
    }

    public static uint ToUInt(this char value)
    {
        return Convert.ToUInt32(value);
    }

    public static uint ToUInt(this sbyte value, uint defaultValue = default)
    {
        return value >= uint.MinValue ? Convert.ToUInt32(value) : defaultValue;
    }

    public static uint ToUInt(this byte value)
    {
        return Convert.ToUInt32(value);
    }

    public static uint ToUInt(this short value, uint defaultValue = default)
    {
        return value >= uint.MinValue ? Convert.ToUInt32(value) : defaultValue;
    }

    public static uint ToUInt(this ushort value)
    {
        return Convert.ToUInt32(value);
    }

    public static uint ToUInt(this int value, uint defaultValue = default)
    {
        return value >= uint.MinValue ? Convert.ToUInt32(value) : defaultValue;
    }

    public static uint ToUInt(this uint value)
    {
        return value;
    }

    public static uint ToUInt(this long value, uint defaultValue = default)
    {
        return value.IsBetween(uint.MinValue, uint.MaxValue) ? Convert.ToUInt32(value) : defaultValue;
    }

    public static uint ToUInt(this ulong value, uint defaultValue = default)
    {
        return value <= uint.MaxValue ? Convert.ToUInt32(value) : defaultValue;
    }

    public static uint ToUInt(this float value, uint defaultValue = default)
    {
        return TryConvert(() => Convert.ToUInt32(value), defaultValue);
    }

    public static uint ToUInt(this double value, uint defaultValue = default)
    {
        return TryConvert(() => Convert.ToUInt32(value), defaultValue);
    }

    public static uint ToUInt(this decimal value, uint defaultValue = default)
    {
        return TryConvert(() => Convert.ToUInt32(value), defaultValue);
    }

    public static uint ToUInt(this string value, uint defaultValue = default)
    {
        return uint.TryParse(value, out var result) ? result : defaultValue;
    }

    public static uint ToUInt(this object value, uint defaultValue = default)
    {
        return TryConvert(() => Convert.ToUInt32(value), defaultValue);
    }

    public static long ToLong(this bool value)
    {
        return Convert.ToInt64(value);
    }

    public static long ToLong(this char value)
    {
        return Convert.ToInt64(value);
    }

    public static long ToLong(this sbyte value)
    {
        return Convert.ToInt64(value);
    }

    public static long ToLong(this byte value)
    {
        return Convert.ToInt64(value);
    }

    public static long ToLong(this short value)
    {
        return Convert.ToInt64(value);
    }

    public static long ToLong(this ushort value)
    {
        return Convert.ToInt64(value);
    }

    public static long ToLong(this int value)
    {
        return Convert.ToInt64(value);
    }

    public static long ToLong(this uint value)
    {
        return Convert.ToInt64(value);
    }

    public static long ToLong(this long value)
    {
        return value;
    }

    public static long ToLong(this ulong value, long defaultValue = default)
    {
        return value <= long.MaxValue ? Convert.ToInt64(value) : defaultValue;
    }

    public static long ToLong(this float value, long defaultValue = default)
    {
        return TryConvert(() => Convert.ToInt64(value), defaultValue);
    }

    public static long ToLong(this double value, long defaultValue = default)
    {
        return TryConvert(() => Convert.ToInt64(value), defaultValue);
    }

    public static long ToLong(this decimal value, long defaultValue = default)
    {
        return TryConvert(() => Convert.ToInt64(value), defaultValue);
    }

    public static long ToLong(this string value, long defaultValue = default)
    {
        return long.TryParse(value, out var result) ? result : defaultValue;
    }

    public static long ToLong(this object value, long defaultValue = default)
    {
        return TryConvert(() => Convert.ToInt64(value), defaultValue);
    }

    public static ulong ToULong(this bool value)
    {
        return Convert.ToUInt64(value);
    }

    public static ulong ToULong(this char value)
    {
        return Convert.ToUInt64(value);
    }

    public static ulong ToULong(this sbyte value, ulong defaultValue = default)
    {
        return value >= (sbyte)ulong.MinValue ? Convert.ToUInt64(value) : defaultValue;
    }

    public static ulong ToULong(this byte value)
    {
        return Convert.ToUInt64(value);
    }

    public static ulong ToULong(this short value, ulong defaultValue = default)
    {
        return value >= (short)ulong.MinValue ? Convert.ToUInt64(value) : defaultValue;
    }

    public static ulong ToULong(this ushort value)
    {
        return Convert.ToUInt64(value);
    }

    public static ulong ToULong(this int value, ulong defaultValue = default)
    {
        return value >= (int)ulong.MinValue ? Convert.ToUInt64(value) : defaultValue;
    }

    public static ulong ToULong(this uint value)
    {
        return Convert.ToUInt64(value);
    }

    public static ulong ToULong(this long value, ulong defaultValue = default)
    {
        return value >= (long)ulong.MinValue ? Convert.ToUInt64(value) : defaultValue;
    }

    public static ulong ToULong(this ulong value)
    {
        return value;
    }

    public static ulong ToULong(this float value, ulong defaultValue = default)
    {
        return TryConvert(() => Convert.ToUInt64(value), defaultValue);
    }

    public static ulong ToULong(this double value, ulong defaultValue = default)
    {
        return TryConvert(() => Convert.ToUInt64(value), defaultValue);
    }

    public static ulong ToULong(this decimal value, ulong defaultValue = default)
    {
        return TryConvert(() => Convert.ToUInt64(value), defaultValue);
    }

    public static ulong ToULong(this string value, ulong defaultValue = default)
    {
        return ulong.TryParse(value, out var result) ? result : defaultValue;
    }

    public static ulong ToULong(this object value, ulong defaultValue = default)
    {
        return TryConvert(() => Convert.ToUInt64(value), defaultValue);
    }

    public static float ToFloat(this bool value)
    {
        return Convert.ToSingle(value);
    }

    public static float ToFloat(this char value)
    {
        return Convert.ToUInt16(value);
    }

    public static float ToFloat(this sbyte value)
    {
        return Convert.ToSingle(value);
    }

    public static float ToFloat(this byte value)
    {
        return Convert.ToSingle(value);
    }

    public static float ToFloat(this short value)
    {
        return Convert.ToSingle(value);
    }

    public static float ToFloat(this ushort value)
    {
        return Convert.ToSingle(value);
    }

    public static float ToFloat(this int value)
    {
        return Convert.ToSingle(value);
    }

    public static float ToFloat(this uint value)
    {
        return Convert.ToSingle(value);
    }

    public static float ToFloat(this long value)
    {
        return Convert.ToSingle(value);
    }

    public static float ToFloat(this ulong value)
    {
        return Convert.ToSingle(value);
    }

    public static float ToFloat(this float value)
    {
        return value;
    }

    public static float ToFloat(this double value)
    {
        return Convert.ToSingle(value);
    }

    public static float ToFloat(this decimal value)
    {
        return Convert.ToSingle(value);
    }

    public static float ToFloat(this string value, float defaultValue = default)
    {
        return float.TryParse(value, out var result) ? result : defaultValue;
    }

    public static float ToFloat(this object value, float defaultValue = default)
    {
        return TryConvert(() => Convert.ToSingle(value), defaultValue);
    }

    public static double ToDouble(this bool value)
    {
        return Convert.ToDouble(value);
    }

    public static double ToDouble(this char value)
    {
        return Convert.ToUInt16(value);
    }

    public static double ToDouble(this sbyte value)
    {
        return Convert.ToDouble(value);
    }

    public static double ToDouble(this byte value)
    {
        return Convert.ToDouble(value);
    }

    public static double ToDouble(this short value)
    {
        return Convert.ToDouble(value);
    }

    public static double ToDouble(this ushort value)
    {
        return Convert.ToDouble(value);
    }

    public static double ToDouble(this int value)
    {
        return Convert.ToDouble(value);
    }

    public static double ToDouble(this uint value)
    {
        return Convert.ToDouble(value);
    }

    public static double ToDouble(this long value)
    {
        return Convert.ToDouble(value);
    }

    public static double ToDouble(this ulong value)
    {
        return Convert.ToDouble(value);
    }

    public static double ToDouble(this float value)
    {
        return Convert.ToDouble(value);
    }

    public static double ToDouble(this double value)
    {
        return value;
    }

    public static double ToDouble(this decimal value)
    {
        return Convert.ToDouble(value);
    }

    public static double ToDouble(this string value, double defaultValue = default)
    {
        return double.TryParse(value, out var result) ? result : defaultValue;
    }

    public static double ToDouble(this object value, double defaultValue = default)
    {
        return TryConvert(() => Convert.ToDouble(value), defaultValue);
    }

    public static decimal ToDecimal(this bool value)
    {
        return Convert.ToDecimal(value);
    }

    public static decimal ToDecimal(this char value)
    {
        return Convert.ToUInt16(value);
    }

    public static decimal ToDecimal(this sbyte value)
    {
        return Convert.ToDecimal(value);
    }

    public static decimal ToDecimal(this byte value)
    {
        return Convert.ToDecimal(value);
    }

    public static decimal ToDecimal(this short value)
    {
        return Convert.ToDecimal(value);
    }

    public static decimal ToDecimal(this ushort value)
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

    public static decimal ToDecimal(this float value, decimal defaultValue = default)
    {
        return TryConvert(() => Convert.ToDecimal(value), defaultValue);
    }

    public static decimal ToDecimal(this double value, decimal defaultValue = default)
    {
        return TryConvert(() => Convert.ToDecimal(value), defaultValue);
    }

    public static decimal ToDecimal(this decimal value)
    {
        return value;
    }

    public static decimal ToDecimal(this string value, decimal defaultValue = default)
    {
        return decimal.TryParse(value, out var result) ? result : defaultValue;
    }

    public static decimal ToDecimal(this object value, decimal defaultValue = default)
    {
        return TryConvert(() => Convert.ToDecimal(value), defaultValue);
    }

    public static Guid ToGuid(this string value, Guid defaultValue = default)
    {
        return Guid.TryParse(value, out var result) ? result : defaultValue;
    }

    public static DateTime ToDateTime(this string value, DateTime defaultValue = default)
    {
        return DateTime.TryParse(value, out var result) ? result : defaultValue;
    }

    public static DateTime ToDateTime(this DateOnly value)
    {
        return value.ToDateTime(TimeOnly.MinValue);
    }

    public static DateOnly ToDateOnly(this string value, DateOnly defaultValue = default)
    {
        return DateOnly.TryParse(value, out var result) ? result : defaultValue;
    }

    public static DateOnly ToDateOnly(this DateTime value)
    {
        return DateOnly.FromDateTime(value);
    }

    public static TimeOnly ToTimeOnly(this string value, TimeOnly defaultValue = default)
    {
        return TimeOnly.TryParse(value, out var result) ? result : defaultValue;
    }

    public static TimeOnly ToTimeOnly(this DateTime value)
    {
        return TimeOnly.FromDateTime(value);
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
